using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace SCADAModelUpdateService
{
    public class TransactionStepsCallback : ITransactionSteps
    {
        private IRealTimeCacheService realTimeCacheProxy;
        public TransactionStepsCallback(IRealTimeCacheService realTimeCacheProxy)
        {
            this.realTimeCacheProxy = realTimeCacheProxy;
        }
        public bool Commit()
        {
            Console.WriteLine("Commit called");

            //usvajanje novog modela
            realTimeCacheProxy.ApplyUpdate();
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
