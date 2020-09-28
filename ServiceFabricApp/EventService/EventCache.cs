using Common.AlarmEvent;
using EventService.Channels;
using PubSubCommon;
using System.Collections.Generic;

namespace EventService
{
    public class EventCache
    {
        private List<Event> eventCache = new List<Event>(100);
        private EventPublish eventPublish;

        public EventCache(EventPublish eventPublish)
        {
            this.eventPublish = eventPublish;
        }

        public List<Event> GetAllEvents()
        {
            return eventCache;
        }

        public void AddEvent(Event newEvent)
        {
            eventCache.Add(newEvent);
            eventPublish.SendEvent(newEvent, new System.EventArgs());
        }
    }
}
