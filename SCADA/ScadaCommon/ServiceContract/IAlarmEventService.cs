using ScadaCommon.Database;
using System.Collections.Generic;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IAlarmEventService
    {
        [OperationContract]
        bool AddAlarm(IAlarm alarm);

        [OperationContract]
        bool AcknowledgeAlarm(IAlarm alarm);

        [OperationContract]
        List<Alarm> GetAllAlarms();

        [OperationContract]
        bool AddEvent(IEvent newEvent);
    }
}
