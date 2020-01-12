using ScadaCommon;
using FrontEndProcessorService.Configuration;
using ScadaCommon.Connection;
using ProcessingModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;
using System.ServiceModel;
using FrontEndProcessorService.PointDataModel;

namespace FrontEndProcessorService.ViewModel
{
    public class FieldCommunicationService : IDisposable, IStorage, IFieldCommunicationService
    {
		#region Fields
		private object lockObject = new object();
        private List<ServiceHost> hosts = null;
        private Thread timerWorker;
		private ConnectionState connectionState;
        private IConnection connection;
        private Acquisitor acquisitor;
		private AutoResetEvent acquisitionTrigger = new AutoResetEvent(false);
		private TimeSpan elapsedTime = new TimeSpan();
		private Dispatcher dispather = Dispatcher.CurrentDispatcher;
		private string logText;
		private DateTime currentTime;
		private IFunctionExecutor commandExecutor;
		private bool timerThreadStopSignal = true;
		private bool disposed = false;
		IConfiguration configuration;
        private IProcessingManager processingManager = null;
        private NetworkDynamicServiceProxy ndsProxy;
        private NetworkDynamicStateServiceProxy ndsStateProxy;
        #endregion Fields

        Dictionary<int, IPoint> pointsCache = new Dictionary<int, IPoint>();

		#region Properties

		public DateTime CurrentTime
		{
			get
			{
				return currentTime;
			}

			set
			{
				currentTime = value;
			}
		}

		public ConnectionState ConnectionState
		{
			get
			{
				return connectionState;
			}

			set
			{
				connectionState = value;
			}
		}

		public string LogText
		{
			get
			{
				return logText;
			}

			set
			{
				logText = value;
			}
		}

		public TimeSpan ElapsedTime
		{
			get
			{
				return elapsedTime;
			}

			set
			{
				elapsedTime = value;
			}
		}

		#endregion Properties

		public FieldCommunicationService()
		{
			Thread.CurrentThread.Name = "Main Thread";
            ndsProxy = new NetworkDynamicServiceProxy("NetworkDynamicServiceEndPoint");
            ndsProxy.Open();
            ndsProxy.Process(null);

            ndsStateProxy = new NetworkDynamicStateServiceProxy("NetworkDynamicStateServiceEndPoint");
            ndsStateProxy.Open();
            ndsStateProxy.ProcessState(null);

            InitializeHosts();

			configuration = new ConfigReader();
            this.connection = new TCPConnection(configuration);
            commandExecutor = new FunctionExecutor(configuration, connection);
            this.processingManager = new ProcessingManager(this, commandExecutor);
            this.acquisitor = new Acquisitor(acquisitionTrigger, this.processingManager, configuration);
			InitializePointCollection();
			InitializeAndStartThreads();
			ConnectionState = connection.ConnectionState;
		}

		#region Private methods

		private void InitializePointCollection()
		{
			foreach (var c in configuration.GetConfigurationItems())
			{
				for (int i = 0; i < c.NumberOfRegisters; i++)
				{
					BasePointItem pi = CreatePoint(c, i, this.processingManager);
					if (pi != null)
					{
						pointsCache.Add(pi.PointId, pi as IPoint);
                        processingManager.InitializePoint(pi.Type, pi.Address, pi.RawValue);
					}
				}
			}
		}

		private BasePointItem CreatePoint(IConfigItem c, int i, IProcessingManager processingManager)
		{
			switch (c.RegistryType)
			{
				case PointType.DIGITAL_INPUT:
					return new DigitalInput(c, i);

				case PointType.DIGITAL_OUTPUT:
					return new DigitalOutput(c, i);

				case PointType.ANALOG_INPUT:
					return new AnalaogInput(c, i);

				case PointType.ANALOG_OUTPUT:
					return new AnalogOutput(c, i);

				default:
					return null;
			}
		}

        public void Start()
        {
            StartHosts();
        }
        private void StartHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("Field Communication Services can not be opend because it is not initialized.");
            }

            foreach (ServiceHost host in hosts)
            {
                host.Open();
            }
        }


        private void InitializeHosts()
        {
            hosts = new List<ServiceHost>();
            hosts.Add(new ServiceHost(typeof(FieldCommunicationService)));
        }

        public void CloseHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("Network Dynamic Services can not be closed because it is not initialized.");
            }

            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }
        }

        private void InitializeAndStartThreads()
		{
			InitializeTimerThread();
			StartTimerThread();
		}

		private void InitializeTimerThread()
		{
			timerWorker = new Thread(TimerWorker_DoWork);
			timerWorker.Name = "Timer Thread";
		}

		private void StartTimerThread()
		{
			timerWorker.Start();
		}

		/// <summary>
		/// Timer thread:
		///		Refreshes timers on UI and signalizes to acquisition thread that one second has elapsed
		/// </summary>
		private void TimerWorker_DoWork()
		{
			while (timerThreadStopSignal)
			{
				if (disposed)
					return;

				CurrentTime = DateTime.Now;
				ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));
				acquisitionTrigger.Set();
				Thread.Sleep(1000);
			}
		}

		#endregion Private methods

		public void Dispose()
		{
			disposed = true;
			timerThreadStopSignal = false;
			(commandExecutor as IDisposable).Dispose();
			this.acquisitor.Dispose();
			acquisitionTrigger.Dispose();
            CloseHosts();
            GC.SuppressFinalize(this);
        }

		public List<IPoint> GetPoints(List<PointIdentifier> pointIds)
		{
			List<IPoint> retVal = new List<IPoint>(pointIds.Count);
			foreach (var pid in pointIds)
			{
				int id = PointIdentifierHelper.GetNewPointId(pid);
				IPoint p = null;
				if (pointsCache.TryGetValue(id, out p))
				{
					retVal.Add(p);
				}
			}
			return retVal;
		}

        public void WriteDigitalOutput(int adress, int value)
        {
            throw new NotImplementedException();
        }

        public void WriteAnalogOutput(int adress, int value)
        {
            throw new NotImplementedException();
        }

        public void ReadDigitalInput(int adress)
        {
            throw new NotImplementedException();
        }

        public void ReadAnalogInput(int adress)
        {
            throw new NotImplementedException();
        }

        public void ReadDigitalOutput(int adress)
        {
            throw new NotImplementedException();
        }

        public void ReadAnalogOutput(int adress)
        {
            throw new NotImplementedException();
        }
    }
}