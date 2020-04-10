using CalculationEngine.Model;
using Common;
using PubSubCommon;
using ScadaCommon;
using ScadaCommon.ComandingModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CalculationEngine
{
    public class CalcEngine
    {
        public static Dictionary<long, IdObject> ConcreteModel = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> ConcreteModel_Copy = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> ConcreteModel_Old = new Dictionary<long, IdObject>();
        
        public static Timer aTimer = null;

        private PublishMeasurements _pub;

        public CalcEngine()
        {
            LoadConfiguration();

            _pub = new PublishMeasurements();
            _pub.SubscribeTo("scada");

            ProcessingData.commandingProxy = new CommandingProxy("CECommandingProxy");
        }

        private void LoadConfiguration()
        {
            string maxDiff = ConfigurationManager.AppSettings.Get("MaxDifference");
            int.TryParse(maxDiff, out int res);
            ProcessingData.workingDifference = res;

            string maxLevel = ConfigurationManager.AppSettings.Get("MaxFluidLevel");
            float.TryParse(maxLevel, out float result);
            ProcessingData.maxFluidLevel = result;
        }

        public static void SetTimer()
        {
            aTimer = new Timer(10000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ProcessingData.UpdateWorkingTimes();
			ProcessingData.UpdateFluidLevel();//
        }
    }
}