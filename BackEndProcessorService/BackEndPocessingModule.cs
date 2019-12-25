using System.Collections.ObjectModel;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService
{
    public class BackEndPocessingModule : IBackEndProessingData
    {
        public ObservableCollection<IProcessingData> ProcessingModules { get; set; }
        public BackEndPocessingModule() { }
        public void Process(IProcessingObject processingObject)
        {
            foreach (var item in ProcessingModules)
            {
                item.Process(processingObject);
            }
        }
    }
}
