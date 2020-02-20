using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDynamicService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ProcessingService : IProcessingServiceContract
    {
        IBackendProcessor backendProcessor;
        public ProcessingService(IBackendProcessor backendProcessor)
        {
            this.backendProcessor = backendProcessor;
        }
        public void Process(ProcessingObject[] inputObj)
        {
            backendProcessor.Process(inputObj);
        }
    }
}
