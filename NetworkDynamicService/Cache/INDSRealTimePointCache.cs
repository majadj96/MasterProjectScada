using Common.GDA;
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
        void InitializePointCache(List<ResourceDescription> inputPoints);
        bool TryGetBasePointItem(long gid, out BasePointCacheItem basePointCacheItem);
        void StoreDelta(Delta delta);
    }
}
