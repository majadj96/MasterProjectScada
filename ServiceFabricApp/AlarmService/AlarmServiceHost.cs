﻿using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AlarmService
{
    public class AlarmServiceHost : IDisposable
    {
        private ServiceHost host = null;
        private AlarmServices alarmServices = new AlarmServices();

        public AlarmServiceHost()
        {
            InitializeHosts();
        }

        private void InitializeHosts()
        {
            host = new ServiceHost(alarmServices);
        }

        public void Start()
        {
            StartHosts();
        }

        private void StartHosts()
        {
            if (host == null)
            {
                throw new Exception("AlarmEventServices can not be opend because it is not initialized.");
            }

            host.Open();

            string message = "The AlarmEventServices is started.";
            Console.WriteLine("\n{0}", message);
        }

        public void CloseHosts()
        {
            if (host == null)
            {
                throw new Exception("AlarmEventServices can not be closed because it is not initialized.");
            }

            host.Close();

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
