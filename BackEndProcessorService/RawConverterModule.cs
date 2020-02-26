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
    public class RawConverterModule : IProcessingData
    {
        public void Process(ProcessingObject[] processingObject)
        {
            foreach (var item in processingObject)
            {
                if (item.PointType == PointType.ANALOG_INPUT_16 || item.PointType == PointType.ANALOG_OUTPUT_16)
                {
                    ((AnalogPoint)item).RawValue = (((AnalogPoint)item).EguValue - NDSConfiguration.GetDeviation(item.PointType)) / NDSConfiguration.GetScalingFactor(item.PointType);
                }
                else
                {
                    if (((DigitalPoint)item).State == DState.OFF)
                    {
                        item.RawValue = 0;
                    }
                    else
                    {
                        item.RawValue = 1;
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
