using CalculationEngine.Model;
using Common;
using PubSubCommon;
using ScadaCommon;
using ScadaCommon.ComandingModel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CalculationEngine
{
    public class ProcessingData
    {
        private CommandingProxy _commandingProxy;
        private PowerCalculator _powerCalculator;
        public int workingDifference;
        public bool IsModelChanged { get; set; }

        public ProcessingData()
        {
            IsModelChanged = false;
            _commandingProxy = new CommandingProxy("CECommandingProxy");
            _powerCalculator = new PowerCalculator();
        }

        public void ProccessData(object data)
        {
            Task task = new Task(() => {
                ScadaUIExchangeModel[] measurements = (ScadaUIExchangeModel[])data;

                foreach (ScadaUIExchangeModel meas in measurements)
                {
                    if (ConcreteModel.CurrentModel.TryGetValue(meas.Gid, out IdObject idObject))
                    {
                        if (idObject.GetType() == typeof(Analog))
                        {
                            ((Analog)idObject).NormalValue = (float)meas.Value;
                        }
                        else if (idObject.GetType() == typeof(Discrete))
                        {
                            Discrete discrete = (Discrete)idObject;
                            discrete.NormalValue = (int)meas.Value;
                        }
                    }
                }

                //UpdateTransformersMeasurements();
                //UpdateMachineStates();
                //UpdateFluidLevels();
            });

            task.Start();
        }

        public void UpdateMachineStates()
        {
            foreach (IdObject item in ConcreteModel.CurrentModel.Values)
            {
                if(item.GetType() == typeof(AsyncMachine))
                {
                    AsyncMachine machine = (AsyncMachine)item;
                    List<long> switchSequence = GetMachineSwitchSequence(machine.MRID, 1);

                    if (switchSequence.Count > 0)
                    {
                        // Neki prekidac je otvoren, masina ne radi
                        machine.IsRunning = false;
                    }
                    else
                    {
                        // Svi prekidaci su zatvoreni, masina radi
                        machine.IsRunning = true;
                    }

                    List<Analog> machineMeasurements = GetMeasurementsForEquipment(machine.GID);
                    Analog powerMeas = null;
                    Analog pressureMeas = null;

                    foreach (Analog meas in machineMeasurements)
                    {
                        if(meas.MeasurementType == MeasurementType.ActivePower)
                        {
                            powerMeas = meas;
                        }
                        else if(meas.Name.ToLower().Contains("pressure"))
                        {
                            pressureMeas = meas;
                        }
                    }

                    float activePower = GetActivePowerForMachine(machine.MRID);
                    float pressure = _powerCalculator.GetPressure(activePower);

                    if(powerMeas != null && powerMeas.NormalValue != activePower)
                    {
                        SendAnalogCommand(activePower, powerMeas.GID);
                    }
                    if(pressureMeas != null && pressureMeas.NormalValue != pressure)
                    {
                        SendAnalogCommand(pressure, pressureMeas.GID);
                    }
                }
            }
        }

        private float GetActivePowerForMachine(string mRID)
        {
            float voltage = 0;
            float current = 0;

            foreach (IdObject item in ConcreteModel.CurrentModel.Values)
            {
                if (mRID == "AsyncM_1")
                {
                    if (item.MRID == "TRWinding_2")
                    {
                        List<Analog> measuremets = GetMeasurementsForEquipment(item.GID);

                        foreach (Analog analog in measuremets)
                        {
                            if (analog.MeasurementType == MeasurementType.Voltage)
                            {
                                voltage = analog.NormalValue;
                            }
                            else if (analog.MeasurementType == MeasurementType.Current)
                            {
                                current = analog.NormalValue;

                            }
                        }

                        break;
                    }
                }
                else if (mRID == "AsyncM_2" || mRID == "AsyncM_3")
                {
                    if (item.MRID == "TRWinding_4")
                    {
                        List<Analog> measuremets = GetMeasurementsForEquipment(item.GID);

                        foreach (Analog analog in measuremets)
                        {
                            if (analog.MeasurementType == MeasurementType.Voltage)
                            {
                                voltage = analog.NormalValue;
                            }
                            else if (analog.MeasurementType == MeasurementType.Current)
                            {
                                current = analog.NormalValue;

                            }
                        }

                        break;
                    }
                }
            }

            return _powerCalculator.GetActivePower(voltage, current);
        }

        public void UpdateFluidLevels()
        {
            Analog level1 = (Analog)GetObjectByMrid("FluidLevel_Tank1");
            Analog level2 = (Analog)GetObjectByMrid("FluidLevel_Tank2");

            if(ConcreteModel.Tanks.Count == 0)
            {
                Tank tank1 = new Tank("Tank1", level1.GID, level1.MaxValue, level1.NormalValue);
                Tank tank2 = new Tank("Tank2", level2.GID, level2.MaxValue, level2.NormalValue);
                ConcreteModel.Tanks.Add(tank1);
                ConcreteModel.Tanks.Add(tank2);
            }
            else
            {
                ConcreteModel.Tanks[0].Capacity = level1.MaxValue;
                ConcreteModel.Tanks[0].CurrentFluidLevel = level1.NormalValue;
                ConcreteModel.Tanks[1].Capacity = level2.MaxValue;
                ConcreteModel.Tanks[1].CurrentFluidLevel = level2.NormalValue;
            }
        }

        public void UpdateWorkingTimes()
        {
            AsyncMachine asyncMachine1 = (AsyncMachine)GetObjectByMrid("AsyncM_1");
            AsyncMachine asyncMachine2 = (AsyncMachine)GetObjectByMrid("AsyncM_2");
            AsyncMachine asyncMachine3 = (AsyncMachine)GetObjectByMrid("AsyncM_3");

            if (asyncMachine1.IsRunning)
            {
                asyncMachine1.WorkingTime += 1;
                ReduceFluidLevelCommand(ConcreteModel.Tanks[0], 3); //smanji nivo fluida
			}
            if (asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                asyncMachine2.WorkingTime += 1;
                asyncMachine3.WorkingTime += 1;
                ReduceFluidLevelCommand(ConcreteModel.Tanks[1], 9);
            }
            else if (asyncMachine2.IsRunning && !asyncMachine3.IsRunning)
            {
                if (asyncMachine2.WorkingTime - asyncMachine3.WorkingTime > workingDifference - 1)
                {
                    ExecuteCommandOnMachine(asyncMachine2.MRID, 0); // Ugasi drugu
                    ExecuteCommandOnMachine(asyncMachine3.MRID, 1); // Ukljuci trecu
                }
                else
                {
                    asyncMachine2.WorkingTime += 1;
                    ReduceFluidLevelCommand(ConcreteModel.Tanks[0], 4);
                }
            }
            else if (!asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                if (asyncMachine3.WorkingTime - asyncMachine2.WorkingTime > workingDifference - 1)
                {
                    ExecuteCommandOnMachine(asyncMachine3.MRID, 0); // Ugasi trecu
                    ExecuteCommandOnMachine(asyncMachine2.MRID, 1); // Ukljuci drugu
                }
                else
                {
                    asyncMachine3.WorkingTime += 1;
                    ReduceFluidLevelCommand(ConcreteModel.Tanks[0], 5);
                }
            }
            
            Console.WriteLine("AsyncM_1 working hours: " + ((AsyncMachine)ConcreteModel.CurrentModel[asyncMachine1.GID]).WorkingTime);
            Console.WriteLine("AsyncM_2 working hours: " + ((AsyncMachine)ConcreteModel.CurrentModel[asyncMachine2.GID]).WorkingTime);
            Console.WriteLine("AsyncM_3 working hours: " + ((AsyncMachine)ConcreteModel.CurrentModel[asyncMachine3.GID]).WorkingTime);
        }

        private void ReduceFluidLevelCommand(Tank tank, float value)
        {
            float commandValue = 0;

            if(tank.CurrentFluidLevel >= value)
            {
                commandValue = tank.CurrentFluidLevel - value;
            }

            CommandObject commandObject = _commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", commandValue, tank.SignalGid);
            CommandResult commandResult = _commandingProxy.WriteAnalogOutput(commandObject);
        }

        private IdObject GetObjectByMrid(string mrid)
        {
            foreach (IdObject item in ConcreteModel.CurrentModel.Values)
            {
                if (item.MRID == mrid)
                    return item;
            }

            return null;
        }

        private void ExecuteCommandOnMachine(string machineMrid, float commandValue)
        {
            List<long> switchSeqence = GetMachineSwitchSequence(machineMrid, commandValue);

            foreach (long gid in switchSeqence)
            {
                CommandObject commandObject = _commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", commandValue, gid);
                CommandResult commandResult = _commandingProxy.WriteDigitalOutput(commandObject);
                //Thread.Sleep(2000);
            }
        }

		public void CheckFluidLevel()
		{
			AsyncMachine asyncMachine1 = (AsyncMachine)GetObjectByMrid("AsyncM_1");
			AsyncMachine asyncMachine2 = (AsyncMachine)GetObjectByMrid("AsyncM_2");
			AsyncMachine asyncMachine3 = (AsyncMachine)GetObjectByMrid("AsyncM_3");
            Tank tank1 = ConcreteModel.Tanks[0];
            Tank tank2 = ConcreteModel.Tanks[1];

            Console.WriteLine("\n[Substation 1] Current fluid level: " + tank1.CurrentFluidLevel + " l.");
			Console.WriteLine("[Substation 2] Current fluid level: " + tank2.CurrentFluidLevel + " l.\n");
            Console.WriteLine("-----------------------------------------\n");

            if (tank1.IsHighLimitLevel) // gornji limit
            {
                if (!asyncMachine1.IsRunning)
                {
                    ExecuteCommandOnMachine(asyncMachine1.MRID, 1);
                }
            }
            else if (tank1.IsLowLimitLevel) // donji limit
            {
                if (asyncMachine1.IsRunning)
                {
                    ExecuteCommandOnMachine(asyncMachine1.MRID, 0);
                }
            }

			if (tank2.IsHighLimitLevel) //pali obje, ako imamo dvije masine
			{
                if (!asyncMachine2.IsRunning)
                {
                    ExecuteCommandOnMachine(asyncMachine2.MRID, 1);
                }

                if (!asyncMachine3.IsRunning)
                {
                    ExecuteCommandOnMachine(asyncMachine3.MRID, 1);
                }
            }
            else if (tank2.IsLowLimitLevel) // ugasi sve masine koje rade
            {
                if (asyncMachine2.IsRunning)
                {
                    ExecuteCommandOnMachine(asyncMachine2.MRID, 0);
                }

                if (asyncMachine3.IsRunning)
                {
                    ExecuteCommandOnMachine(asyncMachine3.MRID, 0);
                }
            }
        }

        private List<long> GetMachineSwitchSequence(string machineMrid, float commandValue)
        {
            List<long> trace = new List<long>();

            if (machineMrid.Equals("AsyncM_1"))
            {
                if (commandValue == 1)
                {
                    AddSwitchToTrace("Discrete_Disc1", trace);
                    AddSwitchToTrace("Discrete_Disc2", trace);
                    AddSwitchToTrace("Breaker_1SwitchStatus", trace);
                    AddSwitchToTrace("Breaker_Pump1", trace);
                }
                else
                {
                    trace.Add(GetObjectByMrid("Breaker_Pump1").GID);
                }
            }
            else if (machineMrid.Equals("AsyncM_2"))
            {
                if (commandValue == 1)
                {
                    AddSwitchToTrace("Discrete_Disc3", trace);
                    AddSwitchToTrace("Discrete_Disc4", trace);
                    AddSwitchToTrace("Breaker_2SwitchStatus", trace);
                    AddSwitchToTrace("Breaker_Pump2", trace);
                }
                else
                {
                    trace.Add(GetObjectByMrid("Breaker_Pump2").GID);
                }
            }
            else if (machineMrid.Equals("AsyncM_3"))
            {
                if (commandValue == 1)
                {
                    AddSwitchToTrace("Discrete_Disc3", trace);
                    AddSwitchToTrace("Discrete_Disc4", trace);
                    AddSwitchToTrace("Breaker_2SwitchStatus", trace);
                    AddSwitchToTrace("Breaker_Pump3", trace);
                }
                else
                {
                    trace.Add(GetObjectByMrid("Breaker_Pump3").GID);
                }
            }

            return trace;
        }

        private void AddSwitchToTrace(string switchSignalMrid, List<long> trace)
        {
            Discrete switchSignal = (Discrete)GetObjectByMrid(switchSignalMrid);
            if (switchSignal.NormalValue == 0)
            {
                trace.Add(switchSignal.GID);
            }
        }

        public void UpdateTransformersMeasurements()
        {
            foreach (IdObject item in ConcreteModel.CurrentModel.Values)
            {
                if(item.GetType() == typeof(TapChanger))
                {
                    TapChanger tapChanger = (TapChanger)item;

                    if(ConcreteModel.CurrentModel.TryGetValue(tapChanger.Winding, out IdObject w))
                    {
                        TransformerWinding winding = (TransformerWinding)w;

                        Analog tapChangerPosition = GetMeasurementsForEquipment(tapChanger.GID)[0];

                        List<Analog> windingMeasurements = GetMeasurementsForEquipment(winding.GID);

                        foreach (Analog meas in windingMeasurements)
                        {
                            if (IsTransformerSupplied(winding.Transformer))
                            {
                                if (meas.MeasurementType == MeasurementType.Voltage)
                                {
                                    float commandVoltage = _powerCalculator.GetVoltagePerTCPosition(tapChangerPosition.NormalValue);

                                    if (meas.NormalValue != commandVoltage)
                                    {
                                        SendAnalogCommand(commandVoltage, meas.GID);
                                    }
                                }
                                else if (meas.MeasurementType == MeasurementType.Current)
                                {
                                    if (meas.NormalValue != _powerCalculator.NominalCurrent)
                                    {
                                        SendAnalogCommand(_powerCalculator.NominalCurrent, meas.GID);
                                    }
                                }
                            }
                            else // Transformer is not supplied
                            {
                                if (meas.MeasurementType == MeasurementType.Voltage)
                                {
                                    if (meas.NormalValue != 0)
                                    {
                                        SendAnalogCommand(0, meas.GID);
                                    }
                                }
                                else if (meas.MeasurementType == MeasurementType.Current)
                                {
                                    if (meas.NormalValue != 0)
                                    {
                                        SendAnalogCommand(0, meas.GID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SendAnalogCommand(float value, long signalGid)
        {
            CommandObject command = _commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", value, signalGid);
            _commandingProxy.WriteAnalogOutput(command);
        }

        //private Analog GetTapChangerPositionForWinding(long windingGid)
        //{
        //    foreach (IdObject item in ConcreteModel.CurrentModel.Values)
        //    {
        //        if(item.GetType() == typeof(TapChanger))
        //        {
        //            TapChanger tapChanger = (TapChanger)item;

        //            if(tapChanger.Winding == windingGid)
        //            {
        //                List<Analog> tapChangerPosition = GetMeasurementsForEquipment(tapChanger.GID);

        //                if(tapChangerPosition.Count > 0)
        //                {
        //                    return tapChangerPosition[0];
        //                }
        //            }
        //        }
        //    }

        //    return null;
        //}

        private List<Analog> GetMeasurementsForEquipment(long equipGid)
        {
            List<Analog> ret = new List<Analog>();

            foreach (IdObject item in ConcreteModel.CurrentModel.Values)
            {
                if(item.GetType() == typeof(Analog))
                {
                    Analog meas = (Analog)item;
                    if (meas.EquipmentGid == equipGid)
                        ret.Add(meas);
                }
            }

            return ret;
        }

        private bool IsTransformerSupplied(long transformerGid)
        {
            Transformer transformer = (Transformer)ConcreteModel.CurrentModel[transformerGid];

            if(transformer.MRID == "PTR_1")
            {
                if (((Discrete)GetObjectByMrid("Discrete_Disc1")).NormalValue == 0)
                    return false;
                if (((Discrete)GetObjectByMrid("Discrete_Disc2")).NormalValue == 0)
                    return false;
                if (((Discrete)GetObjectByMrid("Breaker_1SwitchStatus")).NormalValue == 0)
                    return false;
            }
            else if (transformer.MRID == "PTR_2")
            {
                if (((Discrete)GetObjectByMrid("Discrete_Disc3")).NormalValue == 0)
                    return false;
                if (((Discrete)GetObjectByMrid("Discrete_Disc4")).NormalValue == 0)
                    return false;
                if (((Discrete)GetObjectByMrid("Breaker_2SwitchStatus")).NormalValue == 0)
                    return false;
            }

            return true;
        }
    }
}
