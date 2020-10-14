using System.Collections.Generic;
using System.ServiceModel;
using RepositoryCore.Interfaces;
using Common.AlarmEvent;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService.Proxy
{
    public class AlarmServiceProxy : ClientBase<IAlarmService>, IAlarmService
    {
        public AlarmServiceProxy(string endpointName) : base(endpointName) { }

        public bool AcknowledgeAlarm(Alarm alarm)
        {
            if (Channel.AcknowledgeAlarm(alarm))
                return true;

            return false;
        }

        public void AddAlarm(Alarm alarm)
        {
            Channel.AddAlarm(alarm);
        }

        public List<Alarm> GetAllAlarms()
        {
            return Channel.GetAllAlarms();
        }
    }
}
