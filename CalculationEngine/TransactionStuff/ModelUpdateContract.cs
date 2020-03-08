using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CalculationEngine.Model;
using Common;
using Common.GDA;
using TransactionManager;
using TransactionManagerContracts;

namespace CalculationEngine
{
    public class ModelUpdateContract : IModelUpdateContract
    {
        public UpdateResult UpdateModel(Delta delta)
        {
            Console.WriteLine("Update model invoked");

            CalcEngine.ConcreteModel_Copy = new Dictionary<long, IdObject>(CalcEngine.ConcreteModel);

            foreach (ResourceDescription rd in delta.InsertOperations)
            {
                if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.ANALOG)
                {
                    if (!CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
                    {
                        Analog analog = PopulateAnalogProperties(rd);

                        CalcEngine.ConcreteModel_Copy.Add(analog.GID, analog);
                    }
                }
                else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.DISCRETE)
                {
                    if (!CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
                    {
                        Discrete discrete = PopulateDiscreteProperties(rd);

                        CalcEngine.ConcreteModel_Copy.Add(discrete.GID, discrete);
                    }
                }
				else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.ASYNCHRONOUSMACHINE)
				{
					if (!CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
					{
						AsyncMachine asyncMachine = PopulateAsyncMachineProperties(rd);

						CalcEngine.ConcreteModel_Copy.Add(asyncMachine.GID, asyncMachine);
					}
				}
			}

            foreach (ResourceDescription rd in delta.UpdateOperations)
            {
                if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.ANALOG)
                {
                    if (CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
                    {
                        Analog analog = PopulateAnalogProperties(rd);

                        CalcEngine.ConcreteModel_Copy[analog.GID] = analog;
                    }
                }
                else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.DISCRETE)
                {
                    if (!CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
                    {
                        Discrete discrete = PopulateDiscreteProperties(rd);

                        CalcEngine.ConcreteModel_Copy[discrete.GID] = discrete;
                    }
                }
				else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.ASYNCHRONOUSMACHINE)
				{
					if (!CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
					{
						AsyncMachine asyncMachine = PopulateAsyncMachineProperties(rd);

						CalcEngine.ConcreteModel_Copy[asyncMachine.GID] = asyncMachine;
					}
				}
			}

            foreach (ResourceDescription rd in delta.DeleteOperations)
            {
                if (CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
                {
                    CalcEngine.ConcreteModel_Copy.Remove(rd.Id);
                }
            }

            try
            {
                TMProxy _proxy = new TMProxy(new TransactionService());
                _proxy.Enlist();
            }
            catch (Exception e)
            {
                Console.WriteLine("CE Enlist failed. " + e.Message);
                return new UpdateResult() { Result = ResultType.Failed, Message = e.Message };
            }

            return new UpdateResult() { Result = ResultType.Succeeded };
        }

		private AsyncMachine PopulateAsyncMachineProperties(ResourceDescription rd)
		{
			AsyncMachine asyncMachine = new AsyncMachine(rd.Id)
			{
				MRID = rd.GetProperty(ModelCode.IDOBJ_MRID).AsString(),
				Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString(),
				Description = rd.GetProperty(ModelCode.IDOBJ_DESC).AsString(),
				RatedP = rd.GetProperty(ModelCode.ASYNCMACHINE_RATEDP).AsFloat(),
				CosPhi = rd.GetProperty(ModelCode.ASYNCMACHINE_COSPHI).AsFloat()
			};

			return asyncMachine;
		}

		private Discrete PopulateDiscreteProperties(ResourceDescription rd)
        {
            Discrete discrete = new Discrete(rd.Id)
            {
                MRID = rd.GetProperty(ModelCode.IDOBJ_MRID).AsString(),
                Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString(),
                Description = rd.GetProperty(ModelCode.IDOBJ_DESC).AsString(),
                MeasurementType = (MeasurementType)rd.GetProperty(ModelCode.MEASUREMENT_MEASTYPE).AsEnum(),
                MaxValue = rd.GetProperty(ModelCode.DISCRETE_MAXVALUE).AsInt(),
                MinValue = rd.GetProperty(ModelCode.DISCRETE_MINVALUE).AsInt(),
                NormalValue = rd.GetProperty(ModelCode.DISCRETE_NORMALVALUE).AsInt()
            };

            return discrete;
        }

        private Analog PopulateAnalogProperties(ResourceDescription rd)
        {
            Analog analog = new Analog(rd.Id)
            {
                MRID = rd.GetProperty(ModelCode.IDOBJ_MRID).AsString(),
                Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString(),
                Description = rd.GetProperty(ModelCode.IDOBJ_DESC).AsString(),
                MeasurementType = (MeasurementType)rd.GetProperty(ModelCode.MEASUREMENT_MEASTYPE).AsEnum(),
                MaxValue = rd.GetProperty(ModelCode.ANALOG_MAXVALUE).AsFloat(),
                MinValue = rd.GetProperty(ModelCode.ANALOG_MINVALUE).AsFloat(),
                NormalValue = rd.GetProperty(ModelCode.ANALOG_NORMALVALUE).AsFloat()
            };

            return analog;
        }
    }
}
