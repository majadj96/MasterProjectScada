using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public class CustomEventHandler
    {
        public List<Event> Events = new List<Event>();

        public CustomEventHandler(List<Event> events)
        {
            Events = events.OrderByDescending(e => e.EventReported).ToList();
        }

        public event EventHandler UpdateEventsCollection;

        public void ProcessEvent(Event eventObject)
        {
            Events.Insert(0, eventObject);

            UpdateEventsCollection?.Invoke(null, null);
        }
    }
}
