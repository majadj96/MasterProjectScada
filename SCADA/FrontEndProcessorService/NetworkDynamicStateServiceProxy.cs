using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;
using System.ServiceModel;  

namespace FrontEndProcessorService
{
    public class NetworkDynamicStateServiceProxy : ClientBase<IStateProcessingModule>, IStateProcessingModule
    {
        public NetworkDynamicStateServiceProxy(string endpointName) : base(endpointName)
        {

        }
        public void ProcessState(IProcessingState processingObject)
        {
            Channel.ProcessState(processingObject);
        }
    }
}
