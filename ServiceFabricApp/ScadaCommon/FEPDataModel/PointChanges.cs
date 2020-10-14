using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.FEPDataModel
{
    public class PointChanges
    {
        int transactionId;
        string commandOwner;
        Dictionary<Tuple<PointType, ushort>, ushort> changes = new Dictionary<Tuple<PointType, ushort>, ushort>();

        public int TransactionId
        {
            get
            {
                return transactionId;
            }
            set
            {
                transactionId = value;
            }
        }

        public string CommandOwner
        {
            get
            {
                return commandOwner;
            }
            set
            {
                commandOwner = value;
            }
        }

        public Dictionary<Tuple<PointType, ushort>, ushort> Changes
        {
            get
            {
                return changes;
            }
            set
            {
                changes = value;
            }
        }
    }
}
