using ScadaCommon;
using ScadaCommon.Interfaces;
using System;

namespace FrontEndProcessorService.PointDataModel
{
    internal class DigitalOutput : DigitalBase
	{

		public DigitalOutput(IConfigItem c, int i)
			: base(c, i)
		{
		}
    }
}