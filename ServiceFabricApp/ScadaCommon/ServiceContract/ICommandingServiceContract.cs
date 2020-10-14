using Common;
using ScadaCommon.ComandingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface ICommandingServiceContract
    {
        [OperationContract]
        CommandResult WriteDigitalOutput(CommandObject command);
        [OperationContract]
        CommandResult WriteAnalogOutput(CommandObject command);
        [OperationContract]
        CommandResult ReadDigitalInput(CommandObject command);
        [OperationContract]
        CommandResult ReadAnalogInput(CommandObject command);
        [OperationContract]
        CommandResult ReadDigitalOutput(CommandObject command);
        [OperationContract]
        CommandResult ReadAnalogOutput(CommandObject command);
        [OperationContract]
        bool SetPointOperationMode(long signalGid, OperationMode operationMode);
    }
}
