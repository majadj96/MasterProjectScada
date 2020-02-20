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
        private IFEPCommandingServiceContract fepCmdProxy;
        private IBackendProcessor backendProcessor;
        public CommandingService(IFEPCommandingServiceContract fepCmdProxy, IBackendProcessor backendProcessor)
        {
            this.fepCmdProxy = fepCmdProxy;
            this.backendProcessor = backendProcessor;
        }

        public CommandResult ReadAnalogInput(CommandObject command)
        {
            return CommandResult.Success;
        }

        public CommandResult ReadAnalogOutput(CommandObject command)
        {
            return CommandResult.Success;
        }

        public CommandResult ReadDigitalInput(CommandObject command)
        {
            return CommandResult.Success;
        }

        public CommandResult ReadDigitalOutput(CommandObject command)
        {
            return CommandResult.Success;
        }

        public CommandResult WriteAnalogOutput(CommandObject command)
        {
            return CommandResult.Success;
        }

        public CommandResult WriteDigitalOutput(CommandObject command)
        {
            return CommandResult.Success;
        }
    }
}
