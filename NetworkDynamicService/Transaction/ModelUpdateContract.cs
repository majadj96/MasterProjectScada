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
        private INDSBasePointCacheItems nDSBasePointCacheItems;
        public ModelUpdateContract(INDSRealTimePointCache nDSRealTimePointCache, INDSBasePointCacheItems nDSBasePointCacheItems)
        {
            this.nDSRealTimePointCache = nDSRealTimePointCache;
            this.nDSBasePointCacheItems = nDSBasePointCacheItems;
        }
        public UpdateResult UpdateModel(Delta delta)
        {
            try
            {
                Console.WriteLine("Update model invoked");
                TransactionService transaction = new TransactionService(nDSRealTimePointCache);
                TMProxy _proxy = new TMProxy(transaction);
                _proxy.Enlist();
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
