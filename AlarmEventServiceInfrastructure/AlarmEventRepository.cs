using RepositoryCore;
using RepositoryCore.Interfaces;
using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmEventServiceInfrastructure
{
    public class AlarmEventRepository : IAlarmEventRepository
    {
        AlarmEventContext context = new AlarmEventContext();
        public Alarm AcknowledgeAlarm(Alarm alarm)
        {
            if (alarm == null)
                return null;

            if (string.IsNullOrWhiteSpace(alarm.ID.ToString()))
                return null;
            
            return AckAlarm(alarm);
        }

        public void AddAlarm(Alarm alarm)
        {
            if (alarm == null)
                return;

            context.Alarms.Add(alarm);
            context.SaveChanges();
        }

        public void AddEvent(Event newEvent)
        {
            if (newEvent == null)
                return;

            context.Events.Add(newEvent);
            context.SaveChanges();
        }

        public List<Alarm> GetAllAlarms()
        {
            if (context.Alarms.Count() > 0)
                return context.Alarms.ToList();
            else
                return new List<Alarm>();
        }

        public List<Event> GetAllEvents()
        {
            if (context.Events.Count() > 0)
                return context.Events.ToList();
            else
                return new List<Event>();
        }

        private Alarm AckAlarm(Alarm alarm)
        {
            try
            {
                Alarm aa = context.Alarms.Find(alarm.ID);
                if (aa != null)
                {
                    aa.AlarmAcknowledged = (DateTime)alarm.AlarmAcknowledged;
                    aa.Username = alarm.Username;
                    aa.AlarmAck = true;
                    context.SaveChanges();
                    //log
                    return aa;
                }
                else
                {
                    //log
                    return null;
                }
            }
            catch
            {
                //log
                return null;
            }
        }
    }
}
