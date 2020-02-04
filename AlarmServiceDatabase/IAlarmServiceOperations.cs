using ScadaCommon.Database;
using System.Collections.Generic;
using System.ServiceModel;

namespace AlarmEventService
{
    [ServiceKnownType(typeof(Alarm))]
    [ServiceContract]
    public interface IAlarmServiceOperations
    {
        [OperationContract]
        bool AddAlarm(IAlarm alarm);

        [OperationContract]
        bool DeleteAlarm(int id);

        [OperationContract]
        bool AcknowledgeAlarm(IAlarm alarm);

        [OperationContract]
        List<Alarm> GetAllAlarms();
    }
}
