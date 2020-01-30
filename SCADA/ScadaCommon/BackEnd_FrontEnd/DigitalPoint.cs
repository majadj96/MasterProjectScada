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
    public class DigitalPoint : IProcessingObject
    {
        public DigitalPoint()
        {
        }

        [DataMember]
        public DState State
        {
            get;set;
        }
    }
}
