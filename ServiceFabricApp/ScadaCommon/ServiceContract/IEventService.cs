using Common.AlarmEvent;
using System.Collections.Generic;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IEventService
    {
        [OperationContract]
        void AddEvent(Event newEvent);

        [OperationContract]
        List<Event> GetAllEvents();
    }
}
