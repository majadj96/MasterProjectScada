using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ServiceProxies
{
    public class FepConfigProxy : ClientBase<IFEPConfigService>, IFEPConfigService
    {
        public FepConfigProxy(string endpointName) : base(endpointName)
        {

        }

        public void SendConfiguration(List<BasePointCacheItem> basePointCacheItems)
        {
            Channel.SendConfiguration(basePointCacheItems);
        }
    }
}
