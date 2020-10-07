using Common.AlarmEvent;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using UserInterface.Model;
using UserInterface.ProxyPool;

namespace UserInterface
{
    public class AlarmHandler
    {
        public List<Alarm> Alarms = new List<Alarm>();
        private Dictionary<long, Measurement> _measurements = new Dictionary<long, Measurement>();

        public event EventHandler UpdateAlarmsCollection;

        public AlarmHandler(Dictionary<long, Measurement> measurements)
        {
            _measurements = measurements;

            try
            {
                Alarms = ProxyServices.AlarmEventServiceProxy.GetAllAlarms();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while requesting Alarms");
            }

            for (int i = 0; i < Alarms.Count; i++)
            {
                ChangeAlarmPointName(Alarms[i]);
            }
        }

        public void ProcessAlarm(AlarmDescription alarmDesc)
        {
            switch (alarmDesc.AlarmOperation)
            {
                case AlarmOperation.INSERT:
                    ChangeAlarmPointName(alarmDesc.Alarm);
                    Alarms.Add(alarmDesc.Alarm);
                    break;
                case AlarmOperation.UPDATE:
                    ChangeAlarmPointName(alarmDesc.Alarm);
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
            Messenger.Default.Send<Measurement>(_measurements[alarmDesc.Alarm.GiD]);
        }

        private void ChangeAlarmPointName(Alarm alarm)
        {
            if(_measurements.TryGetValue(alarm.GiD, out Measurement meas))
            {
                alarm.PointName = meas.Name;
            }
        }
    }
}
