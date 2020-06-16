using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public class TapChanger : IdObject
    {
        public TapChanger(long gID) : base(gID)
        {
        }

        public long Winding { get; set; }
        public int HighStep { get; set; }
        public int LowStep { get; set; }
        public int NormalStep { get; set; }
    }
}
