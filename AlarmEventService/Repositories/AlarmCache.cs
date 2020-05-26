using Common;
using Common.AlarmEvent;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmEventService.Repositories
{
    public class AlarmCache
    {
        private Dictionary<string, Alarm> alarmCache = new Dictionary<string, Alarm>();
        private IPub alarmPublisher;
        public AlarmCache(IPub alarmPublisher)
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

            if (!alarmCache.ContainsKey(alarmKey)) {
                alarmCache.Add(alarmKey, alarm);
                alarmPublisher.PublishAlarm(new AlarmDescription(alarm, AlarmOperation.INSERT), "alarm");
            }
            else
            {
                alarmCache[alarmKey] = alarm;
                alarmPublisher.PublishAlarm(new AlarmDescription(alarm, AlarmOperation.UPDATE), "alarm");
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
                alarmPublisher.PublishAlarm(new AlarmDescription(alarm, AlarmOperation.UPDATE), "alarm");
            }
        }

        public void RemoveAlarm(Alarm alarm)
        {
            string alarmKey = string.Empty;
            alarmKey = GetAlarmKey(alarm.Category, alarm.GiD);

            if (alarmCache.ContainsKey(alarmKey))
            {
                alarmCache.Remove(alarmKey);
                alarmPublisher.PublishAlarm(new AlarmDescription(alarm, AlarmOperation.DELETE), "alarm");
            }
        }

        private string GetAlarmKey(AlarmCategory category, long gid)
        {
            return String.Format("{0}^{1}", category.ToString(), gid.ToString());
        }
    }
}
