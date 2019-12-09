using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Core
{
    public class Equipment : PowerSystemResource
    {
        public Equipment(long gID) : base(gID)
        {
        }

        private long equipmentContainer = 0;

        public long EquipmentContainer { get => equipmentContainer; set => equipmentContainer = value; }
    }
}
