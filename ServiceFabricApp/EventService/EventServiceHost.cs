using System;
using System.ServiceModel;

namespace EventService
{
    class EventServiceHost : IDisposable
    {
        private ServiceHost host = null;
        private EventServices eventServices = new EventServices();

        public EventServiceHost()
        {
            InitializeHosts();
        }

        private void InitializeHosts()
        {
            host = new ServiceHost(eventServices);
        }

        public void Start()
        {
            StartHosts();
        }

        private void StartHosts()
        {
            if (host == null)
            {
                throw new Exception("EventService can not be opend because it is not initialized.");
            }

            host.Open();

            string message = "The EventService is started.";
            Console.WriteLine("\n{0}", message);
        }

        public void CloseHosts()
        {
            if (host == null)
            {
                throw new Exception("EventService can not be closed because it is not initialized.");
            }

            host.Close();

            string message = "The EventService is closed.";
            Console.WriteLine("\n\n{0}", message);
        }

        public void Dispose()
        {
            CloseHosts();
            GC.SuppressFinalize(this);
        }
    }
}
