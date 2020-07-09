using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public class Measurement
    {
        public Measurement(long gid)
        {
            Gid = gid;
        }

        public long Gid { get; set; }
        public double Value { get; set; }
        public string Name { get; set; }
        public string Mrid { get; set; }
        public string Description { get; set; }
        public long PowerSystemResource { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public MeasurementType Type { get; set; }
        public string AlarmVisibility { get; set; } = "Hidden";
        public DateTime Time { get; set; }
        public string State
        {
            get
            {
                if (Value != 0)
                    return "Closed";
                else
                    return "Opened";
            }
        }
    }
}
