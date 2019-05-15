using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    public interface IInputObject
    {
        IProcessingObject[] Changes { get; set; }
    }
}
