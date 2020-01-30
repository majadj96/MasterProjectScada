using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    //[KnownType(typeof(AnalogPoint))]
   // [DataContract]
    public class InputObject : IInputObject
    {
        //[DataMember]
        public DigitalPoint[] Changes { get; set; }
    }
}
