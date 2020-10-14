using Common.GDA;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ServiceProxies
{
    public class RealTimeCacheServiceProxy : ClientBase<IRealTimeCacheService>, IRealTimeCacheService
    {
        public RealTimeCacheServiceProxy(string endpointName) : base(endpointName) { }
        public void ApplyUpdate()
        {
            Channel.ApplyUpdate();
        }

        public void StoreDelta(Delta delta)
        {
            Channel.StoreDelta(delta);
        }

        public bool TryGetBasePointItem(long gid, out BasePointCacheItem basePointCacheItem)
        {
            return Channel.TryGetBasePointItem(gid, out basePointCacheItem);
        }
    }
}
