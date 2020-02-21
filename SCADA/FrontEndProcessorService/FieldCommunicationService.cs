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
        #endregion Fields

		#region Properties

		public DateTime CurrentTime
		{
			get { return currentTime; }
			set
			{
                if(currentTime != value)
                {
                    currentTime = value;
                    //ndsStateProxy.UpdateDateAndTime(currentTime);
                }
			}
		}

        public IConfiguration Configuration
        {
            get { return configuration; }
        }

        public IProcessingManager ProcessingManager
        {
            get { return processingManager; }
        }

		#endregion Properties

		public FieldCommunicationService()
		{
			Thread.CurrentThread.Name = "Field Communication Service";
            
            ndsStateProxy = new NetworkDynamicStateServiceProxy("NetworkDynamicStateServiceEndPoint");
            ndsProxy = new NetworkDynamicServiceProxy("NetworkDynamicServiceEndPoint");
        }

        public void StartService(Dictionary<Tuple<ushort, PointType>, BasePointCacheItem> points)
        {
            this.points = points;
            ndsProxy.Open();
            ndsStateProxy.Open();

            configuration = new ConfigReader();
            connection = new TCPConnection(Configuration, ndsStateProxy);
            commandExecutor = new FunctionExecutor(Configuration, connection);
            processingManager = new ProcessingManager(this, commandExecutor, ndsProxy);
            acquisitor = new Acquisitor(acquisitionTrigger, ProcessingManager, Configuration);
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