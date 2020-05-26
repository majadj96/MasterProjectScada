using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class AlarmHandler
    {
        public List<Alarm> Alarms = new List<Alarm>();
        public event EventHandler UpdateAlarmsCollection;

        public void ProcessAlarm(AlarmDescription alarmDesc)
        {
            switch (alarmDesc.AlarmOperation)
            {
                case AlarmOperation.INSERT:
                    Alarms.Add(alarmDesc.Alarm);
                    break;
                case AlarmOperation.UPDATE:
                    int i = Alarms.FindIndex(a => a.AlarmKey == alarmDesc.Alarm.AlarmKey);
                    Alarms[i] = alarmDesc.Alarm;
                    break;
                case AlarmOperation.DELETE:
                    int index = Alarms.FindIndex(a => a.AlarmKey == alarmDesc.Alarm.AlarmKey);
                    Alarms.RemoveAt(index);
                    break;
                default:
                    break;
            }

            UpdateAlarmsCollection?.Invoke(null, null);
        }
    }
}
