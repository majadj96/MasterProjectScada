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
        private AutoResetEvent funcExecuteUnsolicitedSync;
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
		public FunctionExecutor(IStateUpdater stateUpdater, IConfiguration configuration, AutoResetEvent funcExecuteUnsolicitedSync, IConnection connection)
		{
			this.stateUpdater = stateUpdater;
			this.configuration = configuration;
            this.funcExecuteUnsolicitedSync = funcExecuteUnsolicitedSync;
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
                        stateUpdater.LogMessage(string.Format(RECEIVED_MESSAGE, point.Key.Item1, point.Key.Item2, point.Value));
                    }
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
					processConnection.WaitOne();
                    funcExecuteUnsolicitedSync.WaitOne();      //Sinhronizacioni mehanizam za unsolicited poruke
                                                               //Koja metoda prva udje zakljucava semafor i na kraju ga pusta
					while (commandQueue.TryDequeue(out currentCommand))
					{
						this.connection.SendBytes(this.currentCommand.PackRequest());
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
						this.HandleReceivedBytes(message);
						this.currentCommand = null;
					}

                    funcExecuteUnsolicitedSync.Set();
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

        private void ProccessMsg(IDNP3Functions message)
        {
            //ovde obradjujem sve poruke
        }
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