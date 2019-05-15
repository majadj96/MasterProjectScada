using ScadaCommon;
using ScadaCommon.ComandingModel;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndProcessorService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FEPCommandingService : IFEPCommandingServiceContract
    {
        private IProcessingManager processingManager = null;
        private IConfiguration configuration;

        public IConfiguration Configuration
        {
            get { return configuration; }
        }

        public IProcessingManager ProcessingManager
        {
            get { return processingManager; }
        }
        public FEPCommandingService(IProcessingManager processingManager, IConfiguration configuration)
        {
            this.processingManager = processingManager;
            this.configuration = configuration;
        }
        public void WriteDigitalOutput(FEPCommandObject cmdObject)
        {
            ProcessingManager.ExecuteWriteCommand(PointType.DIGITAL_OUTPUT, cmdObject.TransactionId, Configuration.UnitAddress, cmdObject.Address, cmdObject.RawValue, cmdObject.CommandOwner);
        }

        public void WriteAnalogOutput(FEPCommandObject cmdObject)
        {
            ProcessingManager.ExecuteWriteCommand(PointType.ANALOG_OUTPUT, cmdObject.TransactionId, Configuration.UnitAddress, cmdObject.Address, cmdObject.RawValue, cmdObject.CommandOwner);
        }

        public void ReadDigitalInput(FEPCommandObject cmdObject)
        {
            ProcessingManager.ExecuteReadCommand(PointType.DIGITAL_INPUT, cmdObject.TransactionId, Configuration.UnitAddress, cmdObject.Address, 0, cmdObject.CommandOwner);
        }

        public void ReadAnalogInput(FEPCommandObject cmdObject)
        {
            ProcessingManager.ExecuteReadCommand(PointType.ANALOG_INPUT, cmdObject.TransactionId, Configuration.UnitAddress, cmdObject.Address, 0, cmdObject.CommandOwner);
        }

        public void ReadDigitalOutput(FEPCommandObject cmdObject)
        {
            ProcessingManager.ExecuteReadCommand(PointType.DIGITAL_OUTPUT, cmdObject.TransactionId, Configuration.UnitAddress, cmdObject.Address, 0, cmdObject.CommandOwner);
        }

        public void ReadAnalogOutput(FEPCommandObject cmdObject)
        {
            ProcessingManager.ExecuteReadCommand(PointType.ANALOG_OUTPUT, cmdObject.TransactionId, Configuration.UnitAddress, cmdObject.Address, 0, cmdObject.CommandOwner);
        }
    }
}
