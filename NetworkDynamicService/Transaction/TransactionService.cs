using NetworkDynamicService.Cache;
using System;
using TransactionManagerContracts;

namespace NetworkDynamicService.Transaction
{
    public class TransactionService : ITransactionSteps
    {
        private INDSRealTimePointCache nDSRealTimePointCache;
        private Action openProxiesAction;
        public TransactionService(INDSRealTimePointCache nDSRealTimePointCache, Action openProxiesAction)
        {
            this.nDSRealTimePointCache = nDSRealTimePointCache;
            this.openProxiesAction = openProxiesAction;
        }
        public bool Commit()
        {
            Console.WriteLine("Commit called");

            //usvajanje novog modela
            this.nDSRealTimePointCache.ApplyUpdate();
            return true;
        }

        public bool Prepare()
        {
            Console.WriteLine("Prepare called");
            this.openProxiesAction();
            return true;
        }

        public void Rollback()
        {
            Console.WriteLine("Rollback called");
        }
    }
}
