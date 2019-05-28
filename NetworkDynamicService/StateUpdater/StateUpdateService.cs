using System;
using ScadaCommon;
using ScadaCommon.ServiceContract;

namespace NetworkDynamicService.PointUpdater
{
    public class StateUpdateService : IStateUpdateService
    {
        private StateUpdateServiceProxy proxy = new StateUpdateServiceProxy("StateUpdateServiceEndPoint");

        public StateUpdateService()
        {
            proxy.Open();
        }
        public void UpdateDateAndTime(DateTime dateTime)
        {
            proxy.UpdateDateAndTime(dateTime);
        }

        public void UpdateState(ConnectionState connectionState)
        {
            proxy.UpdateState(connectionState);
            Console.WriteLine(connectionState.ToString());
        }
    }
}
