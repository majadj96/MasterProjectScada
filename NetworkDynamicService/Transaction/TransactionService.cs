using System;
using TransactionManagerContracts;

namespace NetworkDynamicService.Transaction
{
    public class TransactionService : ITransactionSteps
    {
        public bool Commit()
        {
            Console.WriteLine("Commit called");
            return true;
        }

        public bool Prepare()
        {
            Console.WriteLine("Prepare called");
            return true;
        }

        public void Rollback()
        {
            Console.WriteLine("Rollback called");
        }
    }
}
