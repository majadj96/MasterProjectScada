using System.ComponentModel;

namespace UserInterface.Model
{
    public enum DiscreteState
    {
        ON = 1,//true
        OFF = 0//false
    }

    public enum ColorState
    {
        [Description("#FF29BF30")]
        ON = 1,
        [Description("#FF29A2B5")]
        OFF = 2
    }

    public static class ColorStateExtensions
    {
        public static string ToDescriptionString(this ColorState val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}