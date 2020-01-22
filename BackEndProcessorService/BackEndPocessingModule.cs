using System.Collections.Generic;
using System.Collections.ObjectModel;
using BackEndProcessorService.Proxy;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService
{
    public class BackEndPocessingModule : IBackEndProessingData
    {
        private PointUpdateProxy pointUpdateProxy = new PointUpdateProxy("UpdatePointEndPoint");
        public List<IProcessingData> ProcessingModules { get; set; }
        public BackEndPocessingModule()
        {
            InitializeProcessingModules();
            pointUpdateProxy.Open();
        }

        public void Process(IProcessingObject processingObject)
        {
            foreach (var item in ProcessingModules)
            {
                item.Process(processingObject);
            }
            this.pointUpdateProxy.UpdatePoint(processingObject);
        }

        private void InitializeProcessingModules()
        {
            this.ProcessingModules = new List<IProcessingData>();
            this.ProcessingModules.Add(new EGUModule());
            this.ProcessingModules.Add(new AlarmingModule());
        }
    }
}
