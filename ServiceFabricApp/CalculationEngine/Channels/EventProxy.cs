using Common.AlarmEvent;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Channels
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
                //Console.WriteLine(e.Message);
                ServiceEventSource.Current.Message("CE: Error adding event. Details: " + e.Message);
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
                ServiceEventSource.Current.Message("CE: Error adding event. Details: " + e.Message);
                return new List<Event>();
            }
        }
    }
}
