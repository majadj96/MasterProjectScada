using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.NDSDataModel
{
    [DataContract]
    [KnownTypeAttribute(typeof(DState))]
    public class DigitalPointCacheItem : BasePointCacheItem
    {
        private DState state;
        public DigitalPointCacheItem() : base()
        {
        }
        [DataMember]
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
