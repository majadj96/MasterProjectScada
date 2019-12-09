using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Core
{
    public class ConductingEquipment : Equipment
    {
        public ConductingEquipment(long gID) : base(gID)
        {
        }

        private List<long> terminals = new List<long>();

        public List<long> Terminals { get => terminals; set => terminals = value; }
    }
}
