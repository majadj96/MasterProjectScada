using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    class FrontEndProcessorServiceProxy : ClientBase<IFieldCommunicationService>, IFieldCommunicationService
    {
        public FrontEndProcessorServiceProxy(string endpointName) : base(endpointName)
        {

        }

        public void ReadAnalogInput(int address)
        {
            Channel.ReadAnalogInput(address);
        }

        public void ReadAnalogOutput(int address)
        {
            Channel.ReadAnalogOutput(address);
        }

        public void ReadDigitalInput(int address)
        {
            Channel.ReadDigitalInput(address);
        }

        public void ReadDigitalOutput(int address)
        {
            Channel.ReadDigitalOutput(address);
        }

        public void WriteAnalogOutput(int address, int value)
        {
            Channel.WriteAnalogOutput(address, value);
        }

        public void WriteDigitalOutput(int address, int value)
        {
            Channel.WriteDigitalOutput(address,value);
        }
    }
}
