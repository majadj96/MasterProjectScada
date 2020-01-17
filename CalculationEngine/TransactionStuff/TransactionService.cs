using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace CalculationEngine
{
    public class TransactionService : ITransactionSteps
    {
        public bool Commit()
        {
            Console.WriteLine("Commit pozvan");
            return true;
        }

        public bool Prepare()
        {
            Console.WriteLine("Prepare pozvan");
            return true;
        }

        public void Rollback()
        {
            Console.WriteLine("Rollback pozvan");
        }
    }
}
