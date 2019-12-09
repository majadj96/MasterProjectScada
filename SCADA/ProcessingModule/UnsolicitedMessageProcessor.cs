using ScadaCommon;
using ScadaCommon.Connection;
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

        public UnsolicitedMessageProcessor(IStateUpdater stateUpdater, IConfiguration configuration, AutoResetEvent funcExecuteUnsolicitedSync, IConnection connection)
        {
            this.connection = connection;
            this.stateUpdater = stateUpdater;
            this.configuration = configuration;
            this.funcExecuteUnsolicitedSync = funcExecuteUnsolicitedSync;
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
                funcExecuteUnsolicitedSync.Set();
                Thread.Sleep(300);
            }
        }
        public void Dispose()
        {
            this.unsolicitedProcessor.Abort();
        }
    }
}
