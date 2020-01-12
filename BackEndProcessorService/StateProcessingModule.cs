using System;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService
{
    public class StateProcessingModule : IStateProcessingModule
    {
        public StateProcessingModule() {  }
        
        public void ProcessState(IProcessingState processingObject)
        {
            Console.WriteLine("Process state working!");
        }
    }
}
