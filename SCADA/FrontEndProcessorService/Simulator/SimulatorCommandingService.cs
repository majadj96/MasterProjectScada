using DNP3;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.Interfaces;
using ScadaCommon.NDSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;

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
        Thread Tank1SimulatorThread;
        Thread Tank2SimulatorThread;

        private System.Timers.Timer timer;
        private bool pointUpdate = false;
        const float nominalPump1Flow = 4;
        const float nominalPump2Flow = 4;
        const float nominalPump3Flow = 5;
        private float pump1FluidFlow = 4;
        private float pump2FluidFlow = 4;
        private float pump3FluidFlow = 5;
        private bool Pump1Running = false;
        private bool Pump2Running = false;
        private bool Pump3Running = false;

        public SimulatorCommandingService(IFunctionExecutor functionExecutor)
        {
            this.functionExecutor = functionExecutor;
            this.model = new Dictionary<Tuple<ushort, PointType>, BasePointCacheItem>();

            Tank1SimulatorThread = new Thread(EmptyTank1);
            Tank2SimulatorThread = new Thread(EmptyTank2);

            StartTimer();
        }

        private void StartTimer()
        {
            timer = new System.Timers.Timer(3000);
            timer.Elapsed += SimulateValues_OnTimer;
            timer.AutoReset = true;
            timer.Start();
        }

        private void SimulateValues_OnTimer(object sender, ElapsedEventArgs e)
        {
            if (!pointUpdate || model.Count <= 0)
                return;

            pointUpdate = false;

            BasePointCacheItem pump1power = null;
            BasePointCacheItem pump2power = null;
            BasePointCacheItem pump3power = null;
            BasePointCacheItem pump1speed = null;
            BasePointCacheItem pump2speed = null;
            BasePointCacheItem pump3speed = null;
            BasePointCacheItem pump1current = null;
            BasePointCacheItem pump1voltage = null;
            BasePointCacheItem pump2current = null;
            BasePointCacheItem pump2voltage = null;

            foreach (BasePointCacheItem item in model.Values)
            {
                if (item.MrId.Equals("Pump1Power"))
                    pump1power = item;
                else if (item.MrId.Equals("Pump2Power"))
                    pump2power = item;
                else if (item.MrId.Equals("Pump3Power"))
                    pump3power = item;
                else if (item.MrId.Equals("Pump1Speed"))
                    pump1speed = item;
                else if (item.MrId.Equals("Pump2Speed"))
                    pump2speed = item;
                else if (item.MrId.Equals("Pump3Speed"))
                    pump3speed = item;
                else if (item.MrId.Equals("PT1Current_W2"))
                    pump1current = item;
                else if (item.MrId.Equals("PT1Voltage_W2"))
                    pump1voltage = item;
                else if (item.MrId.Equals("PT2Current_W2"))
                    pump2current = item;
                else if (item.MrId.Equals("PT2Voltage_W2"))
                    pump2voltage = item;
            }

            if (substationOne.Count == 4 && substationOne.TrueForAll(x => x.State == DState.ON))
            {
                Pump1Running = true;

                if (pump1power.RawValue != pump1current.RawValue * pump1voltage.RawValue)
                {
                    SendMessage(pump1power.Address, pump1current.RawValue * pump1voltage.RawValue);
                    SendMessage(pump1speed.Address, 1450);
                }
            }
            else
            {
                Pump1Running = false;

                if (pump1power.RawValue != 0)
                {
                    SendMessage(pump1power.Address, 0);
                    SendMessage(pump1speed.Address, 0);
                }
            }
            if (substationTwo.Count == 5 && substationTwo.Where(s => s.MrId != "Breaker_Pump3").ToList().TrueForAll(x => x.State == DState.ON))
            {
                Pump2Running = true;

                if (pump2power.RawValue != pump2current.RawValue * pump2voltage.RawValue)
                {
                    SendMessage(pump2power.Address, pump2current.RawValue * pump2voltage.RawValue);
                    SendMessage(pump2speed.Address, 1450);
                }
            }
            else
            {
                Pump2Running = false;

                if (pump2power.RawValue != 0)
                {
                    SendMessage(pump2power.Address, 0);
                    SendMessage(pump2speed.Address, 0);
                }
            }
            if (substationTwo.Count == 5 && substationTwo.Where(s => s.MrId != "Breaker_Pump2").ToList().TrueForAll(x => x.State == DState.ON))
            {
                Pump3Running = true;

                if (pump3power.RawValue != pump2current.RawValue * pump2voltage.RawValue)
                {
                    SendMessage(pump3power.Address, pump2current.RawValue * pump2voltage.RawValue);
                    SendMessage(pump3speed.Address, 1450);
                }
            }
            else
            {
                Pump3Running = false;

                if (pump3power.RawValue != 0)
                {
                    SendMessage(pump3power.Address, 0);
                    SendMessage(pump3speed.Address, 0);
                }
            }

            pump1FluidFlow = CalculateFluidFlow(nominalPump1Flow, pump1power.RawValue);
            pump2FluidFlow = CalculateFluidFlow(nominalPump2Flow, pump2power.RawValue);
            pump3FluidFlow = CalculateFluidFlow(nominalPump3Flow, pump3power.RawValue);

            UpdateFluidLevel();
        }

        public void UpdatePoints(Tuple<ushort, PointType> tuple, BasePointCacheItem pointCacheItem)
        {
            if (model.ContainsKey(tuple))
            {
                model[tuple] = pointCacheItem;
                pointUpdate = true;
            }
            else
            {
                model.Add(tuple, pointCacheItem);
            }

            FillLists(pointCacheItem);

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

            if(pointCacheItem.MrId == "Analog_TapChanger1")
            {
                SetCurrentAndVoltage();
            }
            else if (pointCacheItem.MrId == "Analog_TapChanger2")
            {
                SetCurrentAndVoltageSubTwo();
            }   
        }

        private void UpdateFluidLevel()
        {
            tank1 = (AnalogPointCacheItem)model.Values.FirstOrDefault(x => x.MrId == "FluidLevel_Tank1");

            if (Pump1Running)
            {
                if (!Tank1SimulatorThread.IsAlive)
                {
                    Tank1SimulatorThread = new Thread(EmptyTank1);
                    Tank1SimulatorThread.Start();
                }
            }
            else
            {
                if (Tank1SimulatorThread.IsAlive)
                {
                    Tank1SimulatorThread.Abort();
                }
            }

            tank2 = (AnalogPointCacheItem)model.Values.FirstOrDefault(x => x.MrId == "FluidLevel_Tank2");

            if (Pump2Running || Pump3Running)
            {
                if (!Tank2SimulatorThread.IsAlive)
                {
                    Tank2SimulatorThread = new Thread(EmptyTank2);
                    Tank2SimulatorThread.Start();
                }
            }
            else
            {
                if (Tank2SimulatorThread.IsAlive)
                {
                    Tank2SimulatorThread.Abort();
                }
            }
        }

        private void FillLists(BasePointCacheItem pointCacheItem)
        {
            if (String.Compare(pointCacheItem.MrId, "Breaker_1SwitchStatus") == 0 ||
                   String.Compare(pointCacheItem.MrId, "Breaker_Pump1") == 0 ||
                   String.Compare(pointCacheItem.MrId, "Discrete_Disc1") == 0 ||
                   String.Compare(pointCacheItem.MrId, "Discrete_Disc2") == 0)
            {
                if (!substationOne.Contains((DigitalPointCacheItem)pointCacheItem))
                {
                    substationOne.Add((DigitalPointCacheItem)pointCacheItem);
                }
            }
            else if (String.Compare(pointCacheItem.MrId, "Breaker_2SwitchStatus") == 0 ||
                       String.Compare(pointCacheItem.MrId, "Breaker_Pump2") == 0 ||
                       String.Compare(pointCacheItem.MrId, "Breaker_Pump3") == 0 ||
                       String.Compare(pointCacheItem.MrId, "Discrete_Disc3") == 0 ||
                       String.Compare(pointCacheItem.MrId, "Discrete_Disc4") == 0)
            {
                if (!substationTwo.Contains((DigitalPointCacheItem)pointCacheItem))
                {
                    substationTwo.Add((DigitalPointCacheItem)pointCacheItem);
                }
            }
        }

        private void SetCurrentAndVoltage()
        {
            BasePointCacheItem tapChanger = model.Values.FirstOrDefault(x => x.MrId == "Analog_TapChanger1");

            if(tapChanger != null)
            {
                if (substationOne.Where(x => x.MrId != "Breaker_Pump1").ToList().TrueForAll(x => x.State == DState.ON))
                {
                    SendMessageWithCheck("PT1Current_W1", 5);
                    SendMessageWithCheck("PT1Voltage_W1", 220);
                    float cur = GetTransformedCurrent(tapChanger.RawValue);
                    SendMessageWithCheck("PT1Current_W2", cur);
                    float vol = GetTransformedVoltage(tapChanger.RawValue);
                    SendMessageWithCheck("PT1Voltage_W2", vol);
                }
                else
                {
                    SendMessageWithCheck("PT1Current_W1", 0);
                    SendMessageWithCheck("PT1Current_W2", 0);
                    SendMessageWithCheck("PT1Voltage_W1", 0);
                    SendMessageWithCheck("PT1Voltage_W2", 0);
                }
            }
        }

        private float GetTransformedVoltage(float tc_position)
        {
            if(tc_position >= 0)
            {
                return 220 + 220 * (tc_position / 100);
            }
            else
            {
                return 220 - 220 * (tc_position / 100);
            }
        }

        private float GetTransformedCurrent(float tc_position)
        {
            if (tc_position >= 0)
            {
                return 5;
                //return 5 - 5 * ((tc_position-(float)0.5) / 100);
            }
            else
            {
                return 5;
                //return 5 + 5 * (tc_position - (float)0.5 / 100);
            }
        }

        private void SendMessageWithCheck(string mrid, float value)
        {
            foreach (var item in model.Values)
            {
                if(item.MrId == mrid)
                {
                    if(item.RawValue != value)
                    {
                        SendMessage(item.Address, value);
                    }

                    return;
                }
            }
        }

        private void SetCurrentAndVoltageSubTwo()
        {
            BasePointCacheItem tapChanger = model.Values.FirstOrDefault(x => x.MrId == "Analog_TapChanger2");

            if (tapChanger != null)
            {
                if (substationTwo.Where(x => x.MrId != "Breaker_Pump2" && x.MrId != "Breaker_Pump3").ToList().TrueForAll(x => x.State == DState.ON))
                {
                    SendMessageWithCheck("PT2Current_W1", 5);
                    SendMessageWithCheck("PT2Voltage_W1", 220);
                    SendMessageWithCheck("PT2Current_W2", GetTransformedCurrent(tapChanger.RawValue));                    
                    SendMessageWithCheck("PT2Voltage_W2", GetTransformedVoltage(tapChanger.RawValue));
                }
                else
                {
                    SendMessageWithCheck("PT2Current_W1", 0);
                    SendMessageWithCheck("PT2Current_W2", 0);
                    SendMessageWithCheck("PT2Voltage_W1", 0);
                    SendMessageWithCheck("PT2Voltage_W2", 0);
                }
            }
        }
        
        private void EmptyTank1()
        {
            float fluidFlow = 0;

            if(tank1 != null)
            {
                while (true)
                {
                    fluidFlow = pump1FluidFlow;

                    if(tank1.RawValue - fluidFlow >= 0)
                    {
                        SendMessage(tank1.Address, tank1.RawValue - fluidFlow);

                        Thread.Sleep(3000);
                    }
                }
            }
        }

        private void EmptyTank2()
        {
            float fluidFlow = 0;

            if (tank2 != null)
            {
                while (tank2.RawValue - fluidFlow >= 0)
                {
                    SendMessage(tank2.Address, tank2.RawValue - fluidFlow);

                    Thread.Sleep(3000);
                }

                while (true)
                {
                    if (Pump2Running)
                        fluidFlow = pump2FluidFlow;
                    if (Pump3Running)
                        fluidFlow += pump3FluidFlow;

                    if (tank2.RawValue - fluidFlow >= 0)
                    {
                        SendMessage(tank2.Address, tank2.RawValue - fluidFlow);

                        Thread.Sleep(3000);
                    }
                }
            }
        }

        private float CalculateFluidFlow(float fluidFlow, float pumpPower)
        {
            float powerRatio = 0;

            if (pumpPower == 0)
                return fluidFlow;

            if(pumpPower >= 1100)
            {
                powerRatio = pumpPower / 1100;

                return fluidFlow + powerRatio;
            }
            else
            {
                powerRatio = 1100 / pumpPower;

                return fluidFlow - powerRatio;
            }
        }

        private void SendMessage(ushort address, float value)
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.DIRECT_OPERATE, (ushort)TypeField.ANALOG_OUTPUT_16BIT, 0x28, 0x0001, address, (uint)value, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p, "Simulator for field.");

            functionExecutor.SendDirectMessage(fn);
        }
    }
}
