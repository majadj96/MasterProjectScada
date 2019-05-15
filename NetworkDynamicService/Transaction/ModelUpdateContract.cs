using Common.GDA;
using NetworkDynamicService.Cache;
using ScadaCommon.BackEnd_FrontEnd;
using System;
using System.ServiceModel;
using TransactionManagerContracts;

namespace NetworkDynamicService.Transaction
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ModelUpdateContract : IModelUpdateContract
    {
        private INDSRealTimePointCache nDSRealTimePointCache;
        private IFEPConfigService nDSBasePointCacheItems;
        private ITransactionSteps transactionService;
        private TMProxy transactionManagerProxy;
        public ModelUpdateContract(INDSRealTimePointCache nDSRealTimePointCache, IFEPConfigService nDSBasePointCacheItems, ITransactionSteps transactionService)
        {
            this.nDSRealTimePointCache = nDSRealTimePointCache;
            this.nDSBasePointCacheItems = nDSBasePointCacheItems;
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
            nDSRealTimePointCache.StoreDelta(delta, this.nDSBasePointCacheItems);

            return new UpdateResult() { Result = ResultType.Succeeded };
        }
    }
}
