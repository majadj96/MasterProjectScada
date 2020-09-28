using Common.AlarmEvent;
using ScadaCommon.ServiceContract;
using System.Collections.Generic;
using System.ServiceModel;
using EventService.Channels;

namespace EventService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class EventServices : IEventService
    {
        private EventPublish eventPublish;
        private EventCache eventCache;

        public EventServices()
        {
            this.eventPublish = new EventPublish();
            eventCache = new EventCache(eventPublish);
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