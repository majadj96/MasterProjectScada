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
    public class AnalogPoint : IProcessingObject
    {
       /* private double eguValue;
        private double rawValue;
        private PointType pointType;
        private int adress;
        private DateTime timestamp;
        private bool inAlarm;*/
        public AnalogPoint()
        {
        }

        [DataMember]
        public double EguValue
        {
            get;
            set;
        }
    }
}
