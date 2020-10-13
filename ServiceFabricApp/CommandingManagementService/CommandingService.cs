using Common;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ComandingModel;
using ScadaCommon.Interfaces;
using ScadaCommon.NDSDataModel;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommandingManagementService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CommandingService : ICommandingServiceContract
    {
        private IFEPCommandingServiceContract fepCmdProxy;
        private IBackendProcessor backendProcessor;
        private ProcessingObject[] processingObjects = new ProcessingObject[1];
        private ushort transactionId = 0;
        IRealTimeCacheService nDSRealTimePointCache;
        public CommandingService(IFEPCommandingServiceContract fepCmdProxy, IBackendProcessor backendProcessor, IRealTimeCacheService nDSRealTimePointCache)
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
            fepCmdProxy.ReadAnalogInput(ProcessingObjectToFEPCommandObject(processingObjects, command.CommandOwner));
            return CommandResult.Success;
        }

        public CommandResult ReadAnalogOutput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.ANALOG_OUTPUT_16);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.ReadAnalogOutput(ProcessingObjectToFEPCommandObject(processingObjects, command.CommandOwner));

            return CommandResult.Success;
        }

        public CommandResult ReadDigitalInput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.BINARY_INPUT);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.ReadDigitalInput(ProcessingObjectToFEPCommandObject(processingObjects, command.CommandOwner));

            return CommandResult.Success;
        }

        public CommandResult ReadDigitalOutput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.BINARY_OUTPUT);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.ReadDigitalOutput(ProcessingObjectToFEPCommandObject(processingObjects, command.CommandOwner));

            return CommandResult.Success;
        }

        public CommandResult WriteAnalogOutput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.ANALOG_OUTPUT_16);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);
            fepCmdProxy.WriteAnalogOutput(ProcessingObjectToFEPCommandObject(processingObjects, command.CommandOwner));

            return CommandResult.Success;
        }

        public CommandResult WriteDigitalOutput(CommandObject command)
        {
            ProcessingObject processingObject = CommandObjectToProcessingObject(command, PointType.BINARY_OUTPUT);
            processingObjects[0] = processingObject;
            backendProcessor.CommandingProcess(processingObjects);

            fepCmdProxy.WriteDigitalOutput(ProcessingObjectToFEPCommandObject(processingObjects, command.CommandOwner));

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

        private FEPCommandObject ProcessingObjectToFEPCommandObject(ProcessingObject[] processingObjects, string commandOwner)
        {
            return new FEPCommandObject() {
                Address = (ushort)processingObjects[0].Adress,
                RawValue = (int)processingObjects[0].RawValue,
                TransactionId = GetTransactionId(),
                CommandOwner = commandOwner,
            };
        }
        private ushort GetTransactionId()
        {
            if (transactionId == 15)
            {
                transactionId = 0;
            }
            else
            {
                transactionId++;
            }

            return transactionId;
        }

        public bool SetPointOperationMode(long signalGid, OperationMode operationMode)
        {
            if(this.nDSRealTimePointCache.TryGetBasePointItem(signalGid, out BasePointCacheItem basePointCacheItem))
            {
                try
                {
                    this.fepCmdProxy.SetPointOperationMode(basePointCacheItem.Type, basePointCacheItem.Address, operationMode);
                    basePointCacheItem.OperationMode = operationMode;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
