using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public static class ConcreteModel
    {
        public static Dictionary<long, IdObject> CurrentModel = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> CurrentModel_Copy = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> BackupModel = new Dictionary<long, IdObject>();

        public static List<Tank> Tanks = new List<Tank>();
    }
}
