using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEndProcessorService.Configuration;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;

namespace BackEndProcessorService
{
    public class EGUModule : IProcessingData
    {
        public void Process(IProcessingObject processingObject)
        {
            processingObject.EguValue = NDSConfiguration.GetScalingFactor(processingObject.PointType) * processingObject.RawValue + NDSConfiguration.GetDeviation(processingObject.PointType);
        }

        public INDSConfiguration NDSConfiguration
        {
            get { return new ConfigReader(); }
        }
    }
}
