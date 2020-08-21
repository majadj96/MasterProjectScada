using Common;
using ScadaCommon;
using ScadaCommon.ComandingModel;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService.ProxyPool
{
    public class FepCommandingServiceProxy : ClientBase<IFEPCommandingServiceContract>, IFEPCommandingServiceContract
    {
        public FepCommandingServiceProxy(string endpointName) : base(endpointName)
        {

        }
        public void ReadAnalogInput(FEPCommandObject cmdObject)
        {
            Channel.ReadAnalogInput(cmdObject);
        }

        public void ReadAnalogOutput(FEPCommandObject cmdObject)
        {
            Channel.ReadAnalogOutput(cmdObject);
        }

        public void ReadDigitalInput(FEPCommandObject cmdObject)
        {
            Channel.ReadDigitalInput(cmdObject);
        }

        public void ReadDigitalOutput(FEPCommandObject cmdObject)
        {
            Channel.ReadDigitalOutput(cmdObject);
        }

        public void SetPointOperationMode(PointType pointType, ushort address, OperationMode operationMode)
        {
            Channel.SetPointOperationMode(pointType, address, operationMode);
        }

        public void WriteAnalogOutput(FEPCommandObject cmdObject)
        {
            Channel.WriteAnalogOutput(cmdObject);
        }

        public void WriteDigitalOutput(FEPCommandObject cmdObject)
        {
            Channel.WriteDigitalOutput(cmdObject);
        }
    }
}
