using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEndProcessorService.Configuration;
using BackEndProcessorService.Proxy;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using Common.AlarmEvent;
using ScadaCommon.Interfaces;

namespace BackEndProcessorService
{
    public class AlarmingModule : IProcessingData
    {
        private AlarmEventServiceProxy alarmEventServiceProxy;

        public AlarmingModule(AlarmEventServiceProxy alarmEventServiceProxy)
        {
            this.alarmEventServiceProxy = alarmEventServiceProxy;
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
                        Alarm alarm = new Alarm()
                        {
                            AlarmAcknowledged = null,
                            AlarmReported = DateTime.Now,
                            AlarmReportedBy = Common.AlarmEventType.SCADA,
                            PointName = item.PointType.ToString(),
                            GiD = item.Gid,
                            Username = String.Empty,
                            Message = "Value is in UNREASONABLE state.",
                            AbnormalIndicator = true,
                        };
                        alarmEventServiceProxy.AddAlarm(alarm);
                    }
                    else if (((AnalogPoint)item).EguValue < ((AnalogPoint)item).MinValue)
                    {
                        item.InAlarm = true;
                        //low alarm
                        //dodati u ProcessingObject sta jos ima u Alarmu TODO
                        Alarm alarm = new Alarm()
                        {
                            AlarmAcknowledged = null,
                            AlarmReported = DateTime.Now,
                            AlarmReportedBy = Common.AlarmEventType.SCADA,
                            PointName = item.PointType.ToString(),
                            GiD = item.Gid,
                            Username = String.Empty,
                            Message = "Point is in LOW alarm state.",
                            AbnormalIndicator = true,
                    };
                        alarmEventServiceProxy.AddAlarm(alarm);
                    }
                    else if (((AnalogPoint)item).EguValue > ((AnalogPoint)item).MaxValue)
                    {
                        item.InAlarm = true;
                        //high alarm
                        //dodati u ProcessingObject sta jos ima u Alarmu TODO
                        Alarm alarm = new Alarm()
                        {
                            AlarmAcknowledged = null,
                            AlarmReported = DateTime.Now,
                            AlarmReportedBy = Common.AlarmEventType.SCADA,
                            PointName = item.PointType.ToString(),
                            GiD = item.Gid,
                            Username = String.Empty,
                            Message = "Point is in HIGH alarm state.",
                            AbnormalIndicator = true,
                        };
                        alarmEventServiceProxy.AddAlarm(alarm);
                    }
                    else
                    {
                        item.InAlarm = false;
                        //Tacka je vracena u normalan opseg vrednosti, prijavljuje se alarm za normal.
                        Alarm alarm = new Alarm()
                        {
                            AlarmAcknowledged = null,
                            AlarmReported = DateTime.Now,
                            AlarmReportedBy = Common.AlarmEventType.SCADA,
                            PointName = item.PointType.ToString(),
                            GiD = item.Gid,
                            Username = String.Empty,
                            Message = "Point is in NORMAL alarm state.",
                            AbnormalIndicator = false,
                        };
                        alarmEventServiceProxy.AddAlarm(alarm);
                    }
                }
                else
                {
                    if (((DigitalPoint)item).NormalValue != item.RawValue)
                    {
                        item.InAlarm = true;
                        //alarm treba da se napravi (ABNORMAL ALARM)
                        //dodati u ProcessingObject sta jos ima u Alarmu TODO
                        Alarm alarm = new Alarm()
                        {
                            AlarmAcknowledged = null,
                            AlarmReported = DateTime.Now,
                            AlarmReportedBy = Common.AlarmEventType.SCADA,
                            PointName = item.PointType.ToString(),
                            GiD = item.Gid,
                            Username = String.Empty,
                            Message = "Point is in ABNORMAL alarm state.",
                            AbnormalIndicator = true,
                        };
                        alarmEventServiceProxy.AddAlarm(alarm);
                    }
                    else
                    {
                        item.InAlarm = false;
                        //Tacka se vraca u NORMAL stanje, prijavljuje se normal alarm...
                        Alarm alarm = new Alarm()
                        {
                            AlarmAcknowledged = null,
                            AlarmReported = DateTime.Now,
                            AlarmReportedBy = Common.AlarmEventType.SCADA,
                            PointName = item.PointType.ToString(),
                            GiD = item.Gid,
                            Username = String.Empty,
                            Message = "Point is in NORMAL alarm state.",
                            AbnormalIndicator = false,
                        };
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
