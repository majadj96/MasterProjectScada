using ScadaCommon;
using ScadaCommon.Interfaces;

namespace FrontEndProcessorService.PointDataModel
{
    internal abstract class AnalogBase : BasePointItem, IAnalogPoint 
	{
		private double eguValue;

		public AnalogBase(IConfigItem c, int i)
			: base(c, i)
		{
		}

		public double EguValue
		{
			get
			{
				return eguValue;
			}

			set
			{
				eguValue = value;
			}
		}

		public override string DisplayValue
		{
			get
			{
				return EguValue.ToString();
			}
		}
    }
}
