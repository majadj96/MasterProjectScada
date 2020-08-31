using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    [DataContract]
    [KnownTypeAttribute(typeof(AnalogPoint))]
    [KnownTypeAttribute(typeof(DigitalPoint))]
    [KnownTypeAttribute(typeof(PointType))]
    public class ProcessingObject
    {
        public ProcessingObject() { }
        [DataMember]
        public long Gid { get; set; }
        [DataMember]
        public double RawValue { get; set; }
        [DataMember]
        public PointType PointType { get; set; }
        [DataMember]
        public int Adress { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public bool InAlarm { get; set; }
        [DataMember]
        public PointFlag Flag { get; set; } = 0x0;
    }
}
