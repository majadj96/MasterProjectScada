using Common.AlarmEvent;
using PubSubCommon;
using System.Collections.Generic;

namespace EventService
{
    public class EventCache
    {
        private List<Event> eventCache = new List<Event>(100);
        private IPub eventPublisher;
        private AlarmEventRepository eventDB;

        public EventCache(IPub eventPublisher, AlarmEventRepository eventDB)
        {
            this.eventPublisher = eventPublisher;
            this.eventDB = eventDB;
        }

        public List<Event> GetAllEvents()
        {
            return eventCache;
        }

        public void AddEvent(Event newEvent)
        {
            eventCache.Add(newEvent);
            eventPublisher.Publish(newEvent, "event");
        }
    }
}
