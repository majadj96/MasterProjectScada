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
    public class AlarmingModule : IProcessingData
    {
        public void Process(ProcessingObject[] processingObject)
        {
            foreach (var item in processingObject)
            {
                if (item.PointType == PointType.ANALOG_INPUT_16 || item.PointType == PointType.ANALOG_OUTPUT_16)
                {
                    if (!CheckReasonability(((AnalogPoint)item).EguValue, NDSConfiguration.GetEguMin(item.PointType), NDSConfiguration.GetEguMax(item.PointType)))
                    {
                        item.InAlarm = true;
                        //alarm izvan mernih opsega
                    }
                    else if (((AnalogPoint)item).EguValue < NDSConfiguration.GetLowLimit(item.PointType))
                    {
                        item.InAlarm = true;
                        //low alarm
                    }
                    else if (((AnalogPoint)item).EguValue > NDSConfiguration.GetHighLimit(item.PointType))
                    {
                        item.InAlarm = true;
                        //high alarm
                    }
                }
                else
                {
                    if (NDSConfiguration.GetNormalValue(PointType.BINARY_INPUT) != item.RawValue)
                    {
                        item.InAlarm = true;
                        //alarm treba da se napravi (ABNORMAL ALARM)
                    }
                }
            }
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
