using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.Utility
{
    public static class UniqueIdGenerator
    {
        public static ushort transactionId = 0;

        public static ushort GetcurrentTransactionId()
        {
            return transactionId;
        }

        public static ushort GetNextTransactionId()
        {
            if (transactionId == 15)
            {
                transactionId = 0;
            }
            else
            {
                transactionId++;
            }

            return transactionId;
        }
    }
}
