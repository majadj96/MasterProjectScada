using AlarmService.Channels;
using Common;
using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlarmService
{
    public class AlarmCache
    {
        private Dictionary<string, Alarm> alarmCache = new Dictionary<string, Alarm>();
        private AlarmPublish alarmPublisher;

        public AlarmCache(AlarmPublish alarmPublisher)
        {
            this.alarmPublisher = alarmPublisher;
        }

        public List<Alarm> GetAllAlarms()
        {
            List<Alarm> retList = new List<Alarm>();
            retList = alarmCache.Values.ToList();
            return retList;
        }

        public void AddAlarm(Alarm alarm)
        {
            string alarmKey = string.Empty;
            alarmKey = GetAlarmKey(alarm.Category, alarm.GiD);
            alarm.AlarmKey = alarmKey;
            alarm.ID = alarmCache.Count + 1;

            if (!alarmCache.ContainsKey(alarmKey))
            {
                alarmCache.Add(alarmKey, alarm);
                alarmPublisher.SendEvent(new AlarmDescription(alarm, AlarmOperation.INSERT), new EventArgs());
				ServiceEventSource.Current.Message("Alarm added.");
			}
            else
            {
                alarmCache[alarmKey] = alarm;
                alarmPublisher.SendEvent(new AlarmDescription(alarm, AlarmOperation.UPDATE), new EventArgs());
				ServiceEventSource.Current.Message("Alarm updated.");
			}
        }

        public Alarm FindAlarmInCache(Alarm alarm)
        {
            string alarmKey = string.Empty;
            alarmKey = GetAlarmKey(alarm.Category, alarm.GiD);

            if (alarmCache.ContainsKey(alarmKey))
            {
                return alarmCache[alarmKey];
            }
            else
            {
                return null;
            }
        }

        public bool ExistAlarm(Alarm alarm)
        {
            string alarmKey = string.Empty;
            alarmKey = GetAlarmKey(alarm.Category, alarm.GiD);

            if (alarmCache.ContainsKey(alarmKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateAlarm(Alarm alarm)
        {
            string alarmKey = string.Empty;
            alarmKey = GetAlarmKey(alarm.Category, alarm.GiD);

            if (alarmCache.ContainsKey(alarmKey))
            {
                alarmCache[alarmKey] = alarm;
                alarmPublisher.SendEvent(new AlarmDescription(alarm, AlarmOperation.UPDATE), new EventArgs());
				ServiceEventSource.Current.Message("Alarm updated.");
			}
        }

        public void RemoveAlarm(Alarm alarm)
        {
            string alarmKey = string.Empty;
            alarmKey = GetAlarmKey(alarm.Category, alarm.GiD);

            if (alarmCache.ContainsKey(alarmKey))
            {
                alarmCache.Remove(alarmKey);
                alarmPublisher.SendEvent(new AlarmDescription(alarm, AlarmOperation.DELETE), new EventArgs());
				ServiceEventSource.Current.Message("Alarm removed.");
			}
        }

        private string GetAlarmKey(AlarmCategory category, long gid)
        {
            return String.Format("{0}^{1}", category.ToString(), gid.ToString());
        }
    }
}
