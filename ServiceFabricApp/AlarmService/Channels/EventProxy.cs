using Common.AlarmEvent;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AlarmService.Channels
{
    public class EventProxy : ClientBase<IEventService>, IEventService
    {
        public EventProxy(string endpointName) : base(endpointName) { }

        public void AddEvent(Event newEvent)
        {
            try
            {
                Channel.AddEvent(newEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<Event> GetAllEvents()
        {
            try
            {
                return Channel.GetAllEvents();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Event>();
            }
        }
    }
}
