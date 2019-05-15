using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.BackEnd_FrontEnd
{
    [ServiceContract]
    public interface IFEPConfigService
    {
        [OperationContract]
        void SendConfiguration(List<BasePointCacheItem> basePointCacheItems);
    }
}
