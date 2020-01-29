using ScadaCommon.Interfaces;
using ScadaCommon;

namespace FrontEndProcessorService.PointDataModel
{
    internal abstract class DigitalBase : BasePointItem, IDigitalPoint
    {
		private DState state;

		public DigitalBase(IConfigItem c, int i) 
			: base(c, i)
		{
        }

		public DState State
		{
			get
			{
				return state;
			}

			set
			{
				state = value;
			}
		}
    }
}
