using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndProcessorService
{
    public class NDSConfigurationService : INDSBasePointCacheItems
    {
        private Dictionary<Tuple<ushort,PointType> , BasePointCacheItem> model = new Dictionary<Tuple<ushort, PointType>, BasePointCacheItem>();
        public NDSConfigurationService()
        {
        }
        public void SendConfiguration(List<BasePointCacheItem> basePointCacheItems)
        {
            foreach (BasePointCacheItem item in basePointCacheItems)
            {
                this.model.Add(new Tuple<ushort, PointType>(item.Address, item.Type), item);
            }
        }

        public Dictionary<Tuple<ushort, PointType>, BasePointCacheItem> Model
        {
            get { return model; }
        }
    }
}
