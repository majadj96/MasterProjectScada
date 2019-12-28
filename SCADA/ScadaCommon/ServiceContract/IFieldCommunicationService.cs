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
        void WriteDigitalOutput(int adress, int value);
        [OperationContract]
        void WriteAnalogOutput(int adress, int value);
        [OperationContract]
        void ReadDigitalInput(int adress);
        [OperationContract]
        void ReadAnalogInput(int adress);
        [OperationContract]
        void ReadDigitalOutput(int adress);
        [OperationContract]
        void ReadAnalogOutput(int adress);
    }
}
