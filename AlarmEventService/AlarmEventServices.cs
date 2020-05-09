using Common.AlarmEvent;
using AlarmEventServiceInfrastructure;
using ScadaCommon.ServiceContract;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using PubSubCommon;

namespace AlarmEventService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AlarmEventServices : IAlarmEventService
    {
        private AlarmEventRepository alarmEventDB = new AlarmEventRepository();
        public static IPub publisherProxy;
        public bool AcknowledgeAlarm(Alarm alarm)
        {
            Alarm ret = alarmEventDB.AcknowledgeAlarm(alarm);
            if(ret != null)
            {
                publisherProxy.PublishAlarm(new AlarmDescription(ret, AlarmOperation.UPDATE), "alarm");
                return true;
            }

            return false;
        }

        public void AddAlarm(Alarm alarm)
        {
            alarmEventDB.AddAlarm(alarm);
            publisherProxy.PublishAlarm(new AlarmDescription(alarm, AlarmOperation.INSERT), "alarm");
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
