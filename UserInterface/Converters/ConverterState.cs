using UserInterface.Model;

namespace UserInterface.Converters
{
    public static class ConverterState
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

        public static DiscreteState ConvertToDiscreteState(double value)
        {
            if(value > 0.5)
                return DiscreteState.ON;
            else
                return DiscreteState.OFF;

        }
    }
}
