using DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Meas
{
    public class Measurement : IdentifiedObject
    {
        public Measurement(long gID) : base(gID)
        {
        }

        private List<long> terminals = new List<long>();
        private long pSR = 0;

        public List<long> Terminals { get => terminals; set => terminals = value; }
        public long PSR { get => pSR; set => pSR = value; }
    }
}
