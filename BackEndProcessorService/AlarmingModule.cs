using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEndProcessorService.Configuration;
using BackEndProcessorService.Proxy;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Database;
using ScadaCommon.Interfaces;

namespace BackEndProcessorService
{
    public class AlarmingModule : IProcessingData
    {
        private AlarmEventServiceProxy alarmEventServiceProxy;

        public AlarmingModule()
        {
            alarmEventServiceProxy = new AlarmEventServiceProxy("AlarmEventServiceEndPoint");
            alarmEventServiceProxy.Open();
        }

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
                        //dodati u ProcessingObject sta jos ima u Alarmu TODO
                        Alarm alarm = new Alarm() { Message = "Value is outside boundaries." };
                        alarmEventServiceProxy.AddAlarm(alarm);
                    }
                    else if (((AnalogPoint)item).EguValue < NDSConfiguration.GetLowLimit(item.PointType))
                    {
                        item.InAlarm = true;
                        //low alarm
                        //dodati u ProcessingObject sta jos ima u Alarmu TODO
                        Alarm alarm = new Alarm() { Message = "Point is in low alarm state." };
                        alarmEventServiceProxy.AddAlarm(alarm);
                    }
                    else if (((AnalogPoint)item).EguValue > NDSConfiguration.GetHighLimit(item.PointType))
                    {
                        item.InAlarm = true;
                        //high alarm
                        //dodati u ProcessingObject sta jos ima u Alarmu TODO
                        Alarm alarm = new Alarm() { Message = "Point is in high alarm state." };
                        alarmEventServiceProxy.AddAlarm(alarm);
                    }
                }
                else
                {
                    if (NDSConfiguration.GetNormalValue(PointType.BINARY_INPUT) != item.RawValue)
                    {
                        item.InAlarm = true;
                        //alarm treba da se napravi (ABNORMAL ALARM)
                        //dodati u ProcessingObject sta jos ima u Alarmu TODO
                        Alarm alarm = new Alarm() { Message = "Point is in abnormal alarm state." };
                        alarmEventServiceProxy.AddAlarm(alarm);
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
