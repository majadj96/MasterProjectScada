using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;
using System.ServiceModel;

namespace ScadaCommon.ServiceProxies
{
    public class NetworkDynamicServiceProxy : ClientBase<IProcessingServiceContract>, IProcessingServiceContract
    {
        public NetworkDynamicServiceProxy(string endpointName) : base(endpointName)
        {

        }

        public void Process(ProcessingObject[] inputObj)
        {
            Channel.Process(inputObj);
        }
    }
}
