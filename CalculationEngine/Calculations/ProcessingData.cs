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
        public int workingDifference;
        public float maxFluidLevel;
        public float currentFluidLvlTank1;   // Substation1 tank
		public float currentFluidLvlTank2;   // Substation2 tank
        public const float fluidInflow = 4;

        public ProcessingData()
        {
            _commandingProxy = new CommandingProxy("CECommandingProxy");
        }

        public void ProccessData(object data)
        {
            ScadaUIExchangeModel[] measurements = (ScadaUIExchangeModel[])data;
            List<IdObject> changeset = new List<IdObject>();

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
                        ((Discrete)idObject).NormalValue = (int)meas.Value;
                    }

                    changeset.Add(idObject);
                }
            }

            UpdateAsyncMachines(changeset);
        }

        public void UpdateAsyncMachines(List<IdObject> changeset = null)
        {
            if(changeset == null || changeset.Count == 0)
            {
                // Ako je changeset prazan, provjeri sve
                changeset = new List<IdObject>(ConcreteModel.CurrentModel.Values);
            }

            foreach (IdObject idObject in changeset)
            {
                if (idObject.GetType() == typeof(Discrete))
                {
                    Discrete discrete = (Discrete)idObject;
                    long equipId = discrete.EquipmentGid;

                    DMSType equipType = (DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(equipId));

                    if (equipType == DMSType.BREAKER)
                    {
                        IdObject breaker = ConcreteModel.CurrentModel[equipId];

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

        private void SetAsyncMachineState(string asyncMachineMrid, string mainBreaker, Discrete machineBreaker)
        {
            AsyncMachine asyncMachine = (AsyncMachine)GetObjectByMrid(asyncMachineMrid);
            Discrete mainBreakerMeas = (Discrete)GetObjectByMrid(mainBreaker);

            if (machineBreaker.NormalValue == 0 || mainBreakerMeas.NormalValue == 0)
            {
                //ako je neki breaker otvoren masina ne radi
                asyncMachine.IsRunning = false;
            }
            else if (machineBreaker.NormalValue == 1 && mainBreakerMeas.NormalValue == 1)
            {
                //ako je svaki breaker zatvoren masina radi
                asyncMachine.IsRunning = true;
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
                ReduceFluidLevelTank1(20); //smanji nivo fluida
			}
            if (asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                asyncMachine2.WorkingTime += 1;
                ReduceFluidLevelTank2(8); //smanji za 8 litara zbog prve masine 

                asyncMachine3.WorkingTime += 1;
                ReduceFluidLevelTank2(8); //smanji za 8 litara zbog druge masine
            }
            else if (asyncMachine2.IsRunning && !asyncMachine3.IsRunning)
            {
                if (asyncMachine2.WorkingTime - asyncMachine3.WorkingTime > workingDifference - 1)
                {
                    ExecuteCommandOnMachine("Breaker_AsyncMachine2", "Breaker_2SwitchStatus", 0); // Ugasi drugu

                    ExecuteCommandOnMachine("Breaker_AsyncMachine3", "Breaker_2SwitchStatus", 1); // Ukljuci trecu
                }
                else
                {
                    asyncMachine2.WorkingTime += 1;
                    ReduceFluidLevelTank2(8); //smanji za 8 litara zbog druge masine 
                }
            }
            else if (!asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                if (asyncMachine3.WorkingTime - asyncMachine2.WorkingTime > workingDifference - 1)
                {
                    ExecuteCommandOnMachine("Breaker_AsyncMachine3", "Breaker_2SwitchStatus", 0); // Ugasi trecu

                    ExecuteCommandOnMachine("Breaker_AsyncMachine2", "Breaker_2SwitchStatus", 1); // Ukljuci drugu
                }
                else
                {
                    asyncMachine3.WorkingTime += 1;
                    ReduceFluidLevelTank2(8); //smanji za 8 litara zbog trece masine
                }
            }
            
            Console.WriteLine("AsyncM_1 working hours: " + ((AsyncMachine)ConcreteModel.CurrentModel[asyncMachine1.GID]).WorkingTime);
            Console.WriteLine("AsyncM_2 working hours: " + ((AsyncMachine)ConcreteModel.CurrentModel[asyncMachine2.GID]).WorkingTime);
            Console.WriteLine("AsyncM_3 working hours: " + ((AsyncMachine)ConcreteModel.CurrentModel[asyncMachine3.GID]).WorkingTime);
        }

        private void ReduceFluidLevelTank1(float quantity)
        {
            if (currentFluidLvlTank1 < quantity)
                currentFluidLvlTank1 = 0;
            else
                currentFluidLvlTank1 -= quantity;
        }
        private void ReduceFluidLevelTank2(float quantity)
        {
            if (currentFluidLvlTank2 < quantity)
                currentFluidLvlTank2 = 0;
            else
                currentFluidLvlTank2 -= quantity;
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

        private Discrete GetMeasurementForEquipment(string equipmentMrid)
        {
            IdObject equipment = GetObjectByMrid(equipmentMrid);

            foreach (IdObject idObject in ConcreteModel.CurrentModel.Values)
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

        private void ExecuteCommandOnMachine(string machineBreakerMrid, string mainBreakerMrid, float commandValue)
        {
            Discrete machineBreakerStatus = GetMeasurementForEquipment(machineBreakerMrid);

            if (commandValue == 1)
            {
                // Ako je komanda "ON" prvo ukljuci glavni breaker
                Discrete mainBreakerStatus = (Discrete)GetObjectByMrid(mainBreakerMrid);
                
                if (mainBreakerStatus.NormalValue == 0)
                {
                    CommandObject commandObject = _commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", commandValue, mainBreakerStatus.GID);
                    CommandResult commandResult = _commandingProxy.WriteDigitalOutput(commandObject);
                    //CommandResult commandResult = await ExecuteCommand(commandObject);
                    //Console.WriteLine("[Thread: {0}] {1}", Thread.CurrentThread.ManagedThreadId, commandResult.ToString());
                    Thread.Sleep(5000);
                }
            }

            // Ukljuci breaker od masine
            CommandObject command = _commandingProxy.CreateCommand(DateTime.Now, "CalculationEngine", commandValue, machineBreakerStatus.GID);
            CommandResult commandResult1 = _commandingProxy.WriteDigitalOutput(command);
            //CommandResult commandResult1 = await ExecuteCommand(command);
            //Console.WriteLine("[Thread: {0}] {1}", Thread.CurrentThread.ManagedThreadId, commandResult1.ToString());
            Thread.Sleep(5000);
        }

        private async Task<CommandResult> ExecuteCommand(CommandObject comObj)
        {
            CommandResult result = _commandingProxy.WriteDigitalOutput(comObj);
            Thread.Sleep(2000);

            return result;
        }

		public void UpdateFluidLevel()
		{
			AsyncMachine asyncMachine1 = (AsyncMachine)GetObjectByMrid("AsyncM_1");
			AsyncMachine asyncMachine2 = (AsyncMachine)GetObjectByMrid("AsyncM_2");
			AsyncMachine asyncMachine3 = (AsyncMachine)GetObjectByMrid("AsyncM_3");

			currentFluidLvlTank1 += fluidInflow;
			currentFluidLvlTank2 += fluidInflow;

			Console.WriteLine("\n[Substation 1] Current fluid level: " + currentFluidLvlTank1 + " l.");
			Console.WriteLine("[Substation 2] Current fluid level: " + currentFluidLvlTank2 + " l.\n");
            Console.WriteLine("-----------------------------------------\n");

            if (currentFluidLvlTank1 >= maxFluidLevel * 0.9) // gornji limit
            {
                if (!asyncMachine1.IsRunning)
                {
                    ExecuteCommandOnMachine("Breaker_AsyncMachine1", "Breaker_1SwitchStatus", 1);
                }
            }
            else if (currentFluidLvlTank1 < maxFluidLevel * 0.1) // donji limit
            {
                if (asyncMachine1.IsRunning)
                {
                    ExecuteCommandOnMachine("Breaker_AsyncMachine1", "Breaker_1SwitchStatus", 0);
                }
            }

			if (currentFluidLvlTank2 >= maxFluidLevel * 0.9) //pali obje, ako imamo dvije masine
			{
                if (!asyncMachine2.IsRunning)
                {
                    ExecuteCommandOnMachine("Breaker_AsyncMachine2", "Breaker_2SwitchStatus", 1);
                }

                if (!asyncMachine3.IsRunning)
                {
                    ExecuteCommandOnMachine("Breaker_AsyncMachine3", "Breaker_2SwitchStatus", 1);
                }
            }
            else if (currentFluidLvlTank2 < maxFluidLevel * 0.1) // ugasi sve masine koje rade
            {
                if (asyncMachine2.IsRunning)
                {
                    ExecuteCommandOnMachine("Breaker_AsyncMachine2", "Breaker_2SwitchStatus", 0);
                }

                if (asyncMachine3.IsRunning)
                {
                    ExecuteCommandOnMachine("Breaker_AsyncMachine3", "Breaker_2SwitchStatus", 0);
                }
            }
        }
    }
}
