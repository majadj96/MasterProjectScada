using System;
using System.ServiceModel;
using ScadaCommon;
using ScadaCommon.ServiceContract;

namespace NetworkDynamicService.PointUpdater
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class StateUpdateService : IStateUpdateService
    {
        private StateUpdateServiceProxy stateUpdateProxy;

        public StateUpdateService(StateUpdateServiceProxy stateUpdateProxy)
        {
            this.stateUpdateProxy = stateUpdateProxy;
        }

        public void UpdateState(ConnectionState connectionState)
        {
            //stateUpdateProxy.UpdateState(connectionState);
            Console.WriteLine(connectionState.ToString());
        }
    }
}
