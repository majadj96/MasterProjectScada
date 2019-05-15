using Common.GDA;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService.Cache
{
    public interface INDSRealTimePointCache
    {
        bool TryGetBasePointItem(long gid, out BasePointCacheItem basePointCacheItem);
        void StoreDelta(Delta delta, IFEPConfigService nDSBasePointCacheItems);
        void ApplyUpdate();
    }
}
