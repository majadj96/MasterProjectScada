using ScadaCommon;
using ScadaCommon.ComandingModel;
using ScadaCommon.ServiceContract;
using System.ServiceModel;

namespace UserInterface.ProxyPool
{
    public class CommandingServiceProxy : ClientBase<ICommandingServiceContract>, ICommandingServiceContract
    {
        public CommandingServiceProxy(string endpointName) : base(endpointName)
        {

        }

        public CommandResult ReadAnalogInput(CommandObject command)
        {
            return Channel.ReadAnalogInput(command);
        }

        public CommandResult ReadAnalogOutput(CommandObject command)
        {
            return Channel.ReadAnalogOutput(command);
        }

        public CommandResult ReadDigitalInput(CommandObject command)
        {
            return Channel.ReadDigitalInput(command);
        }

        public CommandResult ReadDigitalOutput(CommandObject command)
        {
            return Channel.ReadDigitalOutput(command);
        }

        public CommandResult WriteAnalogOutput(CommandObject command)
        {
            return Channel.WriteAnalogOutput(command);
        }

        public CommandResult WriteDigitalOutput(CommandObject command)
        {
            return Channel.WriteDigitalOutput(command);
        }
    }
}
