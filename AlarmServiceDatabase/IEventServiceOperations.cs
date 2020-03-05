using Common.AlarmEvent;
using System.Collections.Generic;
using System.ServiceModel;

namespace AlarmEventServiceDatabase
{
    [ServiceKnownType(typeof(Event))]
    [ServiceContract]
    public interface IEventServiceOperations
    {
        [OperationContract]
        void AddEvent(Event newEvent);

        [OperationContract]
        List<Event> GetAllEvents();
    }
}
