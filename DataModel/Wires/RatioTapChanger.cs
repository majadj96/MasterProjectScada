using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Wires
{
    public class RatioTapChanger : TapChanger
    {
        private long transformerWinding = 0;
        public RatioTapChanger(long gID) : base(gID)
        {
        }

        public long TransformerWinding { get => transformerWinding; set => transformerWinding = value; }
    }
}
