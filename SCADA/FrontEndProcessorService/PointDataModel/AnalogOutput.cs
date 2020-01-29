using ScadaCommon;
using ScadaCommon.Interfaces;
using System;

namespace FrontEndProcessorService.PointDataModel
{
    internal class AnalogOutput : AnalogBase
	{

		public AnalogOutput(IConfigItem c, int i)
			: base (c, i)
		{		
		}
    }
}