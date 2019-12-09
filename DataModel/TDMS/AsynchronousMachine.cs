using DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TDMS
{
    public class AsynchronousMachine : RegulatingCondEq
    {
        public AsynchronousMachine(long gID) : base(gID)
        {
        }

        private float cosPhi;
        private float ratedP;

        public float CosPhi { get => cosPhi; set => cosPhi = value; }
        public float RatedP { get => ratedP; set => ratedP = value; }
    }
}
