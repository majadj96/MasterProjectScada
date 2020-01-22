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
            Console.WriteLine("Update model invoked");

            TMProxy _proxy = new TMProxy(new TransactionService());
            _proxy.Enlist();
            

            //_proxy.EndEnlist(true);

            return new UpdateResult();
        }
    }
}
