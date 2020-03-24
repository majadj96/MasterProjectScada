using CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionManagerContracts;

namespace CalculationEngine
{
	public class TransactionService : ITransactionSteps
	{
		public bool Prepare()
		{
			Console.WriteLine("CE Prepare");
			return true;
		}

		public bool Commit()
		{
			Console.WriteLine("CE Commit");

			CalcEngine.ConcreteModel_Old = new Dictionary<long, IdObject>(CalcEngine.ConcreteModel);
			CalcEngine.ConcreteModel = new Dictionary<long, IdObject>(CalcEngine.ConcreteModel_Copy);
			CalcEngine.ConcreteModel_Copy.Clear();

            ProcessingData.CalculateData();

            if(CalcEngine.aTimer == null)
            {
                CalcEngine.SetTimer();
            }

			return true;
		}

		public void Rollback()
		{
			Console.WriteLine("CE Rollback");

			CalcEngine.ConcreteModel = new Dictionary<long, IdObject>(CalcEngine.ConcreteModel_Old);
			CalcEngine.ConcreteModel_Copy.Clear();
		}
	}
}
