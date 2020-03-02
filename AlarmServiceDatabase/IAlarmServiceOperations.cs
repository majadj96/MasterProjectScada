using Common.AlarmEvent;
using System.Collections.Generic;
using System.ServiceModel;

namespace AlarmEventServiceDatabase
{
    [ServiceKnownType(typeof(Alarm))]
    [ServiceContract]
    public interface IAlarmServiceOperations
    {
        [OperationContract]
        void AddAlarm(Alarm alarm);

        [OperationContract]
        bool DeleteAlarm(int id);

        [OperationContract]
        bool AcknowledgeAlarm(Alarm alarm);

        [OperationContract]
        List<Alarm> GetAllAlarms();
    }
}
