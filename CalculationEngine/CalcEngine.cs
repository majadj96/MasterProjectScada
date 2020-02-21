using CalculationEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    public class CalcEngine
    {
        public static Dictionary<long, IdObject> ConcreteModel = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> ConcreteModel_Copy = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> ConcreteModel_Old = new Dictionary<long, IdObject>();

        public CalcEngine()
        {

        }
    }
}
