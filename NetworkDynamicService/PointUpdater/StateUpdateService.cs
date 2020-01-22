using System;
using ScadaCommon;
using ScadaCommon.ServiceContract;

namespace NetworkDynamicService.PointUpdater
{
    public class StateUpdateService : IStateUpdateService
    {
        public void UpdateDateAndTime(DateTime dateTime)
        {
            Console.WriteLine(dateTime.ToString());
        }

        public void UpdateState(ConnectionState connectionState)
        {
            Console.WriteLine(connectionState.ToString());
        }
    }
}
