using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Core
{
    public class PowerSystemResource : IdentifiedObject
    {
        public PowerSystemResource(long gID) : base(gID)
        {
        }

        private List<long> measurements = new List<long>();

        public List<long> Measurements { get => measurements; set => measurements = value; }
    }
}
