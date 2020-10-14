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
    public interface IFEPCommandingServiceContract
    {
        [OperationContract]
        void WriteDigitalOutput(FEPCommandObject cmdObject);
        [OperationContract]
        void WriteAnalogOutput(FEPCommandObject cmdObject);
        [OperationContract]
        void ReadDigitalInput(FEPCommandObject cmdObject);
        [OperationContract]
        void ReadAnalogInput(FEPCommandObject cmdObject);
        [OperationContract]
        void ReadDigitalOutput(FEPCommandObject cmdObject);
        [OperationContract]
        void ReadAnalogOutput(FEPCommandObject cmdObject);
        [OperationContract]
        void SetPointOperationMode(PointType pointType, ushort address, OperationMode operationMode);
    }
}
