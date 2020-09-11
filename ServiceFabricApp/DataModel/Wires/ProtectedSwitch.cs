using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Wires
{
    [DataContract]
    public class ProtectedSwitch : Switch
    {
        public ProtectedSwitch(long gID) : base(gID)
        {
        }

        public override bool Equals(object x)
        {
            return base.Equals(x);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
