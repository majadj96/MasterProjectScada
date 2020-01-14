using ScadaCommon.BackEnd_FrontEnd;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IStateProcessingModule
    {
        [OperationContract]
        void ProcessState(IProcessingState processingObject);
    }
}
