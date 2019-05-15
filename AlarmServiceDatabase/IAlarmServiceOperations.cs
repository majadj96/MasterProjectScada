using ScadaCommon.Database;
using System.Collections.Generic;
using System.ServiceModel;

namespace AlarmEventServiceDatabase
{
    [ServiceKnownType(typeof(Alarm))]
    [ServiceContract]
    public interface IAlarmServiceOperations
    {
        [OperationContract]
        bool AddAlarm(Alarm alarm);

        [OperationContract]
        bool DeleteAlarm(int id);

        [OperationContract]
        bool AcknowledgeAlarm(Alarm alarm);

        [OperationContract]
        List<Alarm> GetAllAlarms();
    }
}
