using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEndProcessorService.Configuration;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;

namespace BackEndProcessorService
{
    public class EGUModule : IProcessingData
    {
        public void Process(IProcessingObject[] processingObject)
        {
            foreach (var item in processingObject)
            {
                if(item.PointType == PointType.ANALOG_INPUT_16 || item.PointType == PointType.ANALOG_OUTPUT_16)
                {
                    ((AnalogPoint)item).EguValue = NDSConfiguration.GetScalingFactor(item.PointType) * item.RawValue + NDSConfiguration.GetDeviation(item.PointType);
                }
                else
                {
                    if(item.RawValue == 0)
                    {
                        ((DigitalPoint)item).State = DState.OFF;
                    }
                    else
                    {
                        ((DigitalPoint)item).State = DState.ON;
                    }
                }
            }
        }

        public INDSConfiguration NDSConfiguration
        {
            get { return new ConfigReader(); }
        }
    }
}
