using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public enum DiscreteState
    {
        ON = 1,//true
        OFF = 0//false
    }

    public static class Converter
    {
        public static bool ConvertToBool(DiscreteState state)
        {
            if (state == DiscreteState.OFF)
                return false;
            else
                return true;
        }

        public static DiscreteState ConvertToDiscreteState(bool state)
        {
            if (state)
                return DiscreteState.ON;
            else
                return DiscreteState.OFF;
        }
    }
}
