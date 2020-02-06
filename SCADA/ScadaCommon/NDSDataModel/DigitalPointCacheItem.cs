using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.NDSDataModel
{
    public class DigitalPointCacheItem : BasePointCacheItem
    {
        private DState state;
        public DigitalPointCacheItem() : base()
        {
        }

        public DState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
    }
}
