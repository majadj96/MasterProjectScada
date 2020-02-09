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

        public bool AcknowledgeAlarm(IAlarm alarm)
        {
            bool ret = true;
            if (alarm == null)
                ret = false;

            if(String.IsNullOrEmpty(alarm.ID.ToString()) || String.IsNullOrWhiteSpace(alarm.ID.ToString()))
                ret = false;

            alarmServiceClient.AcknowledgeAlarm(alarm);

            return ret;
        }

        public bool AddAlarm(IAlarm alarm)
        {
            bool ret = true;
            if (alarm == null)
                ret = false;

            alarmServiceClient.AddAlarm(alarm);

            return ret;
        }

        public bool AddEvent(IEvent newEvent)
        {
            bool ret = true;
            if (newEvent == null)
                ret = false;

            eventServiceClient.AddEvent(newEvent);

            return ret;
        }

        public List<Alarm> GetAllAlarms()
        {
            List<Alarm> list = new List<Alarm>(alarmServiceClient.GetAllAlarms());
            return list;
        }
    }
}
