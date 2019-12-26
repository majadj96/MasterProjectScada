using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.Connection;
using ScadaCommon.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DNP3;

namespace ProcessingModule
{
    /// <summary>
    /// Class containing logic for sending modbus requests and receiving point values. 
    /// </summary>
    public class FunctionExecutor : IDisposable, IFunctionExecutor
    {
        private IConnection connection;
        private IDNP3Functions currentCommand;
        private bool threadCancellationSignal = true;
        private AutoResetEvent processConnection;
        private Thread connectionProcessorThread;
        private ConcurrentQueue<IDNP3Functions> commandQueue = new ConcurrentQueue<IDNP3Functions>();
        private IConfiguration configuration;

        /// <inheritdoc />
        public event UpdatePointDelegate UpdatePointEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionExecutor"/> class.
        /// </summary>
        /// <param name="stateUpdater">The state updater.</param>
        /// <param name="configuration">The configuration.</param>
		public FunctionExecutor(IConfiguration configuration, IConnection connection)
        {
            MessagesForUnsolicited();
            this.configuration = configuration;
            this.connection = connection;
            this.processConnection = new AutoResetEvent(true);
            connectionProcessorThread = new Thread(new ThreadStart(ConnectionProcessorThread));
            connectionProcessorThread.Name = "Communication thread";
            connectionProcessorThread.Start();
        }

        /// <inheritdoc />
        public void EnqueueCommand(IDNP3Functions commandToExecute)
        {
            if (connection.ConnectionState == ConnectionState.CONNECTED)
            {
                this.commandQueue.Enqueue(commandToExecute);
                this.processConnection.Set();
            }
        }

        public void HandleReceivedChangesOfPoints(Dictionary<Tuple<PointType, ushort>, ushort> pointsToupdate)
        {
            if (UpdatePointEvent != null)
            {
                foreach (var point in pointsToupdate)
                {
                    UpdatePointEvent.Invoke(point.Key.Item1, point.Key.Item2, point.Value);
                }
            }
        }

        public void MessagesForUnsolicited()
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc0, (byte)DNP3FunctionCode.DISABLE_UNSOLICITED, 0x00, 0x06, 0, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc0);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Message(p);
            commandQueue.Enqueue(fn);

