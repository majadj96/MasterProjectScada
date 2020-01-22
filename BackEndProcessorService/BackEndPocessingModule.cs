using System.Collections.Generic;
using System.Collections.ObjectModel;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService
{
    public class BackEndPocessingModule : IBackEndProessingData
    {

        public List<IProcessingData> ProcessingModules { get; set; }
        public BackEndPocessingModule()
        {
            InitializeProcessingModules();
        }

        public void Process(IProcessingObject processingObject)
        {
            foreach (var item in ProcessingModules)
            {
                item.Process(processingObject);
            }
        }

        private void InitializeProcessingModules()
        {
            this.ProcessingModules = new List<IProcessingData>();
            this.ProcessingModules.Add(new EGUModule());
            this.ProcessingModules.Add(new AlarmingModule());
        }
    }
}
