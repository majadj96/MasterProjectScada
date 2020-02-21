using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public class Discrete : IdObject
    {
        private MeasurementType measurementType;
        private int maxValue;
        private int minValue;
        private int normalValue;

        public Discrete(long gid) : base(gid)
        {
            GID = gid;
        }

        public MeasurementType MeasurementType { get => measurementType; set => measurementType = value; }
        public int MaxValue { get => maxValue; set => maxValue = value; }
        public int MinValue { get => minValue; set => minValue = value; }
        public int NormalValue { get => normalValue; set => normalValue = value; }
    }
}
