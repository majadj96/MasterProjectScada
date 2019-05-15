using System.Collections.Generic;
using System.ServiceModel;
using BackEndProcessorService.Proxy;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService
{
    public class BackEndPocessingModule : IBackendProcessor
    {
        private IPointUpdateService pointUpdateProxy;
        private AlarmEventServiceProxy alarmEventServiceProxy;
        public List<IProcessingData> ProcessingPipeline { get; set; }
        public List<IProcessingData> CommandingPipeline { get; set; }
        public BackEndPocessingModule(IPointUpdateService pointUpdateProxy, AlarmEventServiceProxy alarmEventServiceProxy)
        {
            this.alarmEventServiceProxy = alarmEventServiceProxy;
            this.pointUpdateProxy = pointUpdateProxy;
            ProcessingPipeline = new List<IProcessingData>();
            CommandingPipeline = new List<IProcessingData>();
            InitializeProcessingModules();
        }

        public void Process(ProcessingObject[] inputObj)
        {
            for (int index = 0; index < inputObj.Length; index++)
            {
                foreach (var item in ProcessingPipeline)
                {
                    item.Process(inputObj);
                }
            }

            this.pointUpdateProxy.UpdatePoint(inputObj);
        }

        public void CommandingProcess(ProcessingObject[] inputObj)
        {
            for (int index = 0; index < inputObj.Length; index++)
            {
                foreach (var item in CommandingPipeline)
                {
                    item.Process(inputObj);
                }
            }
        }

        private void InitializeProcessingModules()
        {
            this.ProcessingPipeline.Add(new EGUConverterModule());
            this.ProcessingPipeline.Add(new AlarmingModule(this.alarmEventServiceProxy));

            this.CommandingPipeline.Add(new AlarmingModule(this.alarmEventServiceProxy));
            this.CommandingPipeline.Add(new RawConverterModule());
        }
    }
}
