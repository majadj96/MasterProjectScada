using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService.ProxyPool
{
    class PointUpdateProxy : ClientBase<IPointUpdateService>, IPointUpdateService
    {
        public PointUpdateProxy(string endpointName) : base(endpointName)
        {
        }

        public void UpdatePoint(IProcessingObject[] processingObject)
        {
            Channel.UpdatePoint(processingObject);
        }
    }
}
