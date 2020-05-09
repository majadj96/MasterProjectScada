using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.AlarmEvent
{
    public enum AlarmOperation { INSERT, UPDATE, DELETE }
    [DataContract]
    public class AlarmDescription
    {
        [DataMember]
        public Alarm Alarm { get; set; }
        [DataMember]
        public AlarmOperation AlarmOperation { get; set; }

        public AlarmDescription(Alarm alarm, AlarmOperation alarmOperation)
        {
            Alarm = alarm;
            AlarmOperation = alarmOperation;
        }
    }
}
