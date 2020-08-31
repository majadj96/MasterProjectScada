using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ComandingModel
{
    public class FEPCommandObject
    {
        private ushort adress;
        private int rawValue;
        private ushort transactionId;
        private string commandOwner;

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

        public ushort TransactionId
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
        public ushort Address
        {
            get
            {
                return adress;
            }
            set
            {
                adress = value;
            }
        }
        public int RawValue
        {
            get
            {
                return rawValue;
            }
            set
            {
                rawValue = value;
            }
        }
    }
}
