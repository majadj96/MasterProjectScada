﻿using ScadaCommon.Database;
using System.Collections.Generic;
using System.ServiceModel;

namespace AlarmEventServiceDatabase
{
    [ServiceKnownType(typeof(Event))]
    [ServiceContract]
    public interface IEventServiceOperations
    {
        [OperationContract]
        bool AddEvent(IEvent newEvent);

        [OperationContract]
        List<Event> GetAllEvents();
    }
}
