using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Core
{
    public class EquipmentContainer : ConnectivityNodeContainer
    {
        public EquipmentContainer(long gID) : base(gID)
        {
        }

        private List<long> equipments = new List<long>();

        public List<long> Equipments { get => equipments; set => equipments = value; }
    }
}
