using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ScadaCommon.BackEnd_FrontEnd
{
    [DataContract]
    [KnownType(typeof(IProcessingObject))]
    public class AnalogPoint : IProcessingObject
    {
        [DataMember]
        private double eguValue;
        [DataMember]
        private double rawValue;
        [DataMember]
        private PointType pointType;
        [DataMember]
        private int adress;
        [DataMember]
        private DateTime timestamp;
        [DataMember]
        private bool inAlarm;
        public AnalogPoint()
        {
        }

        [DataMember]
        public double EguValue
        {
            get
            {
                return eguValue;
            }
            set
            {
                eguValue = value;
            }
        }
        [DataMember]
        public double RawValue
        {
            get
            {
                return rawValue;
            }
            set
            {
                rawValue = value;
            }
        }
        [DataMember]
        public PointType PointType
        {
            get
            {
                return pointType;
            }
            set
            {
                pointType = value;
            }
        }
        [DataMember]
        public int Adress
        {
            get
            {
                return adress;
            }
            set
            {
                adress = value;
            }
        }
        [DataMember]
        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
            }
        }
        [DataMember]
        public bool InAlarm
        {
            get
            {
                return inAlarm;
            }
            set
            {
                inAlarm = value;
            }
        }
    }
}
