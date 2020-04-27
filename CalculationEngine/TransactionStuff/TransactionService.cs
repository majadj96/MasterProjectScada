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
        public static ProcessingData _processingData;

		public bool Prepare()
		{
			Console.WriteLine("CE Prepare");
			return true;
		}

		public bool Commit()
		{
			Console.WriteLine("CE Commit");

			ConcreteModel.BackupModel = new Dictionary<long, IdObject>(ConcreteModel.CurrentModel);
			ConcreteModel.CurrentModel = new Dictionary<long, IdObject>(ConcreteModel.CurrentModel_Copy);
			ConcreteModel.CurrentModel_Copy.Clear();

            _processingData.UpdateAsyncMachines();

            if(!CalcEngine.aTimer.Enabled)
            {
                CalcEngine.aTimer.Enabled = true;
            }

			return true;
		}

		public void Rollback()
		{
			Console.WriteLine("CE Rollback");

			ConcreteModel.CurrentModel = new Dictionary<long, IdObject>(ConcreteModel.BackupModel);
			ConcreteModel.CurrentModel_Copy.Clear();
		}
	}
}
