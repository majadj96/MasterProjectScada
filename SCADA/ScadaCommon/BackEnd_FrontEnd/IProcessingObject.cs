using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    public interface IProcessingObject
    {
        [DataMember]
        double RawValue { get; set; }
        [DataMember]
        PointType PointType { get; set; }
        [DataMember]
        int Adress { get; set; }
        [DataMember]
        DateTime Timestamp { get; set; }
        [DataMember]
        bool InAlarm { get; set; }
    }
}
