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
            eventCache = new EventCache();
        }

        public bool AcknowledgeAlarm(Alarm alarm)
        {
            Alarm cacheAlarm = alarmCache.FindAlarmInCache(alarm);
            
            if(alarm.AbnormalIndicator != true)
            {
                alarmCache.RemoveAlarm(cacheAlarm);
                return true;
            }
            else
            {
                cacheAlarm.AlarmAcknowledged = (DateTime)alarm.AlarmAcknowledged;
                cacheAlarm.Username = alarm.Username;
                cacheAlarm.AlarmAck = true;
                alarmCache.UpdateAlarm(cacheAlarm);
                return true;
            }
        }

        public void AddAlarm(Alarm alarm)
        {
            alarmCache.AddAlarm(alarm);
        }

        public void AddEvent(Event newEvent)
        {
            alarmEventDB.AddEvent(newEvent);
            publisherProxy.PublishEvent(newEvent, "event");
        }

        public List<Alarm> GetAllAlarms()
        {
            return alarmCache.GetAllAlarms();
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

        private IPub CreatePublisherProxy()
        {
            string endpointAddressString = "net.tcp://localhost:7001/Pub";
            EndpointAddress endpointAddress = new EndpointAddress(endpointAddressString);
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            return ChannelFactory<IPub>.CreateChannel(netTcpBinding, endpointAddress);
        }

        private void AlarmToEventConverter(Alarm alarm, out Event alarmEvent)
        {
            Event retAlarmEvent = new Event();

            alarmEvent = retAlarmEvent;
        }
    }
}
