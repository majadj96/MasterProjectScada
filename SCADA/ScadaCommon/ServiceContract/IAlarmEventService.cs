using ScadaCommon.Database;
using System.Collections.Generic;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IAlarmEventService
    {
        [OperationContract]
        bool AddAlarm(Alarm alarm);

        [OperationContract]
        bool AcknowledgeAlarm(Alarm alarm);

        [OperationContract]
        List<Alarm> GetAllAlarms();

        [OperationContract]
        bool AddEvent(Event newEvent);
    }
}
