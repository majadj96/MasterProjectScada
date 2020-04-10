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
		public static float currentFluidLevel = 60; //dodati u konfiguraciju
		public static float currentFluidLevel1 = 70;

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
				currentFluidLevel1 -= 3; //smanji za 3 litra
			}
            if (asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                asyncMachine2.WorkingTime += 1;
				currentFluidLevel -= 1; //smanji za 1 litar zbog prve masine 
                asyncMachine3.WorkingTime += 1;
				currentFluidLevel -= 1; //smanji za 1 litar zbog druge masine
            }
            else if (asyncMachine2.IsRunning && !asyncMachine3.IsRunning)
            {
                if (asyncMachine2.WorkingTime - asyncMachine3.WorkingTime > workingDifference - 1)
                {
                    Discrete breaker2 = GetMeasurementForEquipment("Breaker_AsyncMachine2");

					System.Threading.Thread.Sleep(1000);
                    CommandObject command = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 0, breaker2.GID);

                    CommandResult commandResult = commandingProxy.WriteDigitalOutput(command);
                    Console.WriteLine(commandResult.ToString());

                    if (commandResult == CommandResult.Success)
                    {
                        Discrete breaker3 = GetMeasurementForEquipment("Breaker_AsyncMachine3");

						System.Threading.Thread.Sleep(1000);
						CommandObject command1 = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, breaker3.GID);

                        CommandResult commandResult1 = commandingProxy.WriteDigitalOutput(command1);
                        Console.WriteLine(commandResult1.ToString());
                    }
                    else
                    {
                        asyncMachine2.WorkingTime += 1;
						currentFluidLevel -= 1; //smanji za 1 litar zbog prve masine 
					}
                }
                else
                {
                    asyncMachine2.WorkingTime += 1;
					currentFluidLevel -= 1; //smanji za 1 litar zbog prve masine 
				}
            }
            else if (!asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                if (asyncMachine3.WorkingTime - asyncMachine2.WorkingTime > workingDifference - 1)
                {
                    Discrete breaker3 = GetMeasurementForEquipment("Breaker_AsyncMachine3");

					System.Threading.Thread.Sleep(1000);
					CommandObject command = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 0, breaker3.GID);

                    CommandResult commandResult = commandingProxy.WriteDigitalOutput(command);
                    Console.WriteLine(commandResult.ToString());

                    if (commandResult == CommandResult.Success)
                    {
                        Discrete breaker2 = GetMeasurementForEquipment("Breaker_AsyncMachine2");

						System.Threading.Thread.Sleep(1000);
						CommandObject command1 = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, breaker2.GID);

                        CommandResult commandResult1 = commandingProxy.WriteDigitalOutput(command1);
                        Console.WriteLine(commandResult1.ToString());
                    }
					else
					{
                        asyncMachine3.WorkingTime += 1;
						currentFluidLevel -= 1; //smanji za 1 litar zbog druge masine
					}
                }
                else
                {
                    asyncMachine3.WorkingTime += 1;
					currentFluidLevel -= 1; //smanji za 1 litar zbog druge masine
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

		public static void UpdateFluidLevel()
		{
			AsyncMachine asyncMachine1 = (AsyncMachine)GetEquipmentByMrid("AsyncM_1");
			AsyncMachine asyncMachine2 = (AsyncMachine)GetEquipmentByMrid("AsyncM_2");
			AsyncMachine asyncMachine3 = (AsyncMachine)GetEquipmentByMrid("AsyncM_3");

			currentFluidLevel += (float)1.6;
			currentFluidLevel1 += (float)1.4;

			Console.WriteLine("\n[Substation 1] Current fluid level: " + currentFluidLevel1 + " l.");
			Console.WriteLine("[Substation 2] Current fluid level: " + currentFluidLevel + " l.\n");

			if(currentFluidLevel1 >= maxFluidLevel * 0.9)
			{
				if (!asyncMachine1.IsRunning)
				{
					Discrete mainBreaker = (Discrete)CalcEngine.ConcreteModel.Values.First(x => x.MRID == "Breaker_1SwitchStatus");

					Discrete breaker1 = GetMeasurementForEquipment("Breaker_AsyncMachine1");

					if (mainBreaker.NormalValue == 0)
					{
						CommandObject command1 = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, mainBreaker.GID);
						//System.Threading.Thread.Sleep(1000);
						CommandResult commandResult1 = commandingProxy.WriteDigitalOutput(command1);
						Console.WriteLine(commandResult1.ToString());						
					}

					CommandObject command = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, breaker1.GID);
					System.Threading.Thread.Sleep(1000);
					CommandResult commandResult = commandingProxy.WriteDigitalOutput(command);
					
					Console.WriteLine(commandResult.ToString());
				}
			}
			if (currentFluidLevel >= maxFluidLevel * 0.9) //pali obje, ako imamo dvije masine
			{
				if (!asyncMachine2.IsRunning || !asyncMachine3.IsRunning)//ako bilo koja od dvije masine nije ukljucena
				{
					Discrete mainBreaker = (Discrete)CalcEngine.ConcreteModel.Values.First(x => x.MRID == "Breaker_2SwitchStatus");

					if (mainBreaker.NormalValue == 0)//ukljuci glavni prekidac
					{
						CommandObject command1 = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, mainBreaker.GID);
						System.Threading.Thread.Sleep(1000);
						CommandResult commandResult1 = commandingProxy.WriteDigitalOutput(command1);
						//Console.WriteLine(commandResult1.ToString());
					}

					if (!asyncMachine2.IsRunning)
					{
						Discrete breaker2 = GetMeasurementForEquipment("Breaker_AsyncMachine2");

						CommandObject command = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, breaker2.GID);
						System.Threading.Thread.Sleep(1000);
						CommandResult commandResult = commandingProxy.WriteDigitalOutput(command);
						//Console.WriteLine(commandResult.ToString());
					}

					if (!asyncMachine3.IsRunning)
					{
						Discrete breaker3 = GetMeasurementForEquipment("Breaker_AsyncMachine3");

						CommandObject command1 = commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", 1, breaker3.GID);
						System.Threading.Thread.Sleep(1000);
						CommandResult commandResult1 = commandingProxy.WriteDigitalOutput(command1);
						//Console.WriteLine(commandResult1.ToString());
					}
				}
			}

		}
    }
}
