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
        public FEPCommandingService(IProcessingManager processingManager, IConfiguration configuration)
        {
            this.processingManager = processingManager;
            this.configuration = configuration;
        }
        public void WriteDigitalOutput(FEPCommandObject cmdObject)
        {
            this.processingManager.ExecuteWriteCommand(PointType.DIGITAL_OUTPUT, configuration.GetTransactionId(), configuration.UnitAddress, cmdObject.Address, cmdObject.RawValue);
        }

        public void WriteAnalogOutput(FEPCommandObject cmdObject)
        {
            this.processingManager.ExecuteWriteCommand(PointType.ANALOG_OUTPUT, configuration.GetTransactionId(), configuration.UnitAddress, cmdObject.Address, cmdObject.RawValue);
        }

        public void ReadDigitalInput(FEPCommandObject cmdObject)
        {
            this.processingManager.ExecuteReadCommand(PointType.DIGITAL_INPUT, configuration.GetTransactionId(), configuration.UnitAddress, cmdObject.Address, 0);
        }

        public void ReadAnalogInput(FEPCommandObject cmdObject)
        {
            this.processingManager.ExecuteReadCommand(PointType.ANALOG_INPUT, configuration.GetTransactionId(), configuration.UnitAddress, cmdObject.Address, 0);
        }

        public void ReadDigitalOutput(FEPCommandObject cmdObject)
        {
            this.processingManager.ExecuteReadCommand(PointType.DIGITAL_OUTPUT, configuration.GetTransactionId(), configuration.UnitAddress, cmdObject.Address, 0);
        }

        public void ReadAnalogOutput(FEPCommandObject cmdObject)
        {
            this.processingManager.ExecuteReadCommand(PointType.ANALOG_OUTPUT, configuration.GetTransactionId(), configuration.UnitAddress, cmdObject.Address, 0);
        }

    }
}
