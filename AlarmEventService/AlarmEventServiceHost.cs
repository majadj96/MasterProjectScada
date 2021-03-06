﻿using PubSubCommon;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AlarmEventService
{
    public class AlarmEventServiceHost : IDisposable
    {
        private List<ServiceHost> hosts = null;
        private AlarmEventServices aes = new AlarmEventServices();

        public AlarmEventServiceHost()
        {
            InitializeHosts();
        }

        private void InitializeHosts()
        {
            hosts = new List<ServiceHost>();
            hosts.Add(new ServiceHost(aes));
        }



        public void Start()
        {
            StartHosts();
        }

        private void StartHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("AlarmEventServices can not be opend because it is not initialized.");
            }

            foreach (ServiceHost host in hosts)
            {
                host.Open();
            }

            string message = "The AlarmEventServices is started.";
            Console.WriteLine("\n{0}", message);
        }

        public void CloseHosts()
        {
            if (hosts == null || hosts.Count == 0)
            {
                throw new Exception("AlarmEventServices can not be closed because it is not initialized.");
            }

            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }

            string message = "The AlarmEventServices is closed.";
            Console.WriteLine("\n\n{0}", message);
        }

        public void Dispose()
        {
            CloseHosts();
            GC.SuppressFinalize(this);
        }
    }
}
