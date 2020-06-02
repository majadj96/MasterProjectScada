using AlarmEventServiceInfrastructure;
using Common.AlarmEvent;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmEventService.Repositories
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
