﻿using Common.AlarmEvent;
using System.Collections.Generic;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IAlarmEventService
    {
        [OperationContract]
        void AddAlarm(Alarm alarm);

        [OperationContract]
        bool AcknowledgeAlarm(Alarm alarm);

        [OperationContract]
        List<Alarm> GetAllAlarms();

        [OperationContract]
        void AddEvent(Event newEvent);

        [OperationContract]
        List<Event> GetAllEvents();
    }
}
