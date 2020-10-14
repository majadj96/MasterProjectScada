using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public class AsyncMachine : IdObject
    {
        private float cosPhi;
        private float ratedP;
        private float workingTime;
        private bool isRunning;

        public float CosPhi { get => cosPhi; set => cosPhi = value; }
        public float RatedP { get => ratedP; set => ratedP = value; }
        public float WorkingTime { get => workingTime; set => workingTime = value; }
        public bool IsRunning { get => isRunning; set => isRunning = value; }

        public AsyncMachine(long gID) : base(gID)
        {
        }
    }
}
