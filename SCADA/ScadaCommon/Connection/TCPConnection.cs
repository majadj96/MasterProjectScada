using ScadaCommon.Interfaces;
using ScadaCommon.ServiceProxies;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ScadaCommon.Connection
{
    /// <summary>
    /// Class containing logic for establisshing tcp connections.
    /// </summary>
    public class TCPConnection : IConnection
	{
		private IPEndPoint remoteEP;
		private Socket socket;
		private IConfiguration configuration;
        private ConnectionState connection = ConnectionState.DISCONNECTED;
        private NetworkDynamicStateServiceProxy ndsStateProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="TCPConnection"/> class.
        /// </summary>
        /// <param name="stateUpdater">The state updater.</param>
        /// <param name="configuration">The configuration.</param>
        public TCPConnection(IConfiguration configuration, NetworkDynamicStateServiceProxy ndsStateProxy)
		{
			this.configuration = configuration;
			this.remoteEP = CreateRemoteEndpoint();
            PrepairConnection();
            this.ndsStateProxy = ndsStateProxy;
		}

        /// <inheritdoc />
        public void Connect()
		{
			this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.socket.Blocking = false;
			try
			{
				socket.Connect(remoteEP);
			}
			catch (SocketException se)
			{
				if (se.ErrorCode != 10035)
				{
					throw se;
				}
			}
		}

        public void PrepairConnection()
        {
            try
            {
                int numberOfConnectionRetries = 0;
                Connect();
                while (numberOfConnectionRetries < 10)
                {
                    if (CheckState())
                    {
                        ConnectionState = ConnectionState.CONNECTED;
                        numberOfConnectionRetries = 0;
                        break;
                    }
                    else
                    {
                        numberOfConnectionRetries++;
                        if (numberOfConnectionRetries == 10)
                        {
                            Disconnect();
                            ConnectionState = ConnectionState.DISCONNECTED;
                        }
                    }
                }
            }
            catch (SocketException se)
            {
                if (se.ErrorCode != 10054)
                {
                    throw se;
                }
                ConnectionState = ConnectionState.DISCONNECTED;
                string message = $"{se.TargetSite.ReflectedType.Name}.{se.TargetSite.Name}: {se.Message}";
            }
            catch (Exception ex)
            {
                string message = $"{ex.TargetSite.ReflectedType.Name}.{ex.TargetSite.Name}: {ex.Message}";
            }
        }

        /// <inheritdoc />
        public void Disconnect()
		{
			if (socket.Connected)
			{
				socket.Shutdown(SocketShutdown.Both);
			}

			socket.Close();
			socket = null;
		}

        /// <inheritdoc />
        public byte[] RecvBytes(int numberOfBytes)
		{
			int numberOfReceivedBytes = 0;
            int numberOfReceiveRetries = 0;
            byte[] retval = new byte[numberOfBytes];
			int numOfReceived;
			while (numberOfReceivedBytes < numberOfBytes)
			{
                numberOfReceiveRetries++;
                if (socket.Connected) {
                    numOfReceived = 0;
                    if (socket.Poll(1623, SelectMode.SelectRead))
                    {
                        numOfReceived = socket.Receive(retval, numberOfReceivedBytes, (int)numberOfBytes - numberOfReceivedBytes, SocketFlags.None);
                        if (numOfReceived > 0)
                        {
                            numberOfReceivedBytes += numOfReceived;
                        }
                    }
                    //else
                    //{
                    //    break;
                    //}
                }
                else
                {
                    ConnectionState = ConnectionState.DISCONNECTED;
                    throw new SocketException(10054);
                }
			}
			return retval;
		}

        /// <inheritdoc />
        public void SendBytes(byte[] bytesToSend)
		{
			int currentlySent = 0;

			while (currentlySent < bytesToSend.Count())
			{
				if (socket.Poll(1623, SelectMode.SelectWrite))
				{
					currentlySent += socket.Send(bytesToSend, currentlySent, bytesToSend.Length - currentlySent, SocketFlags.None);
				}
			}
		}

        /// <summary>
        /// Creates a remote endpoint.
        /// </summary>
        /// <returns>The created endpoint</returns>
		private IPEndPoint CreateRemoteEndpoint()
		{
			IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = null;
			foreach (IPAddress ip in ipHostInfo.AddressList)
				if ("127.0.0.1".Equals(ip.ToString()))
					ipAddress = ip;
			return new IPEndPoint(ipAddress, configuration.TcpPort);
		}

        /// <inheritdoc />
		public bool CheckState()
		{
			return this.socket.Poll(30000, SelectMode.SelectWrite) && this.socket.Connected;
		}

        public bool ReadReady()
        {
            return this.socket.Poll(1623, SelectMode.SelectRead);
        }

        public ConnectionState ConnectionState
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;
                ndsStateProxy.UpdateState(connection);
            }
        }
    }
}