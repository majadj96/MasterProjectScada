using DNP3;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.Interfaces;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FrontEndProcessorService.Simulator
{
    public class SimulatorCommandingService
    {
        private Dictionary<Tuple<ushort, PointType>, BasePointCacheItem> model;
        private IFunctionExecutor functionExecutor;
        private List<DigitalPointCacheItem> substationOne = new List<DigitalPointCacheItem>();
        private List<DigitalPointCacheItem> substationTwo = new List<DigitalPointCacheItem>();
        AnalogPointCacheItem tank1 = null;
        AnalogPointCacheItem tank2 = null;

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

            FillLists();

            if (pointCacheItem.MeasurementType == Common.MeasurementType.SwitchStatus)
            {
                if (String.Compare(pointCacheItem.MrId, "Breaker_1SwitchStatus") == 0 || String.Compare(pointCacheItem.MrId, "Breaker_Pump1") == 0 ||
                   String.Compare(pointCacheItem.MrId, "Discrete_Disc1") == 0 || String.Compare(pointCacheItem.MrId, "Discrete_Disc2") == 0)
                {
                    SetCurrentAndVoltage();
                }
                else
                {
                    SetCurrentAndVoltageSubTwo();
                }
            }


            if (tank1 == null)
            {
                try
                {
                    tank1 = (AnalogPointCacheItem)model.Values.Where(x => x.MrId == "FluidLevel_Tank1").ToList()[0];
                }
                catch (Exception) { }

                if (tank1 != null)
                {
                    if (tank1.RawValue > 90 && substationOne.TrueForAll(x => x.State == DState.ON))
                    {
                        Thread t = new Thread(EmptyTank1);
                        t.Start();
                    }
                    else
                        tank1 = null;
                }
            }


            if (tank2 == null)
            {
                try
                {
                    tank2 = (AnalogPointCacheItem)model.Values.Where(x => x.MrId == "FluidLevel_Tank2").ToList()[0];
                }
                catch (Exception) { }

                if (tank2 != null)
                {
                    if (tank2.RawValue > 90 && substationTwo.TrueForAll(x => x.State == DState.ON))
                    {
                        Thread t = new Thread(EmptyTank2);
                        t.Start();
                    }
                    else
                        tank2 = null;
                }
            }
        }

        private void FillLists()
        {
            if (substationOne.Count == 0)
            {
                foreach (var c in model.Values.Where(x => x.Type == PointType.BINARY_OUTPUT).ToList())
                {
                    if (String.Compare(c.MrId, "Breaker_1SwitchStatus") == 0 ||
                       String.Compare(c.MrId, "Breaker_Pump1") == 0 ||
                       String.Compare(c.MrId, "Discrete_Disc1") == 0 ||
                       String.Compare(c.MrId, "Discrete_Disc2") == 0)
                    {
                        if(!substationOne.Contains((DigitalPointCacheItem)c))
                            substationOne.Add((DigitalPointCacheItem)c);
                    }
                }
            }

            if (substationTwo.Count == 0)
            {
                foreach (var c in model.Values.Where(x => x.Type == PointType.BINARY_OUTPUT).ToList())
                {
                    if (String.Compare(c.MrId, "Breaker_2SwitchStatus") == 0 ||
                       String.Compare(c.MrId, "Breaker_Pump2") == 0 ||
                       String.Compare(c.MrId, "Breaker_Pump3") == 0 ||
                       String.Compare(c.MrId, "Discrete_Disc3") == 0 ||
                       String.Compare(c.MrId, "Discrete_Disc4") == 0)
                    {
                        if (!substationTwo.Contains((DigitalPointCacheItem)c))
                            substationTwo.Add((DigitalPointCacheItem)c);
                    }
                }
            }
        }

        private void SetCurrentAndVoltage()
        {
            if (substationOne.TrueForAll(x => x.State == DState.ON))
            {
                foreach (var v in model.Values)
                {
                    if (String.Compare(v.MrId, "PT1Current_W1") == 0)
                        SendMessage(v.Address, 5);
                    if (String.Compare(v.MrId, "PT1Current_W2") == 0)
                        SendMessage(v.Address, 5);

                    if (String.Compare(v.MrId, "PT1Voltage_W1") == 0)
                        SendMessage(v.Address, 220);
                    if (String.Compare(v.MrId, "PT1Voltage_W2") == 0)
                    {
                        SendMessage(v.Address, 220);
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

        private void SetCurrentAndVoltageSubTwo()
        {
            if (substationTwo.TrueForAll(x => x.State == DState.ON))
            {
                foreach (var v in model.Values)
                {
                    if (String.Compare(v.MrId, "PT2Current_W1") == 0)
                        SendMessage(v.Address, 5);
                    if (String.Compare(v.MrId, "PT2Current_W2") == 0)
                        SendMessage(v.Address, 5);

                    if (String.Compare(v.MrId, "PT2Voltage_W1") == 0)
                        SendMessage(v.Address, 220);
                    if (String.Compare(v.MrId, "PT2Voltage_W2") == 0)
                    {
                        SendMessage(v.Address, 220);
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
        
        private void EmptyTank1()
        {
            if(tank1 != null)
            {
                float value = tank1.RawValue;

                while (value >= tank1.NormalValue)
                {
                    value -= 2;

                    SendMessage(tank1.Address, value);

                    Thread.Sleep(2000);
                }
            }
            tank1 = null;
        }

        private void EmptyTank2()
        {
            if (tank2 != null)
            {
                float value = tank2.RawValue;

                while (value >= tank2.NormalValue)
                {
                    value -= 2;

                    SendMessage(tank2.Address, value);

                    Thread.Sleep(2000);
                }
                tank2 = null;
            }
        }

        private void SendMessage(ushort address, float value)
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.DIRECT_OPERATE, (ushort)TypeField.ANALOG_OUTPUT_16BIT, 0x28, 0x0001, address, (uint)value, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);

            functionExecutor.SendDirectMessage(fn);
        }
    }
}
