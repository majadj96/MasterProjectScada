using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    public class PowerCalculator
    {
        public float NominalVoltage { get; } = 220;
        public float NominalCurrent { get; } = 5;

        /// <summary>
        /// TapChanger positions (-7, 7) change Voltage +-5% by step
        /// </summary>
        /// <param name="tcPosition">Position of the TapChanger</param>
        /// <returns>Voltage</returns>
        public float GetVoltagePerTCPosition(float tcPosition)
        {
            if (tcPosition >= 0)
            {
                return (float)(NominalVoltage + NominalVoltage * tcPosition * 0.05);
            }
            else
            {
                return (float)(NominalVoltage - NominalVoltage * tcPosition * 0.05);
            }
        }

        public float GetActivePower(float voltage, float current)
        {
            return voltage * current;
        }

        public float GetPressure(float activePower)
        {
            return activePower / 1000;
        }
    }
}
