using ScadaCommon;
using ScadaCommon.ComandingModel;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    public class CommandingProxy : ClientBase<ICommandingServiceContract>, ICommandingServiceContract
    {
        public CommandingProxy(string endpointName) : base(endpointName)
        {

        }

        public CommandResult ReadAnalogInput(CommandObject command)
        {
            try
            {
                return Channel.ReadAnalogInput(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CommandResult.Failure;
            }
        }

        public CommandResult ReadAnalogOutput(CommandObject command)
        {
            try
            {
                return Channel.ReadAnalogOutput(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CommandResult.Failure;
            }
        }

        public CommandResult ReadDigitalInput(CommandObject command)
        {
            try
            {
                return Channel.ReadDigitalInput(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CommandResult.Failure;
            }
        }

        public CommandResult ReadDigitalOutput(CommandObject command)
        {
            try
            {
                return Channel.ReadDigitalOutput(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ServiceEventSource.Current.Message(e.Message);
                return CommandResult.Failure;
            }
        }

        public CommandResult WriteAnalogOutput(CommandObject command)
        {
            try
            {
                return Channel.WriteAnalogOutput(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ServiceEventSource.Current.Message(e.Message);
                return CommandResult.Failure;
            }
        }

        public CommandResult WriteDigitalOutput(CommandObject command)
        {
            try
            {
                return Channel.WriteDigitalOutput(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CommandResult.Failure;
            }
        }

        public CommandObject CreateCommand(DateTime dateTime, string commandOwner, float value, long gid)
        {
            return new CommandObject()
            {
                CommandingTime = dateTime,
                CommandOwner = commandOwner,
                EguValue = value,
                SignalGid = gid
            };
        }
    }
}
