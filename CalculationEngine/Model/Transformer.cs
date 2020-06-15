using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public class Transformer : IdObject
    {
        public Transformer(long gID) : base(gID)
        {
        }

        public List<long> Windings { get; set; }
    }
}
