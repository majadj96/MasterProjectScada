using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.Model
{
    public class PointModel
    {
        public DateTime Timestamp { get; set; }
        public int Value { get; set; }
        public PointType TypeOfPoint { get; set; }
        public int Adress { get; set; }
    }
}
