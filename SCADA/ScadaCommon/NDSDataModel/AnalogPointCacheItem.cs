using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.NDSDataModel
{
    [DataContract]
    public class AnalogPointCacheItem : BasePointCacheItem
    {
        private float eguValue;
        public AnalogPointCacheItem() : base()
        {
        }

        [DataMember]
        public float EguValue
        {
            get
            {
                return eguValue;
            }
            set
            {
                eguValue = value;
            }
        }
    }
}
