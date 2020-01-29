using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IFieldCommunicationService
    {
        [OperationContract]
        void WriteDigitalOutput(int address, int value);
        [OperationContract]
        void WriteAnalogOutput(int address, int value);
        [OperationContract]
        void ReadDigitalInput(int address);
        [OperationContract]
        void ReadAnalogInput(int address);
        [OperationContract]
        void ReadDigitalOutput(int address);
        [OperationContract]
        void ReadAnalogOutput(int address);
    }
}
