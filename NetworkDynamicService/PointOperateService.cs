using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    public class PointOperateService : IFieldCommunicationService
    {
        private FrontEndProcessorServiceProxy fepsProxy = new FrontEndProcessorServiceProxy("FieldCommunicationServiceEndPoint");

        public PointOperateService()
        {
            fepsProxy.Open();
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
            this.fepsProxy.ReadDigitalInput(address);
        }

        public void ReadDigitalOutput(int address)
        {
            this.fepsProxy.ReadDigitalOutput(address);
        }

        public void WriteAnalogOutput(int address, int value)
        {
            this.fepsProxy.WriteAnalogOutput(address, value);
        }

        public void WriteDigitalOutput(int address, int value)
        {
            this.fepsProxy.WriteDigitalOutput(address, value);
        }
    }
}
