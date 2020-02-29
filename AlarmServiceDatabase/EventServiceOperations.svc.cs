using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AlarmEventServiceDatabase.Server;
using ScadaCommon.Database;

namespace AlarmEventServiceDatabase
{
    [ServiceKnownType(typeof(Event))]
    public class EventServiceOperations : IEventServiceOperations
    {
        public void AddEvent(Event newEvent)
        {
            using (AccessDB db = new AccessDB())
            {
                try
                {

                    db.Events.Add(new Event()
                    {
                        GiD = newEvent.GiD,
                        EventReported = newEvent.EventReported,
                        EventReportedBy = newEvent.EventReportedBy,
                        Message = newEvent.Message,
                        PointName = newEvent.PointName
                    });
                    //log
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //log
                    return;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
        
        public List<Event> GetAllEvents()
        {
            using (AccessDB db = new AccessDB())
            {
                try
                {
                    List<Event> list = db.Events.ToList();
                    if (list.Count > 0)
                    {
                        //log
                        return list;
                    }
                    else
                    {
                        //log
                        return null;
                    }
                }
                catch
                {
                    //log
                    return null;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
    }
}
