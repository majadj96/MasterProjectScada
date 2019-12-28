using ScadaCommon;
using ScadaCommon.Interfaces;

namespace FrontEndProcessorService.PointDataModel
{
    internal class DigitalInput : DigitalBase
	{
		public DigitalInput(IConfigItem c, int i) 
			: base(c, i)
		{
		}
    }
}