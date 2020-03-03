using NetworkDynamicService.Cache;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ComandingModel;
using ScadaCommon.NDSDataModel;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CommandingService : ICommandingServiceContract
    {
        private IFEPCommandingServiceContract fepCmdProxy;
        private IBackendProcessor backendProcessor;
        private ProcessingObject[] processingObjects = new ProcessingObject[1];
        INDSRealTimePointCache nDSRealTimePointCache;
        public CommandingService(IFEPCommandingServiceContract fepCmdProxy, IBackendProcessor backendProcessor, INDSRealTimePointCache nDSRealTimePointCache)
        {
            this.nDSRealTimePointCache = nDSRealTimePointCache;
            this.fepCmdProxy = fepCmdProxy;
            this.backendProcessor = backendProcessor;
        }

        public CommandResult ReadAnalogInput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.ANALOG_INPUT_16);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.ReadAnalogInput(ProcessingObjectToFEPCommandObject(processingObjects));
            return CommandResult.Success;
        }

        public CommandResult ReadAnalogOutput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.ANALOG_OUTPUT_16);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.ReadAnalogOutput(ProcessingObjectToFEPCommandObject(processingObjects));

            return CommandResult.Success;
        }

        public CommandResult ReadDigitalInput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.BINARY_INPUT);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.ReadDigitalInput(ProcessingObjectToFEPCommandObject(processingObjects));

            return CommandResult.Success;
        }

        public CommandResult ReadDigitalOutput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.BINARY_OUTPUT);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.ReadDigitalOutput(ProcessingObjectToFEPCommandObject(processingObjects));

            return CommandResult.Success;
        }

        public CommandResult WriteAnalogOutput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.ANALOG_OUTPUT_16);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.WriteAnalogOutput(ProcessingObjectToFEPCommandObject(processingObjects));

            return CommandResult.Success;
        }

        public CommandResult WriteDigitalOutput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.BINARY_OUTPUT);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.WriteDigitalOutput(ProcessingObjectToFEPCommandObject(processingObjects));

            return CommandResult.Success;
        }

        private ProcessingObject CommandObjectToProcessingObject(CommandObject command, PointType pointType)

        {
            BasePointCacheItem basePointCacheItem;
            this.nDSRealTimePointCache.TryGetBasePointItem(command.SignalGid, out basePointCacheItem);

            switch (pointType)
            {
                case PointType.ANALOG_INPUT_16:
                    return new AnalogPoint() { Gid = command.SignalGid, PointType = pointType, EguValue = command.EguValue, InAlarm = false, Adress = ((AnalogPointCacheItem)basePointCacheItem).Address, MaxValue = ((AnalogPointCacheItem)basePointCacheItem).MaxValue, MinValue = ((AnalogPointCacheItem)basePointCacheItem).MinValue, NormalValue = ((AnalogPointCacheItem)basePointCacheItem).NormalValue };
                case PointType.ANALOG_OUTPUT_16:
                    return new AnalogPoint() { Gid = command.SignalGid, PointType = pointType, EguValue = command.EguValue, InAlarm = false, Adress = ((AnalogPointCacheItem)basePointCacheItem).Address, MaxValue = ((AnalogPointCacheItem)basePointCacheItem).MaxValue, MinValue = ((AnalogPointCacheItem)basePointCacheItem).MinValue, NormalValue = ((AnalogPointCacheItem)basePointCacheItem).NormalValue };
                case PointType.BINARY_INPUT:
                    return new DigitalPoint() { Gid = command.SignalGid, PointType = pointType, InAlarm = false, State =  (DState)command.EguValue, Adress = ((DigitalPointCacheItem)basePointCacheItem).Address, NormalValue = (int)((DigitalPointCacheItem)basePointCacheItem).NormalValue };
                case PointType.BINARY_OUTPUT:
                    return new DigitalPoint() { Gid = command.SignalGid, PointType = pointType, InAlarm = false, State = (DState)command.EguValue, Adress = ((DigitalPointCacheItem)basePointCacheItem).Address, NormalValue = (int)((DigitalPointCacheItem)basePointCacheItem).NormalValue };
                default:
                    return null;
            }
        }

        private FEPCommandObject ProcessingObjectToFEPCommandObject(ProcessingObject[] processingObjects)
        {
            return new FEPCommandObject() { Address = (ushort)processingObjects[0].Adress, RawValue = (int)processingObjects[0].RawValue };
        }
    }
}
