using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using UserInterface.ProxyPool;

namespace UserInterface
{
    public class CustomEventHandler
    {
        public List<Event> Events = new List<Event>();

        public CustomEventHandler()
        {
            try
            {
                List<Event> events = ProxyServices.AlarmEventServiceProxy.GetAllEvents();
                Events = events.OrderByDescending(e => e.EventReported).ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Error while requesting Events");
            }
        }

        public event EventHandler UpdateEventsCollection;

        public void ProcessEvent(Event eventObject)
        {
            Events.Insert(0, eventObject);

            UpdateEventsCollection?.Invoke(null, null);
        }
    }
}
