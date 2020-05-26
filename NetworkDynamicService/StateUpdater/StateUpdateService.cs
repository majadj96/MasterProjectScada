using System;
using System.ServiceModel;
using PubSubCommon;
using ScadaCommon;
using ScadaCommon.ServiceContract;

namespace NetworkDynamicService.PointUpdater
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class StateUpdateService : IStateUpdateService
    {
        private IPub publisherProxy;

        public StateUpdateService(IPub publisherProxy)
        {
            this.publisherProxy = publisherProxy;
        }

        public void UpdateState(ConnectionState connectionState)
        {
            publisherProxy.PublishConnectionState(connectionState, "connectionState");
            Console.WriteLine(connectionState.ToString());
        }
    }
}