            p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.WRITE, (ushort)TypeField.INTERNAL_INDICATIONS, 0x00, 0x0707, 0x00, 0x00, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc2);
            fn = DNP3FunctionFactory.CreateDNP3Message(p);
            commandQueue.Enqueue(fn);

            p = new DNP3ApplicationObjectParameters(0xc3, (byte)DNP3FunctionCode.READ, 0x00, 0x06, 0, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc4);
            fn = DNP3FunctionFactory.CreateDNP3Message(p);
            commandQueue.Enqueue(fn);

            p = new DNP3ApplicationObjectParameters(0xc5, (byte)DNP3FunctionCode.WRITE, (ushort)TypeField.TIME_MESSAGE, 0x07, 0x0001, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc6);
            fn = DNP3FunctionFactory.CreateDNP3Message(p);
            commandQueue.Enqueue(fn);

            p = new DNP3ApplicationObjectParameters(0xc6, (byte)DNP3FunctionCode.ENABLE_UNSOLICITED, 0x00, 0x06, 0, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc7);
            fn = DNP3FunctionFactory.CreateDNP3Message(p);
            commandQueue.Enqueue(fn);
        }

        /// <summary>
        /// Logic for handling the connection.
        /// </summary>
		private void ConnectionProcessorThread()
        {
            while (this.threadCancellationSignal)
            {
                try
                {
                    if (connection.ConnectionState == ConnectionState.CONNECTED)
                    {
                        if (!commandQueue.IsEmpty)
                        {
                            while (commandQueue.TryDequeue(out currentCommand))
                            {
                                this.connection.SendBytes(this.currentCommand.PackRequest());
                                bool recvAgain = true;
                                byte[] message;

                                while (recvAgain)
                                {
                                    byte[] header = this.connection.RecvBytes(10);
                                    byte payLoadSize = 0;
                                    int len = 0;
                                    unchecked
                                    {
                                        payLoadSize = (byte)BitConverter.ToChar(header, 2);
                                    }
                                    byte[] payload;

                                    //Duzina poruke posle header-a (heder je duzine 5) racuna se tako sto od ukupne duzine oduzmemo header
                                    // i na tu duzinu dodajemo duzinu svih crc-ova koji su na svakih 16 bajtova
                                    payLoadSize = (byte)(payLoadSize - 5);

                                    if (payLoadSize % 16 == 0)
                                    {
                                        len = payLoadSize + (payLoadSize / 16) * 2;
                                    }
                                    else
                                    {
                                        len = (payLoadSize / 16) == 0 ? (byte)(payLoadSize + 2) : (byte)(payLoadSize + (payLoadSize / 16) * 2 + 2);
                                    }

                                    payload = this.connection.RecvBytes(len);
                                    
                                    message = new byte[header.Length + payload.Length];
                                    Buffer.BlockCopy(header, 0, message, 0, 10);
                                    Buffer.BlockCopy(payload, 0, message, 10, payload.Length);

                                    //na dvanaestom byte se nalazi u responsu da li je unsolicited ili ne (tacnije u njemu jer je jedan bit)
                                    if (!ChechIfUnsolicited(message[11]))
                                    {
                                        recvAgain = false;
                                    }

                                    this.ProccessMsg(message, len + 10);
                                }
                             this.currentCommand = null;
                            }
                        }
                        else
                        {
                            if (this.connection.ReadReady())
                            {
                                byte[] message;
                                byte[] header = this.connection.RecvBytes(10);
                                byte payLoadSize = 0;
                                int len = 0;
                                unchecked
                                {
                                    payLoadSize = (byte)BitConverter.ToChar(header, 2);
                                }
                                byte[] payload;

                                //Duzina poruke posle header-a (heder je duzine 5) racuna se tako sto od ukupne duzine oduzmemo header
                                // i na tu duzinu dodajemo duzinu svih crc-ova koji su na svakih 16 bajtova
                                payLoadSize = (byte)(payLoadSize - 5);

                                if (payLoadSize % 16 == 0)
                                {
                                    len = payLoadSize + (payLoadSize / 16) * 2;
                                }
                                else
                                {
                                    len = (payLoadSize / 16) == 0 ? (byte)(payLoadSize + 2) : (byte)(payLoadSize + (payLoadSize / 16) * 2 + 2);
                                }

                                payload = this.connection.RecvBytes(len);

                                message = new byte[header.Length + payload.Length];
                                Buffer.BlockCopy(header, 0, message, 0, 10);
                                Buffer.BlockCopy(payload, 0, message, 10, payload.Length);
                                this.ProccessMsg(message, len + 10);
                                this.currentCommand = null;
                            }
                        }
                        Thread.Sleep(30);
                    }
                    else
                    {
                        connection.PrepairConnection();
                        Thread.Sleep(30);
                    }
                }
                catch (SocketException se)
                {
                    if (se.ErrorCode != 10054)
                    {
                        throw se;
                    }
                    currentCommand = null;
                    connection.ConnectionState = ConnectionState.DISCONNECTED;
                    string message = $"{se.TargetSite.ReflectedType.Name}.{se.TargetSite.Name}: {se.Message}";
                }
                catch (Exception ex)
                {
                    string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
                    currentCommand = null;
                }
            }
        }

        private void ProccessMsg(byte[] message, int messageLength)
        {
            Dictionary<Tuple<PointType, ushort>, ushort> pointsToupdate = new Dictionary<Tuple<PointType, ushort>, ushort>();
            byte lengthData = (byte)(BitConverter.ToChar(message, 2) - 5);
            byte[] dataArray = new byte[lengthData];
            int byteProcessed = 0;
            byte qualifier, quality, controlStatus;

            ConfirmUnsMessage(message);

            PreproccessMsg(message, messageLength, ref dataArray, lengthData);

            byte transportControl = dataArray[byteProcessed++];
            byte appControl = dataArray[byteProcessed++];
            byte functionCode = dataArray[byteProcessed++];
            short interalIndications = BitConverter.ToInt16(dataArray, byteProcessed);
            byteProcessed += 2;

            short objectType, prefixMeaning, rangeMeaning;
            int prefixOffset = 0, rangeOffset = 0;
            int start = 0, stop = 0, objectNumber = -1, adress, regValue, value, i, objectIndex = -1;

            while (byteProcessed < lengthData)
            {
                objectType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(dataArray, byteProcessed));
                byteProcessed += 2;

                qualifier = dataArray[byteProcessed++];
                ProcessQualifier(qualifier, out prefixOffset, out prefixMeaning, out rangeOffset, out rangeMeaning);

                if (rangeOffset == 1)
                {
                    objectNumber = (byte)BitConverter.ToChar(dataArray, byteProcessed);
                    byteProcessed ++;
                }
                else if (rangeOffset == 2)
                {
                    if (rangeMeaning == (short)Qualifier.START_STOP_INDEX)
                    {
                        start = (byte)BitConverter.ToChar(dataArray, byteProcessed);
                        byteProcessed++;
                        stop = (byte)BitConverter.ToChar(dataArray, byteProcessed);
                        byteProcessed++;

                        objectNumber = stop - start + 1;
                    }
                    else if(rangeMeaning == (short)Qualifier.OBJECT_COUNT)
                    {
                        objectNumber = BitConverter.ToInt16(dataArray, byteProcessed);
                        byteProcessed += 2;
                    }
                }
                else if (rangeOffset == 4)
                {
                    if (rangeMeaning == (short)Qualifier.START_STOP_INDEX)
                    {
                        start = BitConverter.ToInt16(dataArray, byteProcessed);
                        byteProcessed += 2;
                        stop = BitConverter.ToInt16(dataArray, byteProcessed);
                        byteProcessed += 2;

                        objectNumber = stop - start + 1;
                    }
                    else if (rangeMeaning == (short)Qualifier.OBJECT_COUNT)
                    {
                        objectNumber = BitConverter.ToInt32(dataArray, byteProcessed);
                        byteProcessed += 4;
                    }
                }
                else if (rangeOffset == 8)
                {
                    if (rangeMeaning == (short)Qualifier.START_STOP_INDEX)
                    {
                        start = BitConverter.ToInt32(dataArray, byteProcessed);
                        byteProcessed += 4;
                        stop = BitConverter.ToInt32(dataArray, byteProcessed);
                        byteProcessed += 4;

                        objectNumber = stop - start + 1;
                    }
                }

                switch ((TypeField)objectType)
                {
                    case TypeField.BINARY_COMMAND:
                        for (i = 0; i < objectNumber; i++)
                        {
                            if (prefixOffset == 1)
                            {
                                objectIndex = BitConverter.ToChar(dataArray, byteProcessed);
                                byteProcessed++;
                            }
                            else if (prefixOffset == 2)
                            {
                                objectIndex = BitConverter.ToInt16(dataArray, byteProcessed);
                                byteProcessed +=2;
                            }
                            else if(prefixOffset == 4)
                            {
                                objectIndex = BitConverter.ToInt32(dataArray, byteProcessed);
                                byteProcessed +=4;
                            }
                            regValue = dataArray[byteProcessed++];
                            if (regValue == 0x41)
                            {
                                pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_OUTPUT, (ushort)objectIndex), 1);
                            }
                            else
                            {
                                pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_OUTPUT, (ushort)objectIndex), 0);
                            }
                            byteProcessed += 10;
                        }
                        break;
                    case TypeField.BINARY_INPUT_PACKED_FORMAT:
                        for (adress = start; adress <= stop; adress++)
                        {
                            regValue = dataArray[byteProcessed + adress / 8];
                            value = (regValue & (1 << (adress % 8))) != 0 ? 1 : 0;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_INPUT, (ushort)adress), (ushort)value);
                        }
                        if (objectNumber % 8 == 0)
                        {
                            byteProcessed += objectNumber / 8;
                        }
                        else
                        {
                            byteProcessed += objectNumber / 8 + 1;
                        }
                        break;
                    case TypeField.BINARY_INPUT_WITH_STATUS:
                        for (adress = start; adress <= stop; adress++)
                        {
                            regValue = dataArray[byteProcessed++];
                            value = (regValue & 0x80) >> 7;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_INPUT, (ushort)adress), (ushort)value);
                        }
                        break;
                    case TypeField.BINARY_OUTPUT_PACKED_FORMAT:
                        for (adress = start; adress <= stop; adress++)
                        {
                            regValue = dataArray[byteProcessed + adress / 8];
                            value = (regValue & (1 << (adress % 8))) != 0 ? 1 : 0;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_OUTPUT, (ushort)adress), (ushort)value);
                        }
                        if (objectNumber % 8 == 0)
                        {
                            byteProcessed += objectNumber / 8;
                        }
                        else
                        {
                            byteProcessed += objectNumber / 8 + 1;
                        }
                        break;
                    case TypeField.BINATY_OUTPUT_WITH_STATUS:
                        for (adress = start; adress <= stop; adress++)
                        {
                            regValue = dataArray[byteProcessed++];
                            value = regValue & 0x80;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_INPUT, (ushort)adress), (ushort)value);
                        }
                        break;
                    case TypeField.ANALOG_INPUT_16BIT:
                        for (adress = start; adress <= stop; adress++)
                        {
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, (ushort)adress), (ushort)regValue);
                        }
                        break;
                    case TypeField.ANALOG_OUTPUT_16BIT:
                        for (i = 0; i < objectNumber; i++)
                        {
                            if (prefixOffset == 1)
                            {
                                objectIndex = BitConverter.ToChar(dataArray, byteProcessed);
                                byteProcessed++;
                            }
                            else if (prefixOffset == 2)
                            {
                                objectIndex = BitConverter.ToInt16(dataArray, byteProcessed);
                                byteProcessed += 2;
                            }
                            else if (prefixOffset == 4)
                            {
                                objectIndex = BitConverter.ToInt32(dataArray, byteProcessed);
                                byteProcessed += 4;
                            }
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;

                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT_16, (ushort)objectIndex), (ushort)regValue);
                            controlStatus = dataArray[byteProcessed++];
                        }
                        break;
                    case TypeField.COUNTER_16BIT:
                        for (adress = start; adress <= stop; adress++)
                        {
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            //pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, (ushort)i), (ushort)regValue);
                            //Ovde fakin nemamo countere, ali ugradicemo ih.
                        }
                        break;
                    case TypeField.FROZEN_COUNTER_16BIT:
                        for (adress = start; adress <= stop; adress++)
                        {
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            //pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, (ushort)i), (ushort)regValue);
                            //Ovde fakin nemamo countere, ali ugradicemo ih.
                        }
                        break;
                    case TypeField.ANALOG_OUTPUT_STATUS_16BIT:
                        for (adress = start; adress <= stop; adress++)
                        {
                            quality = (byte)BitConverter.ToChar(dataArray, byteProcessed);
                            byteProcessed++;
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, (ushort)adress), (ushort)regValue);
                        }
                        break;
                    case TypeField.BINARY_OUTPUT_WITHOUT_TIME:
                        for (i = 0; i < objectNumber; i++)
                        {
                            if (prefixOffset == 1)
                            {
                                objectIndex = BitConverter.ToChar(dataArray, byteProcessed);
                                byteProcessed++;
                            }
                            else if (prefixOffset == 2)
                            {
                                objectIndex = BitConverter.ToInt16(dataArray, byteProcessed);
                                byteProcessed += 2;
                            }
                            else if (prefixOffset == 4)
                            {
                                objectIndex = BitConverter.ToInt32(dataArray, byteProcessed);
                                byteProcessed += 4;
                            }
                            regValue = dataArray[byteProcessed++];
                            value = (regValue & 0x80) >> 7;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_OUTPUT, (ushort)objectIndex), (ushort)value);
                        }
                        break;
                    case TypeField.BINARY_INPUT_EVENT_WITHOUT_TIME:
                        for (i = 0; i < objectNumber; i++)
                        {
                            if (prefixOffset == 1)
                            {
                                objectIndex = BitConverter.ToChar(dataArray, byteProcessed);
                                byteProcessed++;
                            }
                            else if (prefixOffset == 2)
                            {
                                objectIndex = BitConverter.ToInt16(dataArray, byteProcessed);
                                byteProcessed += 2;
                            }
                            else if (prefixOffset == 4)
                            {
                                objectIndex = BitConverter.ToInt32(dataArray, byteProcessed);
                                byteProcessed += 4;
                            }
                            regValue = dataArray[byteProcessed++];
                            value = (regValue & 0x80) >> 7;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_INPUT, (ushort)objectIndex), (ushort)value);
                        }
                        break;
                    case TypeField.ANALOG_INPUT_EVENT_16BIT:
                        for (i = 0; i < objectNumber; i++)
                        {
                            if (prefixOffset == 1)
                            {
                                objectIndex = BitConverter.ToChar(dataArray, byteProcessed);
                                byteProcessed++;
                            }
                            else if (prefixOffset == 2)
                            {
                                objectIndex = BitConverter.ToInt16(dataArray, byteProcessed);
                                byteProcessed += 2;
                            }
                            else if (prefixOffset == 4)
                            {
                                objectIndex = BitConverter.ToInt32(dataArray, byteProcessed);
                                byteProcessed += 4;
                            }
                            quality = dataArray[byteProcessed++];
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, (ushort)objectIndex), (ushort)regValue);
                        }
                        break;
                    case TypeField.ANALOG_OUTPUT_EVENT_16BIT_WITHOUT_TIME:
                        for (i = 0; i < objectNumber; i++)
                        {
                            if (prefixOffset == 1)
                            {
                                objectIndex = BitConverter.ToChar(dataArray, byteProcessed);
                                byteProcessed++;
                            }
                            else if (prefixOffset == 2)
                            {
                                objectIndex = BitConverter.ToInt16(dataArray, byteProcessed);
                                byteProcessed += 2;
                            }
                            else if (prefixOffset == 4)
                            {
                                objectIndex = BitConverter.ToInt32(dataArray, byteProcessed);
                                byteProcessed += 4;
                            }
                            quality = dataArray[byteProcessed++];
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, (ushort)objectIndex), (ushort)regValue);
                        }
                        break;
                    default:

                        break;
                }
                HandleReceivedChangesOfPoints(pointsToupdate);
            }
        }

        private void PreproccessMsg(byte[] message, int messageLength, ref byte[] dataArray, int lengthData)
        {
            int dataChunkLen = 0;
            int srcOffset = 10, dstOffset = 0;

            dataChunkLen = lengthData / 16;

            for (int i = 0; i < dataChunkLen; i++)
            {
                Buffer.BlockCopy(message, srcOffset, dataArray, dstOffset, 16);
                dstOffset += 16;
                srcOffset += 18;
            }

            Buffer.BlockCopy(message, srcOffset, dataArray, dstOffset, lengthData % 16);
        }

        void ProcessQualifier(byte qualifier, out int prefixOffset, out short prefixMeaning, out int rangeOffset, out short rangeMeaning)
        {
            prefixOffset = 0;
            rangeOffset = 0;
            prefixMeaning = -1;
            rangeMeaning = -1;

            switch ((qualifier >> 4) & 0xf)
            {
                case 0x0:
                    break;
                case 0x1:
                    prefixOffset = 1;
                    prefixMeaning = (short)Qualifier.INDEX;
                    break;
                case 0x2:
                    prefixOffset = 2;
                    prefixMeaning = (short)Qualifier.INDEX;
                    break;
                case 0x3:
                    prefixOffset = 4;
                    prefixMeaning = (short)Qualifier.INDEX;
                    break;
                case 0x4:
                    prefixOffset = 1;
                    prefixMeaning = (short)Qualifier.OBJECT_SIZE;
                    break;
                case 0x5:
                    prefixOffset = 2;
                    prefixMeaning = (short)Qualifier.OBJECT_SIZE;
                    break;
                case 0x6:
                    prefixOffset = 4;
                    prefixMeaning = (short)Qualifier.OBJECT_SIZE;
                    break;
                case 0x7:
                    break;
            }

            switch (qualifier & 0xf)
            {
                case 0x0:
                    rangeOffset = 2;
                    rangeMeaning = (short)Qualifier.START_STOP_INDEX;
                    break;
                case 0x1:
                    rangeOffset = 4;
                    rangeMeaning = (short)Qualifier.START_STOP_INDEX;
                    break;
                case 0x2:
                    rangeOffset = 8;
                    rangeMeaning = (short)Qualifier.START_STOP_INDEX;
                    break;
                case 0x3:
                    rangeOffset = 2;
                    rangeMeaning = (short)Qualifier.VIRTUAL_ADDRESS;
                    break;
                case 0x4:
                    rangeOffset = 4;
                    rangeMeaning = (short)Qualifier.VIRTUAL_ADDRESS;
                    break;
                case 0x5:
                    rangeOffset = 8;
                    rangeMeaning = (short)Qualifier.VIRTUAL_ADDRESS;
                    break;
                case 0x6:
                    //polje opsega se ne koristi..
                    break;
                case 0x7:
                    rangeOffset = 1;
                    rangeMeaning = (short)Qualifier.OBJECT_COUNT;
                    break;
                case 0x8:
                    rangeOffset = 2;
                    rangeMeaning = (short)Qualifier.OBJECT_COUNT;
                    break;
                case 0x9:
                    rangeOffset = 4;
                    rangeMeaning = (short)Qualifier.OBJECT_COUNT;
                    break;
            }

            return;
        }

        private bool ChechIfUnsolicited(byte unsolicited)
        {
            int uns = (unsolicited & 0x10) >> 4;
            return uns == 1 ? true : false;
        }


        private void ConfirmUnsMessage(byte[] message)
        {
            DNP3ApplicationObjectParameters p;
            IDNP3Functions fn;

            //CONFIRM
            byte mask = 0x20;
            byte confirmRestartTime = (byte)((message[11] & mask) >> 5);
            if (confirmRestartTime == 1)
            {
                byte transportHeaderSeq = (byte)(message[10] & 0xf);
                byte applicationControl = (byte)(message[11] & 0xf);

                p = new DNP3ApplicationObjectParameters((byte)(0xd0 | applicationControl), (byte)DNP3FunctionCode.CONFIRM, 0, 0, 0x0001, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, (byte)((0xc0 | transportHeaderSeq) + 1));
                fn = DNP3FunctionFactory.CreateDNP3Message(p);
                SendDirectMessage(fn);
            }
        }

        public void SendMessage(IDNP3Functions message)
        {
            this.commandQueue.Enqueue(message);
        }

        public void SendDirectMessage(IDNP3Functions message)
        {
            this.connection.SendBytes(message.PackRequest());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            connectionProcessorThread.Abort();
        }

    }
}