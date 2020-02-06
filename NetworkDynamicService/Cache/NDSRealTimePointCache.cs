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
    public class NDSRealTimePointCache : INDSRealTimePointCache
    {
        private Dictionary<long, BasePointCacheItem> pointCache = new Dictionary<long, BasePointCacheItem>();
        private Delta model;

        public NDSRealTimePointCache()
        {

        }

        public void InitializePointCache(List<ResourceDescription> inputPoints)
        {

        }

        public bool TryGetBasePointItem(long gid, out BasePointCacheItem basePointCacheItem)
        {
            return pointCache.TryGetValue(gid, out basePointCacheItem);
        }

        public void StoreDelta(Delta delta)
        {
            this.model = delta;
        }
    }
}
