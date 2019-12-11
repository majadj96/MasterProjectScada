using ScadaCommon;
using ScadaCommon.Connection;
using ScadaCommon.Interfaces;
using System;
using System.Threading;

namespace ProcessingModule
{
    public class UnsolicitedMessageProcessor : IDisposable
    {
        private Thread unsolicitedProcessor;
        private IStateUpdater stateUpdater;
        private IConfiguration configuration;
        private AutoResetEvent funcExecuteUnsolicitedSync;
        private bool threadCancellationSignal = true;
        private IConnection connection;
        private IProcessingManager processingManager;

        public UnsolicitedMessageProcessor(IStateUpdater stateUpdater, IConfiguration configuration, AutoResetEvent funcExecuteUnsolicitedSync, IConnection connection, IProcessingManager processingManager)
        {
            this.connection = connection;
            this.stateUpdater = stateUpdater;
            this.configuration = configuration;
            this.funcExecuteUnsolicitedSync = funcExecuteUnsolicitedSync;
            this.processingManager = processingManager;
            InitializeUnsolicitedThread();
            StartUnsolicitedThread();
        }


        private void InitializeUnsolicitedThread()
        {
            this.unsolicitedProcessor = new Thread(UnsolicitedProccessor_DoWork);
            this.unsolicitedProcessor.Name = "unsolicitedProcessor";
        }

        private void StartUnsolicitedThread()
        {
            this.unsolicitedProcessor.Start();
        }

        private void UnsolicitedProccessor_DoWork()
        {
            while (this.threadCancellationSignal)
            {
                funcExecuteUnsolicitedSync.WaitOne();

                if (connection.ReadReady() == false)
                {
                    funcExecuteUnsolicitedSync.Set();
                    continue;
                }

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

                stateUpdater.LogMessage(message.ToString());

                CheckUnsMessage(message);

                funcExecuteUnsolicitedSync.Set();
                Thread.Sleep(300);
            }
        }

        public void Dispose()
        {
            this.unsolicitedProcessor.Abort();
        }

        private void CheckUnsMessage(byte[] messageByte)
        {
            //CONFIRM
            byte mask = 0x20;
            byte confirmRestartTime = (byte)((messageByte[11] & mask) >> 5);
            if(confirmRestartTime == 1)
            {
                this.processingManager.SendRawBytesMessage(DNP3FunctionCode.CONFIRM, messageByte);
            }
            //RESTART
            mask = 0x80;
            confirmRestartTime = (byte)((messageByte[13] & mask) >> 7);
            if (confirmRestartTime == 1)
            {
                this.processingManager.SendRawBytesMessage(DNP3FunctionCode.WARM_RESTART, messageByte);
            }
            //TIME SYNC
            mask = 0x10;
            confirmRestartTime = (byte)((messageByte[13] & mask) >> 4);
            if (confirmRestartTime == 1)
            {
                this.processingManager.SendRawBytesMessage(DNP3FunctionCode.WRITE, messageByte);
            }
        }
    }
}
