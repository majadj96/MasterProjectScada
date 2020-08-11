using CalculationEngine.Model;
using System;
using System.Collections.Generic;
using TransactionManagerContracts;

namespace CalculationEngine
{
    public class TransactionService : ITransactionSteps
	{
        public static ProcessingData _processingData;

        public bool Prepare()
		{
			Console.WriteLine("CE Prepare");
            ServiceEventSource.Current.Message("CE Prepare");
			return true;
		}

		public bool Commit()
		{
			Console.WriteLine("CE Commit");
            ServiceEventSource.Current.Message("CE Commit");

            ConcreteModel.BackupModel = new Dictionary<long, IdObject>(ConcreteModel.CurrentModel);
			ConcreteModel.CurrentModel = new Dictionary<long, IdObject>(ConcreteModel.CurrentModel_Copy);
			ConcreteModel.CurrentModel_Copy.Clear();

            //_processingData.UpdateAsyncMachines();
            _processingData.IsModelChanged = true;

            CalcEngine.aTimer.Start();

            return true;
		}

		public void Rollback()
		{
			Console.WriteLine("CE Rollback");
            ServiceEventSource.Current.Message("CE Rollback");

            ConcreteModel.CurrentModel = new Dictionary<long, IdObject>(ConcreteModel.BackupModel);
			ConcreteModel.CurrentModel_Copy.Clear();
		}
	}
}
