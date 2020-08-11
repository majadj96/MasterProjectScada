using CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Timers;

namespace CalculationEngine
{
    public class CalcEngine
    {       
        private PublishMeasurements _pub;
        private ProcessingData _processingData;
        public static Timer aTimer;

        public CalcEngine()
        {
            _processingData = new ProcessingData();

            LoadConfiguration();

            TransactionService._processingData = _processingData;
            _pub = new PublishMeasurements(_processingData);
            _pub.SubscribeTo("scada");

            SetTimer();
        }

        private void LoadConfiguration()
        {
            string configString = ConfigurationManager.AppSettings.Get("MaxDifference");
            if (int.TryParse(configString, out int res))
            {
                _processingData.workingDifference = res;
            }

            //configString = ConfigurationManager.AppSettings.Get("MaxFluidLevel");
            //if (float.TryParse(configString, out float result))
            //{
            //    _processingData.maxFluidLevel = result;
            //}

            //configString = ConfigurationManager.AppSettings.Get("NominalFluidLvlTank1");
            //if (float.TryParse(configString, out result))
            //{
            //    _processingData.currentFluidLvlTank1 = result;
            //}

            //configString = ConfigurationManager.AppSettings.Get("NominalFluidLvlTank2");
            //if(float.TryParse(configString, out result))
            //{
            //    _processingData.currentFluidLvlTank2 = result;
            //}                
        }

        private void SetTimer()
        {
            aTimer = new Timer(5000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if(_processingData.IsModelChanged)
            {
                _processingData.UpdateMachineStates();
                _processingData.UpdateFluidLevels();
                _processingData.IsModelChanged = false;
            }
            _processingData.UpdateWorkingTimes();
            _processingData.CheckFluidLevel();
        }
    }
}