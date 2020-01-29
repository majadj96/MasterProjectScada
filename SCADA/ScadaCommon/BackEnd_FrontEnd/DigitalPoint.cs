using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    [DataContract]
    public class DigitalPoint : IProcessingObject
    {
        [DataMember]
        private DState state;
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
        public DigitalPoint()
        {
        }

        [DataMember]
        public DState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
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
