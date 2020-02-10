using Common.GDA;
using NetworkDynamicService.Cache;
using System;
using System.ServiceModel;
using TransactionManagerContracts;

namespace NetworkDynamicService.Transaction
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ModelUpdateContract : IModelUpdateContract
    {
        private INDSRealTimePointCache nDSRealTimePointCache;
        public ModelUpdateContract(INDSRealTimePointCache nDSRealTimePointCache)
        {
            this.nDSRealTimePointCache = nDSRealTimePointCache;
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
            nDSRealTimePointCache.StoreDelta(delta);

            return new UpdateResult() { Result = ResultType.Succeeded };
        }
    }
}
