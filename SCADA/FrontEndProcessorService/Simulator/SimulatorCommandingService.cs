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
        Thread Tank2SimulatorThread2;

        private System.Timers.Timer timer;
        private bool pointUpdate = false;
        private float pump1FluidFlow = 4;
        private float pump2FluidFlow = 4;
        private float pump3FluidFlow = 5;

        public SimulatorCommandingService(IFunctionExecutor functionExecutor)
        {
            this.functionExecutor = functionExecutor;
            this.model = new Dictionary<Tuple<ushort, PointType>, BasePointCacheItem>();

            Tank1SimulatorThread = new Thread(EmptyTank1);
            Tank2SimulatorThread = new Thread(EmptyTank2);
            Tank2SimulatorThread2 = new Thread(EmptyTank2);

            StartTimer();
        }

        private void StartTimer()
        {
            timer = new System.Timers.Timer(3000);
            timer.Elapsed += SimulateValues;
            timer.AutoReset = true;
            timer.Start();
        }

        private void SimulateValues(object sender, ElapsedEventArgs e)
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
                if (pump1power.RawValue != pump1current.RawValue * pump1voltage.RawValue)
                {
                    SendMessage(pump1power.Address, pump1current.RawValue * pump1voltage.RawValue);
                    SendMessage(pump1speed.Address, 1450);
                }
            }
            else
            {
                if (pump1power.RawValue != 0)
                {
                    SendMessage(pump1power.Address, 0);
                    SendMessage(pump1speed.Address, 0);
                }
            }
            if (substationTwo.Count == 5 && substationTwo.Where(s => s.MrId != "Breaker_Pump3").ToList().TrueForAll(x => x.State == DState.ON))
            {
                if (pump2power.RawValue != pump2current.RawValue * pump2voltage.RawValue)
                {
                    SendMessage(pump2power.Address, pump2current.RawValue * pump2voltage.RawValue);
                    SendMessage(pump2speed.Address, 1450);
                }
            }
            else
            {
                if (pump2power.RawValue != 0)
                {
                    SendMessage(pump2power.Address, 0);
                    SendMessage(pump2speed.Address, 0);
                }
            }
            if (substationTwo.Count == 5 && substationTwo.Where(s => s.MrId != "Breaker_Pump2").ToList().TrueForAll(x => x.State == DState.ON))
            {
                if (pump3power.RawValue != pump2current.RawValue * pump2voltage.RawValue)
                {
                    SendMessage(pump3power.Address, pump2current.RawValue * pump2voltage.RawValue);
                    SendMessage(pump3speed.Address, 1450);
                }
            }
            else
            {
                if (pump3power.RawValue != 0)
                {
                    SendMessage(pump3power.Address, 0);
                    SendMessage(pump3speed.Address, 0);
                }
            }
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

            tank1 = (AnalogPointCacheItem)model.Values.FirstOrDefault(x => x.MrId == "FluidLevel_Tank1");

            if (substationOne.Count == 4 && substationOne.TrueForAll(x => x.State == DState.ON)) // pump is working
            {
                if (!Tank1SimulatorThread.IsAlive)
                {
                    Tank1SimulatorThread = new Thread(EmptyTank1);
                    Tank1SimulatorThread.Start(pump1FluidFlow);
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

            if (substationTwo.Count == 5)
            {
                if(substationTwo.TrueForAll(x => x.State == DState.ON))
                {   
                    //both pumps are running
                    if(!Tank2SimulatorThread.IsAlive)
                    {
                        Tank2SimulatorThread = new Thread(EmptyTank2);
                        Tank2SimulatorThread.Start(pump2FluidFlow + pump3FluidFlow);
                    }
                }
                if(substationTwo.Where(s => s.MrId != "Breaker_Pump3").ToList().TrueForAll(x => x.State == DState.ON))
                {
                    //pump2 is running
                    if (!Tank2SimulatorThread.IsAlive)
                    {
                        Tank2SimulatorThread = new Thread(EmptyTank2);
                        Tank2SimulatorThread.Start(pump2FluidFlow);
                    }
                }
                else
                {
                    if (Tank2SimulatorThread.IsAlive)
                    {
                        Tank2SimulatorThread.Abort();
                    }
                }
                if (substationTwo.Where(s => s.MrId != "Breaker_Pump2").ToList().TrueForAll(x => x.State == DState.ON))
                {
                    //pump3 is running
                    if (!Tank2SimulatorThread2.IsAlive)
                    {
                        Tank2SimulatorThread2 = new Thread(EmptyTank2);
                        Tank2SimulatorThread2.Start(pump3FluidFlow);
                    }
                }
                else
                {
                    if (Tank2SimulatorThread2.IsAlive)
                    {
                        Tank2SimulatorThread2.Abort();
                    }
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
            if (substationOne.Where(x => x.MrId != "Breaker_Pump1").ToList().TrueForAll(x => x.State == DState.ON))
            {
                SendMessageWithCheck("PT1Current_W1", 5);
                SendMessageWithCheck("PT1Current_W2", 5);
                SendMessageWithCheck("PT1Voltage_W1", 220);
                SendMessageWithCheck("PT1Voltage_W2", 220);
            }
            else
            {
                SendMessageWithCheck("PT1Current_W1", 0);
                SendMessageWithCheck("PT1Current_W2", 0);
                SendMessageWithCheck("PT1Voltage_W1", 0);
                SendMessageWithCheck("PT1Voltage_W2", 0);
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
            if (substationTwo.Where(x => x.MrId != "Breaker_Pump2" && x.MrId != "Breaker_Pump3").ToList().TrueForAll(x => x.State == DState.ON))
            {
                SendMessageWithCheck("PT2Current_W1", 5);
                SendMessageWithCheck("PT2Current_W2", 5);
                SendMessageWithCheck("PT2Voltage_W1", 220);
                SendMessageWithCheck("PT2Voltage_W2", 220);
            }
            else
            {
                SendMessageWithCheck("PT2Current_W1", 0);
                SendMessageWithCheck("PT2Current_W2", 0);
                SendMessageWithCheck("PT2Voltage_W1", 0);
                SendMessageWithCheck("PT2Voltage_W2", 0);
            }
        }
        
        private void EmptyTank1(object parameter)
        {
            float fluidFlow = (float)parameter;

            if(tank1 != null)
            {
                while (tank1.RawValue - fluidFlow >= 0)
                {
                    SendMessage(tank1.Address, tank1.RawValue - fluidFlow);

                    Thread.Sleep(3000);
                }
            }
        }

        private void EmptyTank2(object parameter)
        {
            float fluidFlow = (float)parameter;

            if (tank2 != null)
            {
                while (tank2.RawValue - fluidFlow >= 0)
                {
                    SendMessage(tank2.Address, tank2.RawValue - fluidFlow);

                    Thread.Sleep(3000);
                }
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
