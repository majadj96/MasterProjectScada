using System.Collections.Generic;
using System.ServiceModel;
using RepositoryCore.Interfaces;
using ScadaCommon.Database;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService.Proxy
{
    public class AlarmEventServiceProxy : ClientBase<IAlarmEventService>, IAlarmEventService
    {
        public AlarmEventServiceProxy(string endpointName) : base(endpointName) { }

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

        public void AddEvent(Event newEvent)
        {
            Channel.AddEvent(newEvent);
        }

        public List<Alarm> GetAllAlarms()
        {
            return Channel.GetAllAlarms();
        }

        public List<Event> GetAllEvents()
        {
            return Channel.GetAllEvents();
        }
    }
}
