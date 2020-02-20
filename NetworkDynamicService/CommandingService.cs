using ScadaCommon;
using ScadaCommon.ComandingModel;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    public class CommandingService : ICommandingServiceContract
    {
        private FrontEndProcessorServiceProxy fepsProxy = new FrontEndProcessorServiceProxy("FieldCommunicationServiceEndPoint");

        public CommandingService()
        {
            fepsProxy.Open();
        }

        public CommandResult ReadAnalogInput(CommandObject command)
        {
            throw new NotImplementedException();
        }

        public CommandResult ReadAnalogOutput(CommandObject command)
        {
            throw new NotImplementedException();
        }

        public CommandResult ReadDigitalInput(CommandObject command)
        {
            throw new NotImplementedException();
        }

        public CommandResult ReadDigitalOutput(CommandObject command)
        {
            throw new NotImplementedException();
        }

        public CommandResult WriteAnalogOutput(CommandObject command)
        {
            throw new NotImplementedException();
        }

        public CommandResult WriteDigitalOutput(CommandObject command)
        {
            throw new NotImplementedException();
        }
    }
}
