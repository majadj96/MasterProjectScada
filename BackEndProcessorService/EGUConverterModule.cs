using BackEndProcessorService.Configuration;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;

namespace BackEndProcessorService
{
    public class EGUConverterModule : IProcessingData
    {
        public void Process(ProcessingObject[] processingObject)
        {
            foreach (var item in processingObject)
            {
                if (item.PointType == PointType.ANALOG_INPUT_16 || item.PointType == PointType.ANALOG_OUTPUT_16)
                {
                    ((AnalogPoint)item).EguValue = NDSConfiguration.GetScalingFactor(item.PointType) * item.RawValue + NDSConfiguration.GetDeviation(item.PointType);
                }
                else
                {
                    if (item.RawValue == 0)
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
