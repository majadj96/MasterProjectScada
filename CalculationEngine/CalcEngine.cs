using CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

		private static Timer aTimer;

		public CalcEngine()
        {

        }

		public static void SetTimer()
		{
			aTimer = new Timer(1000);
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		}

		private static void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
		}

		public static void ProccessData(object data)
		{

		}
	}
}
