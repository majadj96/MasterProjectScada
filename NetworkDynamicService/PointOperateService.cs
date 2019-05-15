using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    public class PointOperateService : IDisposable, IFieldCommunicationService
    {
        private List<ServiceHost> hosts = null;
        private FrontEndProcessorServiceProxy fepsProxy = new FrontEndProcessorServiceProxy("FieldCommunicationServiceEndPoint");

        public PointOperateService()
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
            hosts.Add(new ServiceHost(typeof(PointOperateService)));
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

        public void Dispose()
        {
            CloseHosts();
        }

        public void ReadAnalogInput(int address)
        {
            this.fepsProxy.ReadAnalogInput(address);
        }

        public void ReadAnalogOutput(int address)
        {
            this.fepsProxy.ReadAnalogOutput(address);
        }

        public void ReadDigitalInput(int address)
        {
            this.ReadDigitalInput(address);
        }

        public void ReadDigitalOutput(int address)
        {
            this.ReadDigitalOutput(address);
        }

        public void WriteAnalogOutput(int address, int value)
        {
            this.WriteAnalogOutput(address, value);
        }

        public void WriteDigitalOutput(int address, int value)
        {
            this.WriteDigitalOutput(address, value);
        }
    }
}
