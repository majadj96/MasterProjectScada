﻿using BackEndProcessorService;
using NetworkDynamicService.PointUpdater;
using NetworkDynamicService.ProxyPool;
using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    public class NetworkDynamicService : IDisposable
    {
        private List<ServiceHost> hosts = null;
        private PointUpdateProxy pointUpdateProxy;
        private BackEndPocessingModule backEndPocessingModule;

        public NetworkDynamicService()
        {
            pointUpdateProxy = new PointUpdateProxy("UpdatePointEndPoint");
            pointUpdateProxy.Open();
            backEndPocessingModule = new BackEndPocessingModule(pointUpdateProxy);
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
        }
        
        private void InitializeHosts()
        {
            hosts = new List<ServiceHost>();
            hosts.Add(new ServiceHost(backEndPocessingModule));
            hosts.Add(new ServiceHost(typeof(StateUpdateService)));
            hosts.Add(new ServiceHost(typeof(PointOperateService)));
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
        }
    }
}
