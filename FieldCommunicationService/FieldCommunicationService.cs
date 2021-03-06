﻿using FrontEndProcessorService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FieldCommunicationService
{
    public class FieldCommunicationService : IDisposable
    {
        private List<ServiceHost> hosts = null;

        public FieldCommunicationService()
        {
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
            hosts.Add(new ServiceHost(typeof(FrontEndProcessorService.ViewModel.FieldCommunicationService)));
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
