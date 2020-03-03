using BackEndProcessorService;
using BackEndProcessorService.Proxy;
using NetworkDynamicService.Cache;
using NetworkDynamicService.PointUpdater;
using NetworkDynamicService.ProxyPool;
using NetworkDynamicService.Transaction;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using TransactionManagerContracts;

namespace NetworkDynamicService
{
    public class NetworkDynamicServiceHost : IDisposable
    {
        private List<ServiceHost> hosts = null;
        private PointUpdateProxy pointUpdateProxy;
        private NDSConfigurationProxy ndSConfigurationProxy;
        private AlarmEventServiceProxy alarmEventServiceProxy;
        private BackEndPocessingModule backEndPocessingModule;
        private ICommandingServiceContract commandingService;
        private INDSRealTimePointCache nDSRealTimePointCache;
        private ModelUpdateContract modelUpdateContract;
        private ITransactionSteps transactionService;
        private IStateUpdateService stateUpdateService;
        private StateUpdateServiceProxy stateUpdateProxy;
        private FepCommandingServiceProxy fepCmdProxy;
        private IProcessingServiceContract processingService;
        private PublisherProxy publisherProxy;

        public NetworkDynamicServiceHost()
        {
            pointUpdateProxy = new PointUpdateProxy("UpdatePointEndPoint");
            alarmEventServiceProxy = new AlarmEventServiceProxy("AlarmEventServiceEndPoint");
            ndSConfigurationProxy = new NDSConfigurationProxy("IFEPConfigService");
            stateUpdateProxy = new StateUpdateServiceProxy("StateUpdateServiceEndPoint");
            fepCmdProxy = new FepCommandingServiceProxy("FEPCommandingServiceContract");
            publisherProxy = new PublisherProxy("PublisherEndPoint");

            nDSRealTimePointCache = new NDSRealTimePointCache();
            backEndPocessingModule = new BackEndPocessingModule(pointUpdateProxy, this.alarmEventServiceProxy, this.publisherProxy);

            transactionService =  new TransactionService(nDSRealTimePointCache, OpenProxies);
            modelUpdateContract = new ModelUpdateContract(nDSRealTimePointCache, ndSConfigurationProxy, transactionService);
            stateUpdateService = new StateUpdateService(stateUpdateProxy);
            commandingService = new CommandingService(fepCmdProxy, backEndPocessingModule, nDSRealTimePointCache);
            processingService = new ProcessingService(backEndPocessingModule);
            InitializeHosts();
        }

        public void Start()
        {
            StartHosts();
        }

        private void OpenProxies()
        {
           // pointUpdateProxy.Open();
            //alarmEventServiceProxy.Open();
            //ndSConfigurationProxy.Open();
           // stateUpdateProxy.Open();
            fepCmdProxy.Open();
            publisherProxy.Open();
        }

        private void StartHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("Network Dynamic Services can not be opend because it is not initialized.");
            }

            foreach (ServiceHost host in hosts)
            {
                host.Open();
            }

            string message = "The Network Dynamic Service is started.";
            Console.WriteLine("\n{0}", message);
        }
        
        private void InitializeHosts()
        {
            hosts = new List<ServiceHost>();
            hosts.Add(new ServiceHost(stateUpdateService));
            hosts.Add(new ServiceHost(commandingService));
            hosts.Add(new ServiceHost(modelUpdateContract));
            hosts.Add(new ServiceHost(processingService));
        }

        public void Dispose()
        {
            CloseHosts();
            GC.SuppressFinalize(this);
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

            string message = "The Network Dynamic Service is closed.";
            Console.WriteLine("\n\n{0}", message);
        }
    }
}
