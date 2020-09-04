using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.GDA;
using DataModel.Core;

namespace DataModel.Wires
{
    public class ACLineSegment : Conductor
    {
        public ACLineSegment(long gID) : base(gID)
        {
        }
        
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
            
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
