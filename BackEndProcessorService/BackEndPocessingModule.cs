using System;
using System.Collections.Generic;
using System.ServiceModel;
using BackEndProcessorService.Proxy;
using PubSubCommon;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.ServiceContract;

namespace BackEndProcessorService
{
    public class BackEndPocessingModule : IBackendProcessor
    {
        private IPointUpdateService pointUpdateProxy;
        private IPub publisherProxy;
        private AlarmEventServiceProxy alarmEventServiceProxy;
        public List<IProcessingData> ProcessingPipeline { get; set; }
        public List<IProcessingData> CommandingPipeline { get; set; }
        public BackEndPocessingModule(IPointUpdateService pointUpdateProxy, AlarmEventServiceProxy alarmEventServiceProxy, IPub publisherProxy)
        {
            this.alarmEventServiceProxy = alarmEventServiceProxy;
            this.pointUpdateProxy = pointUpdateProxy;
            this.publisherProxy = publisherProxy;
            ProcessingPipeline = new List<IProcessingData>();
            CommandingPipeline = new List<IProcessingData>();
            InitializeProcessingModules();
        }

        public void Process(ProcessingObject[] inputObj)
        {
            List<ScadaUIExchangeModel> measurement = new List<ScadaUIExchangeModel>();
            for (int index = 0; index < inputObj.Length; index++)
            {
                foreach (var item in ProcessingPipeline)
                {
                    item.Process(inputObj);
                }
                if (inputObj[index].PointType == PointType.ANALOG_INPUT_16 || inputObj[index].PointType == PointType.ANALOG_OUTPUT_16) {
                    measurement.Add(new ScadaUIExchangeModel() { Gid = inputObj[index].Gid, Time = DateTime.Now, Value = ((AnalogPoint)(inputObj[index])).EguValue });
                }
                else
                {
                    measurement.Add(new ScadaUIExchangeModel() { Gid = inputObj[index].Gid, Time = DateTime.Now, Value = inputObj[index].RawValue });
                }
            }

            this.pointUpdateProxy.UpdatePoint(inputObj);
            publisherProxy.PublishMeasure(measurement.ToArray(), "scada");
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
