﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.NDSDataModel
{
    [DataContract]
    [KnownTypeAttribute(typeof(AnalogPointCacheItem))]
    [KnownTypeAttribute(typeof(DigitalPointCacheItem))]
    [KnownTypeAttribute(typeof(PointType))]
    [KnownTypeAttribute(typeof(MeasurementType))]
    public abstract class BasePointCacheItem
    {
        protected MeasurementType measurementType;
        protected string description = String.Empty;
        protected string mrId = String.Empty;
        protected PointType type;
        protected ushort address;
        protected long gid;
        protected DateTime timestamp;
        protected string name = string.Empty;
        protected float minValue;
        protected float maxValue;
        protected float normalValue;
        protected bool inAlarm = false;

        public BasePointCacheItem()
        {

        }
        #region Properties
        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        [DataMember]
        public string MrId
        {
            get
            {
                return mrId;
            }
            set
            {
                mrId = value;
            }
        }
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public MeasurementType MeasurementType
        {
            get
            {
                return measurementType;
            }

            set
            {
                measurementType = value;
            }
        }
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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