using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Meas
{
    public class Discrete : Measurement
    {
        public Discrete(long gID) : base(gID)
        {
        }

        private int maxValue;
        private int minValue;
        private int normalValue;

        public int MaxValue { get => maxValue; set => maxValue = value; }
        public int MinValue { get => minValue; set => minValue = value; }
        public int NormalValue { get => normalValue; set => normalValue = value; }
    }
}
