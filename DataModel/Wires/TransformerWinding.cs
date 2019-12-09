using DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Wires
{
    public class TransformerWinding : ConductingEquipment
    {
        private long powerTransformer = 0;
        private long ratioTapChanger = 0;
        public TransformerWinding(long gID) : base(gID)
        {
        }

        public long PowerTransformer { get => powerTransformer; set => powerTransformer = value; }
        public long RatioTapChanger { get => ratioTapChanger; set => ratioTapChanger = value; }
    }
}
