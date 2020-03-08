using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PubSubCommon
{
    [Serializable]
    [DataContract]
    public class ScadaUIExchangeModel
    {
        private long gid;
        private double value;
        private DateTime time;

        public ScadaUIExchangeModel()
        {

        }

        [DataMember]
        public long Gid
        {
            get
            {
                return gid;
            }
            set
            {
                gid = value;
            }
        }

        [DataMember]
        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        [DataMember]
        public DateTime Time
        {
            get
            {
                return time;
            }
            set
            {
                this.time = value;
            }
        }
    }
}
