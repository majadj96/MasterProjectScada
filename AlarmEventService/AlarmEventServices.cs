using ScadaCommon.Database;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;

namespace AlarmEventService
{
    public class AlarmEventServices : IAlarmEventService
    {
        private AlarmServiceRef.AlarmServiceOperationsClient alarmServiceClient;
        private EventServiceRef.EventServiceOperationsClient eventServiceClient;

        public AlarmEventServices()
        {
            alarmServiceClient = new AlarmServiceRef.AlarmServiceOperationsClient();
            eventServiceClient = new EventServiceRef.EventServiceOperationsClient();
        }

        public bool AcknowledgeAlarm(Alarm alarm)
        {
            bool ret = true;
            if (alarm == null)
                ret = false;

            if(String.IsNullOrEmpty(alarm.ID.ToString()) || String.IsNullOrWhiteSpace(alarm.ID.ToString()))
                ret = false;

            alarmServiceClient.AcknowledgeAlarm(alarm);

            return ret;
        }

        public void AddAlarm(Alarm alarm)
        {
            if (alarm == null)
                return;

            alarmServiceClient.AddAlarm(alarm);
        }

        public void AddEvent(Event newEvent)
        {
            if (newEvent == null)
                return;

            eventServiceClient.AddEvent(newEvent);
        }

        public List<Alarm> GetAllAlarms()
        {
            List<Alarm> list = new List<Alarm>(alarmServiceClient.GetAllAlarms());
            return list;
        }

        public List<Event> GetAllEvents()
        {
            List<Event> list = new List<Event>(eventServiceClient.GetAllEvents());
            return list;
        }
    }
}
