using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Model
{
    public class Tank
    {
        public Tank(string name, long signalGid, float capacity, float currentFluidLevel)
        {
            Name = name;
            SignalGid = signalGid;
            Capacity = capacity;
            CurrentFluidLevel = currentFluidLevel;
        }

        public string Name { get; }
        public long SignalGid { get; }
        public float Capacity { get; set; }
        public float CurrentFluidLevel { get; set; }
        public bool IsHighLimitLevel
        {
            get
            {
                return CurrentFluidLevel > Capacity * 0.9;
            }
        }
        public bool IsLowLimitLevel
        {
            get
            {
                return CurrentFluidLevel < Capacity * 0.1;
            }
        }
    }
}
