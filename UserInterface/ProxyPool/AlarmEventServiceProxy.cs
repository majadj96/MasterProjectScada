using RepositoryCore;
using RepositoryCore.Interfaces;
using ScadaCommon;
using ScadaCommon.ComandingModel;
using Common.AlarmEvent;
using ScadaCommon.ServiceContract;
using System.Collections.Generic;
using System.ServiceModel;

namespace UserInterface.ProxyPool
{
    public class AlarmEventServiceProxy : ClientBase<IAlarmEventRepository>, IAlarmEventRepository
    {
        public AlarmEventServiceProxy(string endpointName) : base(endpointName)
        {

        }

        public bool AcknowledgeAlarm(Alarm alarm)
        {
            return Channel.AcknowledgeAlarm(alarm);
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
