using Common.AlarmEvent;
using AlarmEventServiceInfrastructure;
using ScadaCommon.ServiceContract;
using System.Collections.Generic;
using System.ServiceModel;
using System;

namespace AlarmEventService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AlarmEventServices : IAlarmEventService
    {
        private AlarmEventRepository alarmEventDB = new AlarmEventRepository();
        public bool AcknowledgeAlarm(Alarm alarm)
        {
            return alarmEventDB.AcknowledgeAlarm(alarm);
        }

        public void AddAlarm(Alarm alarm)
        {
            alarmEventDB.AddAlarm(alarm);
        }

        public void AddEvent(Event newEvent)
        {
            alarmEventDB.AddEvent(newEvent);
        }

        public List<Alarm> GetAllAlarms()
        {
            try
            {
                return alarmEventDB.GetAllAlarms();
            }
            catch (Exception ex)
            {
                return new List<Alarm>();
            }
        }

        public List<Event> GetAllEvents()
        {
            try
            {
                return alarmEventDB.GetAllEvents();
            }
            catch (Exception ex)
            {
                return new List<Event>();
            }
        }
    }
}
