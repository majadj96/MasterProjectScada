using RepositoryCore;
using RepositoryCore.Interfaces;
using ScadaCommon.Database;
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
        public bool AcknowledgeAlarm(Alarm alarm)
        {
            bool ret = true;
            if (alarm == null)
                ret = false;

            if (String.IsNullOrEmpty(alarm.ID.ToString()) || String.IsNullOrWhiteSpace(alarm.ID.ToString()))
                ret = false;
            
            ret = AckAlarm(alarm);

            return ret;
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
            return context.Alarms.ToList();
        }

        public List<Event> GetAllEvents()
        {
            return context.Events.ToList();
        }

        private bool AckAlarm(Alarm alarm)
        {
            try
            {
                Alarm aa = context.Alarms.Find(alarm.ID);
                if (aa != null)
                {
                    aa.AlarmAcknowledged = (DateTime)alarm.AlarmAcknowledged;
                    aa.Username = alarm.Username;
                    context.SaveChanges();
                    //log
                    return true;
                }
                else
                {
                    //log
                    return false;
                }
            }
            catch
            {
                //log
                return false;
            }
        }
    }
}
