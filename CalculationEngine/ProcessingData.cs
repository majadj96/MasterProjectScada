using CalculationEngine.Model;
using Common;
using PubSubCommon;
using ScadaCommon;
using ScadaCommon.ComandingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    public class ProcessingData
    {
        public static CommandingProxy commandingProxy;
        public static int workingDifference;
        public static float maxFluidLevel; 

        public static void ProccessData(object data)
        {
            ScadaUIExchangeModel[] measurements = (ScadaUIExchangeModel[])data;

            foreach (ScadaUIExchangeModel meas in measurements)
            {
                if (CalcEngine.ConcreteModel.ContainsKey(meas.Gid))
                {
                    if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid)) == DMSType.ANALOG)
                    {
                        ((Analog)CalcEngine.ConcreteModel[meas.Gid]).NormalValue = (float)meas.Value;
                    }
                    else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid)) == DMSType.DISCRETE)
                    {
                        int.TryParse(meas.Value.ToString(), out int result);
                        ((Discrete)CalcEngine.ConcreteModel[meas.Gid]).NormalValue = result;
                    }
                }
            }
        }

        public static void CalculateData()
        {
            foreach (IdObject idObject in CalcEngine.ConcreteModel.Values)
            {
                if (idObject.GetType() == typeof(Discrete))
                {
                    Discrete discrete = (Discrete)idObject;
                    long equipId = discrete.EquipmentGid;

                    DMSType equipType = (DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(equipId));

                    if (equipType == DMSType.BREAKER)
                    {
                        IdObject breaker = CalcEngine.ConcreteModel[equipId];

                        if (breaker.MRID == "Breaker_AsyncMachine1")
                        {
                            SetAsyncMachineState("AsyncM_1", "Breaker_1SwitchStatus", discrete);
                        }
                        else if (breaker.MRID == "Breaker_AsyncMachine2")
                        {
                            SetAsyncMachineState("AsyncM_2", "Breaker_2SwitchStatus", discrete);
                        }
                        else if (breaker.MRID == "Breaker_AsyncMachine3")
                        {
                            SetAsyncMachineState("AsyncM_3", "Breaker_2SwitchStatus", discrete);
                        }
                    }
                }
            }
        }

        private static void SetAsyncMachineState(string asyncMachine, string mainBreaker, Discrete machineBreaker)
        {
            long asyncM = CalcEngine.ConcreteModel.Values.First(x => x.MRID == asyncMachine).GID;
            Discrete mainBreakerMeas = (Discrete)CalcEngine.ConcreteModel.Values.First(x => x.MRID == mainBreaker);

            if (machineBreaker.NormalValue == 0 || mainBreakerMeas.NormalValue == 0)
            {
                //ako je breaker otvoren masina ne radi
                ((AsyncMachine)CalcEngine.ConcreteModel[asyncM]).IsRunning = false;
            }
            else if (machineBreaker.NormalValue == 1 && mainBreakerMeas.NormalValue == 1)
            {
                //ako je breaker zatvoren masina radi
                ((AsyncMachine)CalcEngine.ConcreteModel[asyncM]).IsRunning = true;
            }
        }

        public static void UpdateWorkingTimes()
        {
            AsyncMachine asyncMachine1 = (AsyncMachine)GetEquipmentByMrid("AsyncM_1");
            AsyncMachine asyncMachine2 = (AsyncMachine)GetEquipmentByMrid("AsyncM_2");
            AsyncMachine asyncMachine3 = (AsyncMachine)GetEquipmentByMrid("AsyncM_3");

            if (asyncMachine1.IsRunning)
            {
                asyncMachine1.WorkingTime += 1;
            }
            if (asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                asyncMachine2.WorkingTime += 1;
                asyncMachine3.WorkingTime += 1;
            }
            else if (asyncMachine2.IsRunning && !asyncMachine3.IsRunning)
            {
                if (asyncMachine2.WorkingTime - asyncMachine3.WorkingTime > workingDifference - 1)
                {
                    Discrete breaker2 = GetMeasurementForEquipment("Breaker_AsyncMachine2");

                    CommandObject command = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 0, breaker2.GID);

                    CommandResult commandResult = commandingProxy.WriteDigitalOutput(command);
                    Console.WriteLine(commandResult.ToString());

                    if (commandResult == CommandResult.Success)
                    {
                        Discrete breaker3 = GetMeasurementForEquipment("Breaker_AsyncMachine3");

                        CommandObject command1 = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, breaker3.GID);

                        CommandResult commandResult1 = commandingProxy.WriteDigitalOutput(command1);
                        Console.WriteLine(commandResult1.ToString());
                    }
                    else
                    {
                        asyncMachine2.WorkingTime += 1;
                    }
                }
                else
                {
                    asyncMachine2.WorkingTime += 1;
                }
            }
            else if (!asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                if (asyncMachine3.WorkingTime - asyncMachine2.WorkingTime > workingDifference - 1)
                {
                    Discrete breaker3 = GetMeasurementForEquipment("Breaker_AsyncMachine3");

                    CommandObject command = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 0, breaker3.GID);

                    CommandResult commandResult = commandingProxy.WriteDigitalOutput(command);
                    Console.WriteLine(commandResult.ToString());

                    if (commandResult == CommandResult.Success)
                    {
                        Discrete breaker2 = GetMeasurementForEquipment("Breaker_AsyncMachine2");

                        CommandObject command1 = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, breaker2.GID);

                        CommandResult commandResult1 = commandingProxy.WriteDigitalOutput(command1);
                        Console.WriteLine(commandResult1.ToString());
                    }
                    else
                    {
                        asyncMachine3.WorkingTime += 1;
                    }
                }
                else
                {
                    asyncMachine3.WorkingTime += 1;
                }
            }
            
            Console.WriteLine("AsyncM_1 working hours: " + ((AsyncMachine)CalcEngine.ConcreteModel[asyncMachine1.GID]).WorkingTime);
            Console.WriteLine("AsyncM_2 working hours: " + ((AsyncMachine)CalcEngine.ConcreteModel[asyncMachine2.GID]).WorkingTime);
            Console.WriteLine("AsyncM_3 working hours: " + ((AsyncMachine)CalcEngine.ConcreteModel[asyncMachine3.GID]).WorkingTime);
            Console.WriteLine("--------------------------------");
        }

        private static IdObject GetEquipmentByMrid(string mrid)
        {
            foreach (IdObject item in CalcEngine.ConcreteModel.Values)
            {
                if (item.MRID == mrid)
                    return item;
            }

            return null;
        }

        private static Discrete GetMeasurementForEquipment(string equipmentMrid)
        {
            IdObject equipment = GetEquipmentByMrid(equipmentMrid);

            foreach (IdObject idObject in CalcEngine.ConcreteModel.Values)
            {
                if (idObject.GetType() == typeof(Discrete))
                {
                    Discrete discrete = (Discrete)idObject;
                    if (discrete.EquipmentGid == equipment.GID)
                    {
                        return discrete;
                    }
                }
            }

            return null;
        }
    }
}
