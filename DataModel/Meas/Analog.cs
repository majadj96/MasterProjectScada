using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Meas
{
    public class Analog : Measurement
    {
        public Analog(long gID) : base(gID)
        {
        }

        private float maxValue;
        private float minValue;
        private float normalValue;

        public float MaxValue { get => maxValue; set => maxValue = value; }
        public float MinValue { get => minValue; set => minValue = value; }
        public float NormalValue { get => normalValue; set => normalValue = value; }
    }
}
