using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IBackEndProessingData
    {
        [OperationContract]
        void Process(ProcessingObject[] inputObj);
    }
}
