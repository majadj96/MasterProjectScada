using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    [DataContract]
    [KnownTypeAttribute(typeof(DState))]
    public class DigitalPoint : ProcessingObject
    {
        public DigitalPoint()
        {
        }

        [DataMember]
        public int NormalValue { get; set; }
        [DataMember]
        public int MinValue { get; set; }
        [DataMember]
        public int MaxValue { get; set; }

        [DataMember]
        public DState State
        {
            get; set;
        }
    }
}
