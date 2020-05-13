using RepositoryCore;
using RepositoryCore.Interfaces;
using Common.AlarmEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmEventServiceInfrastructure
{
    public class AlarmEventRepository : IAlarmEventRepository
    {
        AlarmEventContext context = new AlarmEventContext();

        public void AddEvent(Event newEvent)
        {
            if (newEvent == null)
                return;

            context.Events.Add(newEvent);
            context.SaveChanges();
        }

        public List<Event> GetAllEvents()
        {
            if (context.Events.Count() > 0)
                return context.Events.ToList();
            else
                return new List<Event>();
        }
    }
}
