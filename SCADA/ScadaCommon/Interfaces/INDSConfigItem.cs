using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.Interfaces
{
    public interface INDSConfigItem
    {
        PointType RegistryType { get; }

        uint ScalingFactor { get; }
        uint Deviation { get; }
        uint NormalValue { get; }
        uint LowLimit { get; }
        uint HighLimit { get; }
    }
}
