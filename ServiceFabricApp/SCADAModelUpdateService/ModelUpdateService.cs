using Common.GDA;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace SCADAModelUpdateService
{
    public class ModelUpdateService : IModelUpdateContract
    {
        //U Ovom servisu treba podici i implementirati sve ovo, ali FEPConfig proxy treba da se izmesti da ne ide kroz storeDelta vec da bude u cache servisu...
        private IRealTimeCacheService scadaRealTimePointCacheProxy;
        private ITransactionSteps transactionService;
        private TMProxy transactionManagerProxy;
        public ModelUpdateService(IRealTimeCacheService SCADARealTimePointCacheProxy, ITransactionSteps transactionService)
        {
            scadaRealTimePointCacheProxy = SCADARealTimePointCacheProxy;
            this.transactionService = transactionService;
            transactionManagerProxy = new TMProxy(transactionService);
        }
        public UpdateResult UpdateModel(Delta delta)
        {
            try
            {
                Console.WriteLine("Update model invoked");
                transactionManagerProxy.Enlist();
            }
            catch (Exception e)
            {
                return new UpdateResult() { Result = ResultType.Failed, Message = e.Message };
            }

            //storuj deltu 
            scadaRealTimePointCacheProxy.StoreDelta(delta);

            return new UpdateResult() { Result = ResultType.Succeeded };
        }
    }
}
