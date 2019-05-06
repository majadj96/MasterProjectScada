using DNP3;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.Interfaces;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontEndProcessorService.Simulator
{
    public class SimulatorCommandingService
    {
        private Dictionary<Tuple<ushort, PointType>, BasePointCacheItem> model;
        private IFunctionExecutor functionExecutor;

        public SimulatorCommandingService(IFunctionExecutor functionExecutor)
        {
            this.functionExecutor = functionExecutor;
            this.model = new Dictionary<Tuple<ushort, PointType>, BasePointCacheItem>();
        }

        public void UpdatePoints(Tuple<ushort, PointType> tuple, BasePointCacheItem pointCacheItem)
        {
            if (model.ContainsKey(tuple))
                model[tuple] = pointCacheItem;
            else
                model.Add(tuple, pointCacheItem);

            if (pointCacheItem.MeasurementType == Common.MeasurementType.SwitchStatus)
            {
                if (String.Compare(pointCacheItem.MrId, "Breaker_1SwitchStatus") == 0 || String.Compare(pointCacheItem.MrId, "Breaker_Pump1") == 0 ||
                   String.Compare(pointCacheItem.MrId, "Discrete_Disc1") == 0 || String.Compare(pointCacheItem.MrId, "Discrete_Disc2") == 0 )
                {
                    SetCurrentAndVoltage();
                }
                else
                {
                    SetCurrentAndVoltageSubTwo();
                }
            }
        }

        public void SetCurrentAndVoltage()
        {
            List<DigitalPointCacheItem> lista = new List<DigitalPointCacheItem>();

            foreach(var c in model.Values.Where(x => x.Type == PointType.BINARY_OUTPUT).ToList())
            {
                if (String.Compare(c.MrId, "Breaker_1SwitchStatus") == 0 ||
                   String.Compare(c.MrId, "Breaker_Pump1") == 0 ||
                   String.Compare(c.MrId, "Discrete_Disc1") == 0 ||
                   String.Compare(c.MrId, "Discrete_Disc2") == 0)
                    lista.Add((DigitalPointCacheItem)c);
            }

            if (lista.TrueForAll(x => x.State == DState.ON))
            {
                foreach (var v in model.Values)
                {
                    v.RawValue = 5;
                    v.NormalValue = 5;
                    if (String.Compare(v.MrId, "PT1Current_W1") == 0)
                        SendCurrentVoltageMessage(v);
                    if (String.Compare(v.MrId, "PT1Current_W2") == 0)
                        SendCurrentVoltageMessage(v);

                    v.RawValue = 220;
                    v.NormalValue = 220;
                    if (String.Compare(v.MrId, "PT1Voltage_W1") == 0)
                        SendCurrentVoltageMessage(v);
                    if (String.Compare(v.MrId, "PT1Voltage_W2") == 0)
                    {
                        SendCurrentVoltageMessage(v);
                        break;
                    }
                }
            }
            /*else if (lista.Where(x => x.MrId == "Discrete_Disc1").ToList()[0].State == DState.OFF)
            {
                foreach (var v in model.Values)
                {
                    v.RawValue = 0;
                    v.NormalValue = 0;
                    if (String.Compare(v.MrId, "PT1Current_W1") == 0)
                        SendCurrentVoltageMessage(v);
                    if (String.Compare(v.MrId, "PT1Current_W2") == 0)
                        SendCurrentVoltageMessage(v);

                    v.RawValue = 0;
                    v.NormalValue = 0;
                    if (String.Compare(v.MrId, "PT1Voltage_W1") == 0)
                        SendCurrentVoltageMessage(v);
                    if (String.Compare(v.MrId, "PT1Voltage_W2") == 0)
                    {
                        SendCurrentVoltageMessage(v);
                        break;
                    }
                }
            }*/
        }

        public void SetCurrentAndVoltageSubTwo()
        {
            List<DigitalPointCacheItem> lista = new List<DigitalPointCacheItem>();

            foreach (var c in model.Values.Where(x => x.Type == PointType.BINARY_OUTPUT).ToList())
            {
                if (String.Compare(c.MrId, "Breaker_2SwitchStatus") == 0 ||
                   String.Compare(c.MrId, "Breaker_Pump2") == 0 ||
                   String.Compare(c.MrId, "Breaker_Pump3") == 0 ||
                   String.Compare(c.MrId, "Discrete_Disc3") == 0 ||
                   String.Compare(c.MrId, "Discrete_Disc4") == 0)
                    lista.Add((DigitalPointCacheItem)c);
            }

            if (lista.TrueForAll(x => x.State == DState.ON))
            {
                foreach (var v in model.Values)
                {
                    v.RawValue = 5;
                    v.NormalValue = 5;
                    if (String.Compare(v.MrId, "PT2Current_W1") == 0)
                        SendCurrentVoltageMessage(v);
                    if (String.Compare(v.MrId, "PT2Current_W2") == 0)
                        SendCurrentVoltageMessage(v);

                    v.RawValue = 220;
                    v.NormalValue = 220;
                    if (String.Compare(v.MrId, "PT2Voltage_W1") == 0)
                        SendCurrentVoltageMessage(v);
                    if (String.Compare(v.MrId, "PT2Voltage_W2") == 0)
                    {
                        SendCurrentVoltageMessage(v);
                        break;
                    }
                }
            }
            /*else if (lista.Where(x => x.MrId == "Discrete_Disc3").ToList()[0].State == DState.OFF)
            {
                foreach (var v in model.Values)
                {
                    v.RawValue = 0;
                    v.NormalValue = 0;
                    if (String.Compare(v.MrId, "PT2Current_W1") == 0)
                        SendCurrentVoltageMessage(v);
                    if (String.Compare(v.MrId, "PT2Current_W2") == 0)
                        SendCurrentVoltageMessage(v);

                    v.RawValue = 0;
                    v.NormalValue = 0;
                    if (String.Compare(v.MrId, "PT2Voltage_W1") == 0)
                        SendCurrentVoltageMessage(v);
                    if (String.Compare(v.MrId, "PT2Voltage_W2") == 0)
                    {
                        SendCurrentVoltageMessage(v);
                        break;
                    }
                }
            }*/
        }

        public void SendCurrentVoltageMessage(BasePointCacheItem item)
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.DIRECT_OPERATE, (ushort)TypeField.ANALOG_OUTPUT_16BIT, 0x28, 0x0001, item.Address, (uint)item.RawValue, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);

            functionExecutor.SendDirectMessage(fn);
        }
    }
}
