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
            Console.WriteLine("Prepare pozvan");
            return true;
        }

        public bool Commit()
        {
            Console.WriteLine("Commit pozvan");

            CalcEngine.ConcreteModel_Old = new Dictionary<long, IdObject>(CalcEngine.ConcreteModel);
            CalcEngine.ConcreteModel = new Dictionary<long, IdObject>(CalcEngine.ConcreteModel_Copy);
            CalcEngine.ConcreteModel_Copy.Clear();

            return true;
        }

        public void Rollback()
        {
            Console.WriteLine("Rollback pozvan");

            CalcEngine.ConcreteModel = new Dictionary<long, IdObject>(CalcEngine.ConcreteModel_Old);
            CalcEngine.ConcreteModel_Copy.Clear();
        }
    }
}
