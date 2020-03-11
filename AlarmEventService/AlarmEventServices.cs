using AlarmEventServiceInfrastructure;
using ScadaCommon.Database;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmEventService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AlarmEventServices : IAlarmEventService
    {
        private AlarmEventRepository alarmEventDB = new AlarmEventRepository();
        public bool AcknowledgeAlarm(Alarm alarm)
        {
            return alarmEventDB.AcknowledgeAlarm(alarm);
        }

        public void AddAlarm(Alarm alarm)
        {
            alarmEventDB.AddAlarm(alarm);
        }

        public void AddEvent(Event newEvent)
        {
            alarmEventDB.AddEvent(newEvent);
        }

        public List<Alarm> GetAllAlarms()
        {
            return alarmEventDB.GetAllAlarms();
        }

        public List<Event> GetAllEvents()
        {
            return alarmEventDB.GetAllEvents();
        }
    }
}
