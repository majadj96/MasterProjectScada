using CalculationEngine.Model;
using Common;
using PubSubCommon;
using ScadaCommon;
using ScadaCommon.ComandingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CalculationEngine
{
    public class CalcEngine : IPub
    {
        public static Dictionary<long, IdObject> ConcreteModel = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> ConcreteModel_Copy = new Dictionary<long, IdObject>();
        public static Dictionary<long, IdObject> ConcreteModel_Old = new Dictionary<long, IdObject>();

        private SubscribeProxy _proxy;
        private static CommandingProxy _commandingProxy;
        public static Timer aTimer = null;
        const int workingDifference = 10;

        public CalcEngine()
        {
            _proxy = new SubscribeProxy(this);
            _proxy.Subscribe("scada");

            _commandingProxy = new CommandingProxy("CECommandingProxy");
        }

        #region IPub implementation

        public void Publish(NMSModel model, string topicName)
        {
            throw new ActionNotSupportedException("CE does not have implementation for this method.");
        }

        public void PublishMeasure(ScadaUIExchangeModel[] measurement, string topicName)
        {
            ProccessData(measurement);
        }

        #endregion

        #region Timer

        public static void SetTimer()
        {
            aTimer = new Timer(5000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            long machine1Gid = ConcreteModel.Values.First(x => x.MRID == "AsyncM_1").GID;
            long machine2Gid = ConcreteModel.Values.First(x => x.MRID == "AsyncM_2").GID;
            long machine3Gid = ConcreteModel.Values.First(x => x.MRID == "AsyncM_3").GID;
            AsyncMachine asyncMachine1 = (AsyncMachine)ConcreteModel[machine1Gid];
            AsyncMachine asyncMachine2 = (AsyncMachine)ConcreteModel[machine2Gid];
            AsyncMachine asyncMachine3 = (AsyncMachine)ConcreteModel[machine3Gid];
            if (asyncMachine1.IsRunning)
            {
                ((AsyncMachine)ConcreteModel[machine1Gid]).WorkingTime += 1;
            }
            if (asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                ((AsyncMachine)ConcreteModel[machine2Gid]).WorkingTime += 1;
                ((AsyncMachine)ConcreteModel[machine3Gid]).WorkingTime += 1;
            }
            else if (asyncMachine2.IsRunning && !asyncMachine3.IsRunning)
            {
                if (asyncMachine2.WorkingTime - asyncMachine3.WorkingTime > workingDifference - 1)
                {
                    Discrete breaker2 = GetMeasurementForEquipment("Breaker_AsyncMachine2");

                    CommandObject command = new CommandObject
                    {
                        CommandingTime = DateTime.Now,
                        SignalGid = breaker2.GID,
                        EguValue = 0,
                        CommandOwner = "CalculationEngine"
                    };

                    CommandResult commandResult = _commandingProxy.WriteDigitalOutput(command);
                    Console.WriteLine(commandResult.ToString());

                    if (commandResult == CommandResult.Success)
                    {
                        Discrete breaker3 = GetMeasurementForEquipment("Breaker_AsyncMachine3");

                        CommandObject command1 = new CommandObject
                        {
                            CommandingTime = DateTime.Now,
                            SignalGid = breaker3.GID,
                            EguValue = 1,
                            CommandOwner = "CalculationEngine"
                        };

                        CommandResult commandResult1 = _commandingProxy.WriteDigitalOutput(command1);
                        Console.WriteLine(commandResult1.ToString());
                    }
                    else
                    {
                        ((AsyncMachine)ConcreteModel[machine2Gid]).WorkingTime += 1;
                    }
                }
                else
                {
                    ((AsyncMachine)ConcreteModel[machine2Gid]).WorkingTime += 1;
                }
            }
            else if (!asyncMachine2.IsRunning && asyncMachine3.IsRunning)
            {
                if (asyncMachine3.WorkingTime - asyncMachine2.WorkingTime > workingDifference - 1)
                {
                    Discrete breaker3 = GetMeasurementForEquipment("Breaker_AsyncMachine3");

                    CommandObject command = new CommandObject
                    {
                        CommandingTime = DateTime.Now,
                        SignalGid = breaker3.GID,
                        EguValue = 0,
                        CommandOwner = "CalculationEngine"
                    };

                    CommandResult commandResult = _commandingProxy.WriteDigitalOutput(command);
                    Console.WriteLine(commandResult.ToString());

                    if (commandResult == CommandResult.Success)
                    {
                        Discrete breaker2 = GetMeasurementForEquipment("Breaker_AsyncMachine2");

                        CommandObject command1 = new CommandObject
                        {
                            CommandingTime = DateTime.Now,
                            SignalGid = breaker2.GID,
                            EguValue = 1,
                            CommandOwner = "CalculationEngine"
                        };

                        CommandResult commandResult1 = _commandingProxy.WriteDigitalOutput(command1);
                        Console.WriteLine(commandResult1.ToString());
                    }
                    else
                    {
                        ((AsyncMachine)ConcreteModel[machine3Gid]).WorkingTime += 1;
                    }
                }
                else
                {
                    ((AsyncMachine)ConcreteModel[machine3Gid]).WorkingTime += 1;
                }
            }

            Console.WriteLine("AsyncM_1 working hours: " + ((AsyncMachine)ConcreteModel[machine1Gid]).WorkingTime);
            Console.WriteLine("AsyncM_2 working hours: " + ((AsyncMachine)ConcreteModel[machine2Gid]).WorkingTime);
            Console.WriteLine("AsyncM_3 working hours: " + ((AsyncMachine)ConcreteModel[machine3Gid]).WorkingTime);
            Console.WriteLine("--------------------------------");
        }

        private static Discrete GetMeasurementForEquipment(string equipmentMrid)
        {
            IdObject equipment = ConcreteModel.Values.FirstOrDefault(x => x.MRID == equipmentMrid);

            foreach (IdObject idObject in ConcreteModel.Values)
            {
                if(idObject.GetType() == typeof(Discrete))
                {
                    Discrete discrete = (Discrete)idObject;
                    if(discrete.EquipmentGid == equipment.GID)
                    {
                        return discrete;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Processing methods

        private void ProccessData(object data)
        {
            ScadaUIExchangeModel[] measurements = (ScadaUIExchangeModel[])data;

            foreach (ScadaUIExchangeModel meas in measurements)
            {
                if (ConcreteModel.ContainsKey(meas.Gid))
                {
                    if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid)) == DMSType.ANALOG)
                    {
                        ((Analog)ConcreteModel[meas.Gid]).NormalValue = (float)meas.Value;
                    }
                    else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid)) == DMSType.DISCRETE)
                    {
                        int.TryParse(meas.Value.ToString(), out int result);
                        ((Discrete)ConcreteModel[meas.Gid]).NormalValue = result;
                    }
                }
            }

            CalculateData();
        }

        public static void CalculateData()
        {
            foreach (IdObject idObject in ConcreteModel.Values)
            {
                if (idObject.GetType() == typeof(Discrete))
                {
                    Discrete discrete = (Discrete)idObject;
                    long equipId = discrete.EquipmentGid;

                    DMSType equipType = (DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(equipId));

                    if (equipType == DMSType.BREAKER)
                    {
                        IdObject breaker = ConcreteModel[equipId];

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
            long asyncM = ConcreteModel.Values.First(x => x.MRID == asyncMachine).GID;
            Discrete mainBreakerMeas = (Discrete)ConcreteModel.Values.First(x => x.MRID == mainBreaker);

            if (machineBreaker.NormalValue == 0 || mainBreakerMeas.NormalValue == 0)
            {
                //ako je breaker otvoren masina ne radi
                ((AsyncMachine)ConcreteModel[asyncM]).IsRunning = false;
            }
            else if (machineBreaker.NormalValue == 1 && mainBreakerMeas.NormalValue == 1)
            {
                //ako je breaker zatvoren masina radi
                ((AsyncMachine)ConcreteModel[asyncM]).IsRunning = true;
            }
        }

        #endregion
    }
}