using Common.GDA;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.Interfaces
{
    [ServiceContract]
    public interface IRealTimeCacheService
    {
        [OperationContract]
        bool TryGetBasePointItem(long gid, out BasePointCacheItem basePointCacheItem);
        [OperationContract]
        void StoreDelta(Delta delta);
        [OperationContract]
        void ApplyUpdate();
    }
}
