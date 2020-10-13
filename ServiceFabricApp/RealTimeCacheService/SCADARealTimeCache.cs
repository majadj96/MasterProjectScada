using Common;
using Common.GDA;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeCacheService
{
    public class SCADARealTimeCache : IRealTimeCacheService
    {
        private Dictionary<long, BasePointCacheItem> ndsModel = new Dictionary<long, BasePointCacheItem>();
        private Dictionary<long, BasePointCacheItem> ndsModelNew = new Dictionary<long, BasePointCacheItem>();
        private IFEPConfigService fepConfigServiceProxy;
        private bool modelUpdate = false;
        private Delta model;

        public SCADARealTimeCache(IFEPConfigService fepConfigServiceProxy)
        {
            this.fepConfigServiceProxy = fepConfigServiceProxy;
        }

        public bool ModelUpdate
        {
            get
            {
                return modelUpdate;
            }
            set
            {
                modelUpdate = value;
            }
        }

        private void InitializePointCache(Delta inputPoints)
        {
            int digitalInputAdress = 0;
            int digitalOutputAdress = 0;
            int analogInputAdress = 0;
            int analogOutputAdress = 0;
            int adress = 0;

            string description = String.Empty;
            string mrId = String.Empty;
            string name = String.Empty;
            PointType pointType = PointType.BINARY_INPUT;
            MeasurementType measurementType = MeasurementType.Unitless;
            int minDiscrete = 0;
            int maxDiscrete = 0;
            int normalDiscrete = 0;
            float minAnalog = 0;
            float maxAnalog = 0;
            float normalAnalog = 0;

            foreach (ResourceDescription item in inputPoints.InsertOperations)
            {
                if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(item.Id)) == DMSType.DISCRETE)
                {
                    foreach (Property item2 in item.Properties)
                    {
                        switch (item2.Id)
                        {
                            case ModelCode.IDOBJ_DESC:
                                description = item2.ToString();
                                break;
                            case ModelCode.IDOBJ_MRID:
                                mrId = item2.ToString();
                                break;
                            case ModelCode.IDOBJ_NAME:
                                name = item2.ToString();
                                break;
                            case ModelCode.MEASUREMENT_DIRECTION:
                                if ((SignalDirection)(long)item2.GetValue() == SignalDirection.Read)
                                {
                                    pointType = PointType.BINARY_INPUT;
                                    adress = digitalInputAdress;
                                    digitalInputAdress++;
                                }
                                else if ((SignalDirection)(long)item2.GetValue() == SignalDirection.ReadWrite)
                                {
                                    pointType = PointType.BINARY_OUTPUT;
                                    adress = digitalOutputAdress;
                                    digitalOutputAdress++;
                                }
                                break;
                            case ModelCode.MEASUREMENT_MEASTYPE:
                                measurementType = (MeasurementType)(long)item2.GetValue();
                                break;
                            case ModelCode.DISCRETE_MINVALUE:
                                minDiscrete = (int)(long)item2.GetValue();
                                break;
                            case ModelCode.DISCRETE_MAXVALUE:
                                maxDiscrete = (int)(long)item2.GetValue();
                                break;
                            case ModelCode.DISCRETE_NORMALVALUE:
                                normalDiscrete = (int)(long)item2.GetValue();
                                break;
                            default:
                                break;
                        }
                    }
                    BasePointCacheItem digital = new DigitalPointCacheItem()
                    {
                        Gid = item.Id,
                        Description = description,
                        MrId = mrId,
                        Name = name,
                        Type = pointType,
                        MeasurementType = measurementType,
                        MinValue = minDiscrete,
                        MaxValue = maxDiscrete,
                        NormalValue = normalDiscrete,
                        Address = (ushort)adress
                    };
                    ndsModelNew.Add(item.Id, digital);
                }
                else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(item.Id)) == DMSType.ANALOG)
                {
                    foreach (Property item2 in item.Properties)
                    {
                        switch (item2.Id)
                        {
                            case ModelCode.IDOBJ_DESC:
                                description = item2.ToString();
                                break;
                            case ModelCode.IDOBJ_MRID:
                                mrId = item2.ToString();
                                break;
                            case ModelCode.IDOBJ_NAME:
                                name = item2.ToString();
                                break;
                            case ModelCode.MEASUREMENT_DIRECTION:
                                if ((SignalDirection)(long)item2.GetValue() == SignalDirection.Read)
                                {
                                    pointType = PointType.ANALOG_INPUT;
                                    adress = analogInputAdress;
                                    analogInputAdress++;
                                }
                                else if ((SignalDirection)(long)item2.GetValue() == SignalDirection.ReadWrite)
                                {
                                    pointType = PointType.ANALOG_OUTPUT;
                                    adress = analogOutputAdress;
                                    analogOutputAdress++;
                                }
                                break;
                            case ModelCode.MEASUREMENT_MEASTYPE:
                                measurementType = (MeasurementType)(long)item2.GetValue();
                                break;
                            case ModelCode.ANALOG_MINVALUE:
                                minAnalog = (float)item2.GetValue();
                                break;
                            case ModelCode.ANALOG_MAXVALUE:
                                maxAnalog = (float)item2.GetValue();
                                break;
                            case ModelCode.ANALOG_NORMALVALUE:
                                normalAnalog = (float)item2.GetValue();
                                break;
                            default:
                                break;
                        }
                    }
                    BasePointCacheItem analog = new AnalogPointCacheItem()
                    {
                        Gid = item.Id,
                        Description = description,
                        MrId = mrId,
                        Name = name,
                        Type = pointType,
                        MeasurementType = measurementType,
                        MinValue = minAnalog,
                        MaxValue = maxAnalog,
                        NormalValue = normalAnalog,
                        Address = (ushort)adress
                    };
                    ndsModelNew.Add(item.Id, analog);
                }
            }

            this.fepConfigServiceProxy.SendConfiguration(this.ndsModelNew.Values.ToList());
        }

        //prilikom commit-a preuzmi novi model i postavi za vazeci
        public void ApplyUpdate()
        {
            this.ndsModel = this.ndsModelNew;
        }

        public bool TryGetBasePointItem(long gid, out BasePointCacheItem basePointCacheItem)
        {
            return ndsModel.TryGetValue(gid, out basePointCacheItem);
        }

        public void StoreDelta(Delta delta)
        {
            this.modelUpdate = true;
            this.model = delta;
            InitializePointCache(delta);
            this.modelUpdate = false;
        }
    }
}
