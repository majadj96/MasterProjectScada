using ScadaCommon.BackEnd_FrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ServiceContract
{
    [ServiceContract]
    public interface IBackEndProessingData
    {
        [OperationContract]
        void Process(IProcessingObject processingObject);
    }
}
