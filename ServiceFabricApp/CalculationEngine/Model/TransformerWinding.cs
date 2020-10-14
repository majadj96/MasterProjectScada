using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public class TransformerWinding : IdObject
    {
        public long Transformer { get; set; }
        public TransformerWinding(long gID) : base(gID)
        {
        }
    }
}
