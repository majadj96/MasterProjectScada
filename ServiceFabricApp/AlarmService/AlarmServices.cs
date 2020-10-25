using Common.AlarmEvent;
using ScadaCommon.ServiceContract;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using PubSubCommon;
using AlarmService.Channels;

namespace AlarmService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AlarmServices : IAlarmService
    {
        private AlarmPublish alarmPublish;
        private AlarmCache alarmCache;
        private EventProxy eventProxy;

        public AlarmServices()
        {
            this.alarmPublish = new AlarmPublish();
            eventProxy = new EventProxy("EventServiceEndpoint");
            alarmCache = new AlarmCache(alarmPublish);
        }

        public bool AcknowledgeAlarm(Alarm alarm)
        {
            Alarm cacheAlarm = alarmCache.FindAlarmInCache(alarm);

			ServiceEventSource.Current.Message("Alarm acknowledged");

			if (cacheAlarm.AbnormalIndicator != true)
            {
                alarmCache.RemoveAlarm(cacheAlarm);

                ReportAlarmEvent(alarm, AlarmOperation.DELETE);
                return true;
            }
            else
            {
                cacheAlarm.AlarmAcknowledged = (DateTime)alarm.AlarmAcknowledged;
                cacheAlarm.Username = alarm.Username;
                cacheAlarm.AlarmAck = true;
                alarmCache.UpdateAlarm(cacheAlarm);

                ReportAlarmEvent(alarm, AlarmOperation.ACKNOWLEDGE);

                return true;
            }
        }

        public void AddAlarm(Alarm alarm)
        {
            Event alarmEventToReport;
            alarmCache.AddAlarm(alarm);
            AlarmToEventConverter(alarm, out alarmEventToReport);

            eventProxy.AddEvent(alarmEventToReport);
		}

        public List<Alarm> GetAllAlarms()
        {
            return alarmCache.GetAllAlarms();
        }

        private void AlarmToEventConverter(Alarm alarm, out Event alarmEvent)
        {
            Event retAlarmEvent = new Event()
            {
                GiD = alarm.GiD,
                EventReported = alarm.AlarmReported,
                Message = alarm.Message,
                EventReportedBy = alarm.AlarmReportedBy,
                PointName = alarm.PointName
            };

            alarmEvent = retAlarmEvent;
        }

        private void ReportAlarmEvent(Alarm alarm, AlarmOperation operation)
        {
            Event alarmEventToReport;
            string alarmEventMessage;

            AlarmToEventConverter(alarm, out alarmEventToReport);
            alarmEventMessage = alarmEventToReport.Message;

            switch (operation)
            {
                case AlarmOperation.DELETE:
                    alarmEventToReport.Message = String.Format("Remove '{0}', Reason: 'Alarm is acknowledged'", alarmEventMessage);
                    break;
                case AlarmOperation.ACKNOWLEDGE:
                    alarmEventToReport.Message = String.Format("Acknowledge '{0}' alarm", alarmEventMessage);
                    break;
            }
            alarmEventToReport.Message = String.Format("Acknowledge '{0}' alarm", alarmEventMessage);

            eventProxy.AddEvent(alarmEventToReport);
        }
    }
}
