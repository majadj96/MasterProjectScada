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
using ScadaCommon.ServiceProxies;
using ScadaCommon.NDSDataModel;
using ScadaCommon.BackEnd_FrontEnd;
using EntityFrameworkMeasurementInfrastructure;


namespace FrontEndProcessorService
{
    public class FieldCommunicationService : IDisposable, IStorage
    {
        #region Fields
        private Thread timerWorker;
        private IConnection connection;
        private Acquisitor acquisitor;
		private AutoResetEvent acquisitionTrigger = new AutoResetEvent(false);
		private Dispatcher dispather = Dispatcher.CurrentDispatcher;
		private DateTime currentTime;
		private IFunctionExecutor commandExecutor;
		private bool timerThreadStopSignal = true;
		private bool disposed = false;
		private IConfiguration configuration;
        private IProcessingManager processingManager = null;
        private NetworkDynamicServiceProxy ndsProxy;
        private NetworkDynamicStateServiceProxy ndsStateProxy;
        private Dictionary<Tuple<ushort, PointType>, BasePointCacheItem> points;
        private List<ServiceHost> hosts = null;
        private FEPCommandingService fEPCommandingService;
        private IFEPConfigService nDSConfigurationService;
        #endregion Fields

        #region Properties

        public DateTime CurrentTime
        {
            get { return currentTime; }
            set
            {
                if (currentTime != value)
                {
                    currentTime = value;
                }
            }
        }

		#endregion Properties

		public FieldCommunicationService()
		{
			Thread.CurrentThread.Name = "Field Communication Service";
            
            ndsStateProxy = new NetworkDynamicStateServiceProxy("NetworkDynamicStateServiceEndPoint");
            ndsProxy = new NetworkDynamicServiceProxy("NetworkDynamicServiceEndPoint");

            configuration = new ConfigReader();
            connection = new TCPConnection(configuration, ndsStateProxy);
            commandExecutor = new FunctionExecutor(configuration, connection);
            processingManager = new ProcessingManager(this, commandExecutor, ndsProxy);

            fEPCommandingService = new FEPCommandingService(processingManager, configuration);
            nDSConfigurationService = new NDSConfigurationService(StartService);

            InitializeHosts();
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
            hosts.Add(new ServiceHost(nDSConfigurationService));
            hosts.Add(new ServiceHost(fEPCommandingService));
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

        public void StartService(Dictionary<Tuple<ushort, PointType>, BasePointCacheItem> points)
        {
            this.points = points;
            ndsProxy.Open();
            ndsStateProxy.Open();

            commandExecutor.StartExecution();


            acquisitor = new Acquisitor(acquisitionTrigger, processingManager, configuration);
            InitializeAndStartThreads();
        }

        #region Private methods

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
            GC.SuppressFinalize(this);
            CloseHosts();
        }

		public List<BasePointCacheItem> GetPoints(List<PointIdentifier> pointIds)
		{
			List<BasePointCacheItem> retVal = new List<BasePointCacheItem>(pointIds.Count);
			foreach (var pid in pointIds)
			{
				BasePointCacheItem p = null;
				if (points.TryGetValue(Tuple.Create(pointIds[0].Address, pointIds[0].PointType), out p))
				{
					retVal.Add(p);
				}
			}
			return retVal;
		}
    }
}