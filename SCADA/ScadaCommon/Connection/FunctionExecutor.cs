using ScadaCommon.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ScadaCommon.Connection
{
    /// <summary>
    /// Class containing logic for sending modbus requests and receiving point values. 
    /// </summary>
    public class FunctionExecutor : IDisposable, IFunctionExecutor
	{
		private IConnection connection;
		private IStateUpdater stateUpdater;
        private IDNP3Functions currentCommand;
        private bool threadCancellationSignal = true;
		private AutoResetEvent processConnection;
		private Thread connectionProcessorThread;
        private ConcurrentQueue<IDNP3Functions> commandQueue = new ConcurrentQueue<IDNP3Functions>();
        private IConfiguration configuration;
        private string RECEIVED_MESSAGE = "Point of type {0} on address {1:d5} received value: {2}";

        /// <inheritdoc />
        public event UpdatePointDelegate UpdatePointEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionExecutor"/> class.
        /// </summary>
        /// <param name="stateUpdater">The state updater.</param>
        /// <param name="configuration">The configuration.</param>
		public FunctionExecutor(IStateUpdater stateUpdater, IConfiguration configuration, IConnection connection)
		{
			this.stateUpdater = stateUpdater;
			this.configuration = configuration;
            this.connection = connection;
			this.processConnection = new AutoResetEvent(false);
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

        /// <summary>
        /// Invokes the update point event after the response is parsed.
        /// </summary>
        /// <param name="receivedBytes">The received response.</param>
		public void HandleReceivedBytes(byte[] receivedBytes)
		{
            if (receivedBytes.Length != 17)
            {
                Dictionary<Tuple<PointType, ushort>, ushort> pointsToupdate = this.currentCommand?.ParseResponse(receivedBytes);
                if (UpdatePointEvent != null)
                {
                    foreach (var point in pointsToupdate)
                    {
                        UpdatePointEvent.Invoke(point.Key.Item1, point.Key.Item2, point.Value);
                    }
                }
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

        /// <summary>
        /// Logic for handling the connection.
        /// </summary>
		private void ConnectionProcessorThread()
		{
			while (this.threadCancellationSignal)
			{
				try
				{
                    if (connection.ConnectionState == ConnectionState.CONNECTED) {
                        if (processConnection.WaitOne())
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
                                    this.currentCommand = null;
                                }
                            }
                        }
                        else
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
					this.stateUpdater.UpdateConnectionState(ConnectionState.DISCONNECTED);
					string message = $"{se.TargetSite.ReflectedType.Name}.{se.TargetSite.Name}: {se.Message}";
					stateUpdater.LogMessage(message);
				}
				catch (Exception ex)
				{
					string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
					stateUpdater.LogMessage(message);
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
            byte qualifier, quality;

            PreproccessMsg(message, messageLength, ref dataArray, lengthData);

            byte transportControl = dataArray[byteProcessed++];
            byte appControl = dataArray[byteProcessed++];
            byte functionCode = dataArray[byteProcessed++];
            short interalIndications = BitConverter.ToInt16(dataArray, byteProcessed);
            byteProcessed += 2;

            short objectType, prefixMeaning, rangeMeaning;
            int prefixOffset = 0, rangeOffset = 0;
            int start = 0, stop = 0, octetNumber, objectNumber, i, regValue, value, objectPrefix;

            while (byteProcessed < lengthData)
            {
                objectType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(dataArray, byteProcessed));
                byteProcessed += 2;

                qualifier = dataArray[byteProcessed++];
                ProcessQualifier(qualifier, out prefixOffset, out prefixMeaning, out rangeOffset, out rangeMeaning);

                octetNumber = rangeOffset / 2;
                if (octetNumber == 1)
                {
                    start = (byte)BitConverter.ToChar(dataArray, byteProcessed);
                    byteProcessed++;
                    stop = (byte)BitConverter.ToChar(dataArray, byteProcessed);
                    byteProcessed++;
                }
                else if (octetNumber == 2)
                {
                    start = BitConverter.ToInt16(dataArray, byteProcessed);
                    byteProcessed += 2;
                    stop = BitConverter.ToInt16(dataArray, byteProcessed);
                    byteProcessed += 2;
                }
                else if (octetNumber == 4)
                {
                    start = BitConverter.ToInt32(dataArray, byteProcessed);
                    byteProcessed += 4;
                    stop = BitConverter.ToInt32(dataArray, byteProcessed);
                    byteProcessed += 4;
                }

                objectNumber = stop - start + 1;

                switch ((TypeField)objectType)
                {
                    case TypeField.BINARY_INPUT_PACKED_FORMAT:
                        for (i = start; i <= stop; i++)
                        {
                            regValue = dataArray[byteProcessed + i / 8];
                            value = (regValue & (1 << (i % 8))) != 0 ? 1 : 0;
                            pointsToupdate.Add(new Tuple<PointType, ushort> (PointType.BINARY_INPUT, (ushort)i), (ushort)value);
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
                    case TypeField.BINARY_OUTPUT_PACKED_FORMAT:
                        for (i = start; i <= stop; i++)
                        {
                            regValue = dataArray[byteProcessed + i / 8];
                            value = (regValue & (1 << (i % 8))) != 0 ? 1 : 0;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.BINARY_OUTPUT, (ushort)i), (ushort)value);
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
                    case TypeField.ANALOG_INPUT_16BIT:
                        for (i = start; i <= stop; i++)
                        {
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, (ushort)i), (ushort)regValue);
                        }
                        break;
                    case TypeField.COUNTER_16BIT:
                        for (i = start; i <= stop; i++)
                        {
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            //pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, (ushort)i), (ushort)regValue);
                            //Ovde fakin nemamo countere, ali ugradicemo ih.
                        }
                        break;
                    case TypeField.FROZEN_COUNTER_16BIT:
                        for (i = start; i <= stop; i++)
                        {
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            //pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT, (ushort)i), (ushort)regValue);
                            //Ovde fakin nemamo countere, ali ugradicemo ih.
                        }
                        break;
                    case TypeField.ANALOG_OUTPUT_STATUS_16BIT:
                        for (i = start; i <= stop; i++)
                        {
                            quality = (byte)BitConverter.ToChar(dataArray, byteProcessed);
                            byteProcessed++;
                            regValue = BitConverter.ToInt16(dataArray, byteProcessed);
                            byteProcessed += 2;
                            pointsToupdate.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, (ushort)i), (ushort)regValue);
                        }
                        break;
                    case TypeField.BINARY_OUTPUT_EVENT:
                        break;
                    case TypeField.ANALOG_INPUT_EVENT_16BIT:
                        break;
                    case TypeField.BINARY_INPUT_EVENT_WITHOUT_TIME:
                        break;
                    case TypeField.FLOATING_POINT_OUTPUT_EVENT_32BIT:
                        break;
                    case TypeField.COUNTER_CHANGE_EVENT_16BIT:
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
                    rangeMeaning = (short)Qualifier.INDEX;
                    break;
                case 0x1:
                    rangeOffset = 4;
                    rangeMeaning = (short)Qualifier.INDEX;
                    break;
                case 0x2:
                    rangeOffset = 8;
                    rangeMeaning = (short)Qualifier.INDEX;
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


        //private void CheckUnsMessage(byte[] messageByte)
        //{
        //    //CONFIRM
        //    byte mask = 0x20;
        //    byte confirmRestartTime = (byte)((messageByte[11] & mask) >> 5);
        //    if (confirmRestartTime == 1)
        //    {
        //        this.processingManager.SendRawBytesMessage(DNP3FunctionCode.CONFIRM, messageByte);
        //    }
        //    //RESTART
        //    mask = 0x80;
        //    confirmRestartTime = (byte)((messageByte[13] & mask) >> 7);
        //    if (confirmRestartTime == 1)
        //    {
        //        this.processingManager.SendRawBytesMessage(DNP3FunctionCode.WARM_RESTART, messageByte);
        //    }
        //    //TIME SYNC
        //    mask = 0x10;
        //    confirmRestartTime = (byte)((messageByte[13] & mask) >> 4);
        //    if (confirmRestartTime == 1)
        //    {
        //        this.processingManager.SendRawBytesMessage(DNP3FunctionCode.WRITE, messageByte);
        //    }
        //}

        public void SendMessage(IDNP3Functions message)
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