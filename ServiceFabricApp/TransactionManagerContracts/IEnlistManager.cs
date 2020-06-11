using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TransactionManagerContracts
{

    [ServiceContract(CallbackContract = typeof(ITransactionSteps))]
    public interface IEnlistManager
    {
        [OperationContract]
        void Enlist();

        [OperationContract]
        void EndEnlist(bool isSuccessful);
    }


}
