using Common.GDA;
using System;
using TransactionManagerContracts;

namespace NetworkDynamicService.Transaction
{
    public class ModelUpdateContract : IModelUpdateContract
    {
        public UpdateResult UpdateModel(Delta delta)
        {
            try
            {
                Console.WriteLine("Update model invoked");

                TMProxy _proxy = new TMProxy(new TransactionService());
                _proxy.Enlist();
            }
            catch (Exception e)
            {
                return new UpdateResult() { Result = ResultType.Failed, Message = e.Message };
            }

            return new UpdateResult() { Result = ResultType.Succeeded };
        }
    }
}
