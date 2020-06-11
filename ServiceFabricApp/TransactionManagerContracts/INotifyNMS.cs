using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TransactionManagerContracts
{
    [ServiceContract]
    public interface INotifyNMS
    {
        [OperationContract]
        void UpdateUIModel(bool commitSucceed = true);
    }
}
