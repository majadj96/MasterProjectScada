using BackEndProcessorService;
using BackEndProcessorService.Proxy;
using NetworkDynamicService.Cache;
using NetworkDynamicService.PointUpdater;
using NetworkDynamicService.ProxyPool;
using NetworkDynamicService.Transaction;
using ScadaCommon.BackEnd_FrontEnd;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace NetworkDynamicService
{
    public class NetworkDynamicServiceHost : IDisposable
    {
        private List<ServiceHost> hosts = null;
        private PointUpdateProxy pointUpdateProxy;
        private NDSConfigurationProxy ndSConfigurationProxy;
        private AlarmEventServiceProxy alarmEventServiceProxy;
        private BackEndPocessingModule backEndPocessingModule;
        private INDSRealTimePointCache nDSRealTimePointCache;
        private ModelUpdateContract modelUpdateContract;

        public NetworkDynamicServiceHost()
        {
            //pointUpdateProxy = new PointUpdateProxy("UpdatePointEndPoint");
            //pointUpdateProxy.Open();

            alarmEventServiceProxy = new AlarmEventServiceProxy("AlarmEventServiceEndPoint");
            alarmEventServiceProxy.Open();

            ndSConfigurationProxy = new NDSConfigurationProxy("INDSBasePointCacheItemsEndPoint");
            ndSConfigurationProxy.Open();

            nDSRealTimePointCache = new NDSRealTimePointCache();

            backEndPocessingModule = new BackEndPocessingModule(pointUpdateProxy, this.alarmEventServiceProxy);
            modelUpdateContract = new ModelUpdateContract(nDSRealTimePointCache, ndSConfigurationProxy);
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
            hosts.Add(new ServiceHost(backEndPocessingModule));
            hosts.Add(new ServiceHost(typeof(StateUpdateService)));
            hosts.Add(new ServiceHost(typeof(PointOperateService)));
            hosts.Add(new ServiceHost(modelUpdateContract)); //transaction
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
