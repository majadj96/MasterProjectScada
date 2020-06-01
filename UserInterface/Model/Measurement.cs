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
    }
}
