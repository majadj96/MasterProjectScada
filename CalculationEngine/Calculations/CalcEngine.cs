using CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Timers;

namespace CalculationEngine
{
    public class CalcEngine
    {       
        private ProcessingData _processingData;
        public static Timer aTimer;

        public CalcEngine()
        {
            ConcreteModel model = new ConcreteModel();

            int config = LoadConfiguration();
            _processingData = new ProcessingData(model, config);

            TransactionService transactionService = new TransactionService(_processingData, model);
            ModelUpdateContract modelUpdateService = new ModelUpdateContract(model, transactionService);
            CEServiceHost svcHost = new CEServiceHost(modelUpdateService);

            PublishMeasurements pub = new PublishMeasurements(_processingData);
            SubscribeProxy sub = new SubscribeProxy(pub);
            sub.Subscribe("scada");

            SetTimer();
        }

        private int LoadConfiguration()
        {
            string configString = ConfigurationManager.AppSettings.Get("MaxDifference");
            return int.Parse(configString);
        }

        private void SetTimer()
        {
            aTimer = new Timer(5000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if(_processingData.IsModelChanged)
            {
                _processingData.UpdateMachineStates();
                _processingData.UpdateFluidLevels();

                _processingData.IsModelChanged = false;
            }

            //_processingData.CheckTransformersMeasurements();
            //_processingData.CheckMachineMeasurements();
            //_processingData.UpdateMachineStates();
            _processingData.UpdateWorkingTimes();
            //_processingData.UpdateFluidLevels();
            _processingData.CheckFluidLevel();
        }
    }
}