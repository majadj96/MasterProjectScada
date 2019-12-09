using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Core
{
    public class Terminal : IdentifiedObject
    {
        public Terminal(long gID) : base(gID)
        {
        }

        private long connectivityNode = 0;
        private long conductingEquipment = 0;
        private List<long> measurements = new List<long>();

        public long ConnectivityNode { get => connectivityNode; set => connectivityNode = value; }
        public long ConductingEquipment { get => conductingEquipment; set => conductingEquipment = value; }
        public List<long> Measurements { get => measurements; set => measurements = value; }
    }
}
