using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.Interfaces
{
    public interface INDSConfiguration
    {
        List<INDSConfigItem> GetConfigurationItems();
        uint GetScalingFactor(PointType pointType);
        uint GetDeviation(PointType pointType);
        uint GetNormalValue(PointType pointType);
        uint GetLowLimit(PointType pointType);
        uint GetHighLimit(PointType pointType);
    }
}
