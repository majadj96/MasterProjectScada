using Common.AlarmEvent;
using AlarmEventServiceInfrastructure;
using ScadaCommon.ServiceContract;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using PubSubCommon;
using AlarmEventService.Repositories;

namespace AlarmEventService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AlarmEventServices : IAlarmEventService
    {
        private AlarmEventRepository alarmEventDB = new AlarmEventRepository();
        private IPub publisherProxy;
        private AlarmCache alarmCache;
        private EventCache eventCache;

        public AlarmEventServices()
        {
            publisherProxy = CreatePublisherProxy();
            alarmCache = new AlarmCache(publisherProxy);
            eventCache = new EventCache(publisherProxy, alarmEventDB);
        }

        public bool AcknowledgeAlarm(Alarm alarm)
        {
            Alarm cacheAlarm = alarmCache.FindAlarmInCache(alarm);

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

            eventCache.AddEvent(alarmEventToReport);
        }

        public void AddEvent(Event newEvent)
        {
            eventCache.AddEvent(newEvent);
        }

        public List<Alarm> GetAllAlarms()
        {
            return alarmCache.GetAllAlarms();
        }

        public List<Event> GetAllEvents()
        {
            return eventCache.GetAllEvents();
        }

        private IPub CreatePublisherProxy()
        {
            string endpointAddressString = "net.tcp://localhost:7001/Pub";
            EndpointAddress endpointAddress = new EndpointAddress(endpointAddressString);
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            return ChannelFactory<IPub>.CreateChannel(netTcpBinding, endpointAddress);
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

            switch (operation) {
                case AlarmOperation.DELETE:
                    alarmEventToReport.Message = String.Format("Remove '{0}', Resaon: 'Alarm is acknowledged'", alarmEventMessage);
                    break;
                case AlarmOperation.ACKNOWLEDGE:
                    alarmEventToReport.Message = String.Format("Acknowledge '{0}' alarm", alarmEventMessage);
                    break;
            }
            alarmEventToReport.Message = String.Format("Acknowledge '{0}' alarm", alarmEventMessage);
            eventCache.AddEvent(alarmEventToReport);
        }
    }
}
