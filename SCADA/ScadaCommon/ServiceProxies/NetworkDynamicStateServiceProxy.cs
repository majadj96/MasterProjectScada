using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;
using System;
using System.ServiceModel;

namespace ScadaCommon.ServiceProxies
{
    public class NetworkDynamicStateServiceProxy : ClientBase<IStateUpdateService>, IStateUpdateService
    {
        public NetworkDynamicStateServiceProxy(string endpointName) : base(endpointName)
        {

        }
        
        public void UpdateDateAndTime(DateTime dateTime)
        {
            Channel.UpdateDateAndTime(dateTime);
        }

        public void UpdateState(short connectionState)
        {
            Channel.UpdateState(connectionState);
        }
    }
}
