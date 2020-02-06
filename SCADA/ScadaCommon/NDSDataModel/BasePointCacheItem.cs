using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.NDSDataModel
{
    public abstract class BasePointCacheItem
    {
        protected PointType type;
        protected ushort address;
        protected long gid;
        private DateTime timestamp = DateTime.Now;
        private string name = string.Empty;
        protected float minValue;
        protected float maxValue;
        protected float normalValue;
        protected bool inAlarm;

        public BasePointCacheItem()
        {

        }
        #region Properties
        public bool InAlarm
        {
            get
            {
                return inAlarm;
            }
            set
            {
                inAlarm = value;
            }
        }
        public float MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
            }
        }
        public float MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }
        }
        public float NormalValue
        {
            get
            {
                return normalValue;
            }
            set
            {
                normalValue = value;
            }
        }
        public long Gid {
            get
            {
                return gid;
            }
            set
            {
                gid = value;
            }
        }
        public PointType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        /// <summary>
        /// Address of point on MdbSim Simulator
        /// </summary>
        public ushort Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }

            set
            {
                timestamp = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        #endregion Properties
    }
}