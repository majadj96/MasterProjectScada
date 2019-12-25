using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaCommon.BackEnd_FrontEnd;

namespace BackEndProcessorService
{
    public class BackEndPocessingModule : IBackEndProcessingData
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
