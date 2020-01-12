using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;
using System.ServiceModel;

namespace FrontEndProcessorService
{
    public class NetworkDynamicServiceProxy : ClientBase<IBackEndProessingData>, IBackEndProessingData
    {
        public NetworkDynamicServiceProxy(string endpointName) : base(endpointName)
        {

        }
        public void Process(IProcessingObject processingObject)
        {
            Channel.Process(processingObject);
        }
    }
}
