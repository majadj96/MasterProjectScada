using System.Collections.Generic;
using System.ServiceModel;
using ScadaCommon.Database;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService.Proxy
{
    public class AlarmEventServiceProxy : ClientBase<IAlarmEventService>, IAlarmEventService
    {
        public AlarmEventServiceProxy(string endpointName) : base(endpointName) { }

        public bool AcknowledgeAlarm(IAlarm alarm)
        {
            if (Channel.AcknowledgeAlarm(alarm))
                return true;

            return false;
        }

        public bool AddAlarm(IAlarm alarm)
        {
            if (Channel.AddAlarm(alarm))
                return true;

            return false;
        }

        public bool AddEvent(IEvent newEvent)
        {
            if (Channel.AddEvent(newEvent))
                return true;

            return false;
        }

        public List<Alarm> GetAllAlarms()
        {
            return Channel.GetAllAlarms();
        }
    }
}
