using Common.AlarmEvent;
using ScadaCommon.ServiceContract;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using PubSubCommon;

namespace EventService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class EventServices : IEventService
    {
        //private AlarmEventRepository alarmEventDB = new AlarmEventRepository();
        private IPub publisherProxy;
        private EventCache eventCache;

        public EventServices()
        {
            //publisherProxy = CreatePublisherProxy();
            eventCache = new EventCache(publisherProxy, alarmEventDB);
        }

        public void AddEvent(Event newEvent)
        {
            eventCache.AddEvent(newEvent);
        }

        public List<Event> GetAllEvents()
        {
            return eventCache.GetAllEvents();
        }
    }
}