using System;
using System.Collections.Generic;
using System.ServiceModel;
using BackEndProcessorService.Proxy;
using PubSubCommon;
using RepositoryCore;
using RepositoryCore.Interfaces;
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
        private IMeasurementRepository measurementRepository;
        public List<IProcessingData> ProcessingPipeline { get; set; }
        public List<IProcessingData> CommandingPipeline { get; set; }
        public BackEndPocessingModule(IPointUpdateService pointUpdateProxy, AlarmEventServiceProxy alarmEventServiceProxy, IPub publisherProxy, IMeasurementRepository measurementRepository)
        {
            this.alarmEventServiceProxy = alarmEventServiceProxy;
            this.pointUpdateProxy = pointUpdateProxy;
            this.publisherProxy = publisherProxy;
            this.measurementRepository = measurementRepository;
            ProcessingPipeline = new List<IProcessingData>();
            CommandingPipeline = new List<IProcessingData>();
            InitializeProcessingModules();
        }

        public void Process(ProcessingObject[] inputObj)
        {
            List<ScadaUIExchangeModel> measurement = new List<ScadaUIExchangeModel>();
            List<Measurement> measurementsDB = new List<Measurement>();
            for (int index = 0; index < inputObj.Length; index++)
            {
                foreach (var item in ProcessingPipeline)
                {
                    item.Process(inputObj);
                }
                if (inputObj[index].PointType == PointType.ANALOG_INPUT_16 || inputObj[index].PointType == PointType.ANALOG_OUTPUT_16)
                {
                    measurement.Add(new ScadaUIExchangeModel()
                    {
                        Gid = inputObj[index].Gid,
                        Time = DateTime.Now,
                        Value = ((AnalogPoint)(inputObj[index])).EguValue,
                        Flag = inputObj[index].Flag
                    });
                    measurementsDB.Add(new Measurement() { Gid = inputObj[index].Gid, ChangedTime = DateTime.Now, Value = (int)((AnalogPoint)(inputObj[index])).EguValue });
                }
                else
                {
                    measurement.Add(new ScadaUIExchangeModel()
                    {
                        Gid = inputObj[index].Gid,
                        Time = DateTime.Now,
                        Value = inputObj[index].RawValue,
                        Flag = inputObj[index].Flag
                    });
                    measurementsDB.Add(new Measurement() { Gid = inputObj[index].Gid, ChangedTime = inputObj[index].Timestamp, Value = (int)inputObj[index].RawValue });
                }
            }
            measurementRepository.AddMeasurements(measurementsDB.ToArray());
            publisherProxy.Publish(measurement.ToArray(), "scada");
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

            this.CommandingPipeline.Add(new RawConverterModule());
        }
    }
}
