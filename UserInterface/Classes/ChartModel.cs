using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Classes
{
    public class ChartModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }

        public ChartModel(DateTime dateTime, double value)
        {
            this.DateTime = dateTime;
            this.Value = value;
        }
    }
}
