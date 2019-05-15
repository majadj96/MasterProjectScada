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
    public class AlarmingModule : IProcessingData
    {
        public void Process(IProcessingObject processingObject)
        {
            //if (!CheckReasonability(processingObject.EguValue, NDSConfiguration.GetEguMin(processingObject.PointType), NDSConfiguration.GetEguMax(processingObject.PointType)))
            //{
            //    //alarm izvan mernih opsega
            //}
            //else if(processingObject.EguValue < NDSConfiguration.GetLowLimit(processingObject.PointType))
            //{
            //    //low alarm
            //}else if(processingObject.EguValue > NDSConfiguration.GetHighLimit(processingObject.PointType))
            //{
            //    //high alarm
            //}
        }

        private bool CheckReasonability(double eguValue, uint eguMin, uint eguMax)
        {
            if (eguValue < eguMin || eguValue > eguMax)
            {
                return false;
            }
            return true;
        }

        public INDSConfiguration NDSConfiguration
        {
            get { return new ConfigReader(); }
        }
    }
}
