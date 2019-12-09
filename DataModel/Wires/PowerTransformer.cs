using DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Wires
{
    public class PowerTransformer : Equipment
    {
        private List<long> transformerWindings = new List<long>();
        public PowerTransformer(long gID) : base(gID)
        {
        }

        public List<long> TransformerWindings { get => transformerWindings; set => transformerWindings = value; }
    }
}
