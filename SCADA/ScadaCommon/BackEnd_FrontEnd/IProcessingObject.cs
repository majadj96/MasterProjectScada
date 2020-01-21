using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    public interface IProcessingObject
    {
        double EguValue { get; set; }
        double RawValue { get; set; }
        PointType PointType { get; set; }
    }
}
