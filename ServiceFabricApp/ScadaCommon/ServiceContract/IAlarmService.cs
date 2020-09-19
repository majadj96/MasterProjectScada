using Common.AlarmEvent;
using System.Collections.Generic;
using System.ServiceModel;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IAlarmService
    {
        [OperationContract]
        void AddAlarm(Alarm alarm);

        [OperationContract]
        bool AcknowledgeAlarm(Alarm alarm);

        [OperationContract]
        List<Alarm> GetAllAlarms();
    }
}
