using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common.GDA;
using TransactionManager;
using TransactionManagerContracts;

namespace CalculationEngine
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
