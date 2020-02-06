using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.NDSDataModel
{
    public class AnalogPointCacheItem : BasePointCacheItem
    {
        private float rawValue;
        private float eguValue;
        public AnalogPointCacheItem() : base()
        {
        }

        public float RawValue
        {
            get
            {
                return rawValue;
            }
            set
            {
                rawValue = value;
            }
        }

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
