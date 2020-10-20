using Common;
using Common.GDA;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using ScadaCommon;
using ScadaCommon.BackEnd_FrontEnd;
using ScadaCommon.Interfaces;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeCacheService
{
    public class SCADARealTimeCache : IRealTimeCacheService
    {
        private IReliableDictionary<long, BasePointCacheItem> ndsModel;
        private IReliableDictionary<long, BasePointCacheItem> ndsModelNew;
        private IFEPConfigService fepConfigServiceProxy;
        IReliableStateManager stateManager;
        private bool modelUpdate = false;
        private Delta model;

        public SCADARealTimeCache(IFEPConfigService fepConfigServiceProxy, IReliableStateManager stateManager)
        {
            this.fepConfigServiceProxy = fepConfigServiceProxy;
            this.stateManager = stateManager;

            
            //Task.Run(async () => await Initialize()).Wait();
        }

        private async Task Initialize()
        {
            ndsModel = await this.stateManager.GetOrAddAsync<IReliableDictionary<long, BasePointCacheItem>>("ndsModel");
            ndsModelNew = await this.stateManager.GetOrAddAsync<IReliableDictionary<long, BasePointCacheItem>>("ndsModelNew");
        }
        private void Add(long key, BasePointCacheItem point)
        {

            Task.Run(async () =>
            {
                ndsModelNew = await this.stateManager.GetOrAddAsync<IReliableDictionary<long, BasePointCacheItem>>("ndsModelNew");

                using (var tx = this.stateManager.CreateTransaction())
                {
                    var result = await ndsModelNew.TryAddAsync(tx, key, point);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

            }).Wait();
        }

        private BasePointCacheItem GetItem(long key)
        {
            BasePointCacheItem retVal = null;

            Task.Run(async () =>
            {
                ndsModelNew = await this.stateManager.GetOrAddAsync<IReliableDictionary<long, BasePointCacheItem>>("ndsModelNew");

                using (var tx = this.stateManager.CreateTransaction())
                {
                    var result = await ndsModelNew.TryGetValueAsync(tx, key);
                    retVal = result.Value;
                }

            }).Wait();

            return retVal;
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
                    Add(item.Id, digital);
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
                    Add(item.Id, analog);
                }
            }

            this.fepConfigServiceProxy.SendConfiguration(ModelNewToList());
        }

        //prilikom commit-a preuzmi novi model i postavi za vazeci
        public async void ApplyUpdate()
        {
            //this.ndsModel = this.ndsModelNew;
            await CopyDataModel();
        }

        public bool TryGetBasePointItem(long gid, out BasePointCacheItem basePointCacheItem)
        {
            basePointCacheItem = GetItem(gid);
            return true; //ndsModel.TryGetValue(gid, out basePointCacheItem);
        }

        public void StoreDelta(Delta delta)
        {
            this.modelUpdate = true;
            this.model = delta;
            InitializePointCache(delta);
            this.modelUpdate = false;
        }

        private List<BasePointCacheItem> ModelNewToList()
        {
            List<BasePointCacheItem> retVal = new List<BasePointCacheItem>();

            Task.Run(async () =>
            {
                ndsModelNew = await this.stateManager.GetOrAddAsync<IReliableDictionary<long, BasePointCacheItem>>("ndsModelNew");

                using (var tx = this.stateManager.CreateTransaction())
                {
                    var enumerable = await ndsModelNew.CreateEnumerableAsync(tx);
                    var asyncEnumerator = enumerable.GetAsyncEnumerator();

                    while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<long, BasePointCacheItem> item = asyncEnumerator.Current;
                        retVal.Add(item.Value);
                    }

                    await tx.CommitAsync();
                }
            }).Wait();

            return retVal;
        }

        private async Task CopyDataModel()
        {
            await Initialize();

            using (var tx = this.stateManager.CreateTransaction())
            {
                var enumerable = await ndsModelNew.CreateEnumerableAsync(tx);
                var asyncEnumerator = enumerable.GetAsyncEnumerator();

                while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                {
                    KeyValuePair<long, BasePointCacheItem> item = asyncEnumerator.Current;
                    await ndsModel.AddAsync(tx, item.Key, item.Value);
                }

                await tx.CommitAsync();
            }
        }
    }
}
