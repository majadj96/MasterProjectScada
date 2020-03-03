using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    [DataContract]
    public class InputObject : IInputObject
    {
        [DataMember]
        public ProcessingObject[] Changes { get; set; }
    }
}
