using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public class ConcreteModel
    {
        public Dictionary<long, IdObject> CurrentModel { get; set; }
        public Dictionary<long, IdObject> CurrentModel_Copy { get; set; }
        public Dictionary<long, IdObject> BackupModel { get; set; }
        public List<Tank> Tanks { get; set; }

        public ConcreteModel()
        {
            CurrentModel = new Dictionary<long, IdObject>();
            CurrentModel_Copy = new Dictionary<long, IdObject>();
            BackupModel = new Dictionary<long, IdObject>();
            Tanks = new List<Tank>();
        }
    }
}
