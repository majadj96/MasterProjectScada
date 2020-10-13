using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDataProccessorService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ProccessingService : IProcessingServiceContract
    {
        IBackendProcessor backendProcessor;
        public ProccessingService(IBackendProcessor backendProcessor)
        {
            this.backendProcessor = backendProcessor;
        }
        public void Process(ProcessingObject[] inputObj)
        {
            backendProcessor.Process(inputObj);
        }
    }
}
