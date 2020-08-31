using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public class Analog : IdObject
    {
        private MeasurementType measurementType;
        private float maxValue;
        private float minValue;
        private float normalValue;
        private long equipmentGid;

        public Analog(long gid) : base(gid)
        {
            GID = gid;
        }

        public float MaxValue { get => maxValue; set => maxValue = value; }
        public float MinValue { get => minValue; set => minValue = value; }
        public float NormalValue { get => normalValue; set => normalValue = value; }
        public MeasurementType MeasurementType { get => measurementType; set => measurementType = value; }
        public long EquipmentGid { get => equipmentGid; set => equipmentGid = value; }
    }
}
