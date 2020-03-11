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
				InsertEntity(rd);
			}

			foreach (ResourceDescription rd in delta.UpdateOperations)
			{
				UpdateEntity(rd);
			}

			foreach (ResourceDescription rd in delta.DeleteOperations)
			{
				RemoveEntity(rd);
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

		#region Populate Entities

		private void RemoveEntity(ResourceDescription rd)
		{
			if (CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
			{
				CalcEngine.ConcreteModel_Copy.Remove(rd.Id);
			}
		}

		private void InsertEntity(ResourceDescription rd)
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
			else
			{
				if (!CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
				{
					IdObject idObject = PopulateIdObjectProperties(rd);

					CalcEngine.ConcreteModel_Copy.Add(idObject.GID, idObject);
				}
			}
		}

		private void UpdateEntity(ResourceDescription rd)
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
			else
			{
				if (!CalcEngine.ConcreteModel_Copy.ContainsKey(rd.Id))
				{
					IdObject idObject = PopulateIdObjectProperties(rd);

					CalcEngine.ConcreteModel_Copy[idObject.GID] = idObject;
				}
			}
		}

		private IdObject PopulateIdObjectProperties(ResourceDescription rd)
		{
			IdObject idObject = new IdObject(rd.Id);

			Property p;

			if ((p = rd.GetProperty(ModelCode.IDOBJ_MRID)) != null)
			{
				idObject.MRID = p.AsString();
			}
			if ((p = rd.GetProperty(ModelCode.IDOBJ_NAME)) != null)
			{
				idObject.Name = p.AsString();
			}
			if ((p = rd.GetProperty(ModelCode.IDOBJ_DESC)) != null)
			{
				idObject.Description = p.AsString();
			}

			return idObject;
		}

		private AsyncMachine PopulateAsyncMachineProperties(ResourceDescription rd)
		{
			IdObject idObject = PopulateIdObjectProperties(rd);

			AsyncMachine machine = new AsyncMachine(idObject.GID)
			{
				MRID = idObject.MRID,
				Name = idObject.Name,
				Description = idObject.Description
			};

			Property p;

			if ((p = rd.GetProperty(ModelCode.ASYNCMACHINE_COSPHI)) != null)
			{
				machine.CosPhi = p.AsFloat();
			}
			if ((p = rd.GetProperty(ModelCode.ASYNCMACHINE_RATEDP)) != null)
			{
				machine.RatedP = p.AsFloat();
			}

			return machine;
		}

		private Discrete PopulateDiscreteProperties(ResourceDescription rd)
		{
			IdObject idObject = PopulateIdObjectProperties(rd);

			Discrete discrete = new Discrete(idObject.GID)
			{
				MRID = idObject.MRID,
				Name = idObject.Name,
				Description = idObject.Description
			};

			Property p;

			if ((p = rd.GetProperty(ModelCode.MEASUREMENT_MEASTYPE)) != null)
			{
				discrete.MeasurementType = (MeasurementType)p.AsEnum();
			}
			if ((p = rd.GetProperty(ModelCode.DISCRETE_MAXVALUE)) != null)
			{
				discrete.MaxValue = p.AsInt();
			}
			if ((p = rd.GetProperty(ModelCode.DISCRETE_MINVALUE)) != null)
			{
				discrete.MinValue = p.AsInt();
			}
			if ((p = rd.GetProperty(ModelCode.DISCRETE_NORMALVALUE)) != null)
			{
				discrete.NormalValue = p.AsInt();
			}
            if ((p = rd.GetProperty(ModelCode.MEASUREMENT_PSR)) != null)
            {
                discrete.EquipmentGid = p.AsReference();
            }

            return discrete;
		}

		private Analog PopulateAnalogProperties(ResourceDescription rd)
		{
			IdObject idObject = PopulateIdObjectProperties(rd);

			Analog analog = new Analog(idObject.GID)
			{
				MRID = idObject.MRID,
				Name = idObject.Name,
				Description = idObject.Description
			};

			Property p;

			if ((p = rd.GetProperty(ModelCode.MEASUREMENT_MEASTYPE)) != null)
			{
				analog.MeasurementType = (MeasurementType)p.AsEnum();
			}
			if ((p = rd.GetProperty(ModelCode.ANALOG_MAXVALUE)) != null)
			{
				analog.MaxValue = p.AsFloat();
			}
			if ((p = rd.GetProperty(ModelCode.ANALOG_MINVALUE)) != null)
			{
				analog.MinValue = p.AsFloat();
			}
			if ((p = rd.GetProperty(ModelCode.ANALOG_NORMALVALUE)) != null)
			{
				analog.NormalValue = p.AsFloat();
			}
            if ((p = rd.GetProperty(ModelCode.MEASUREMENT_PSR)) != null)
            {
                analog.EquipmentGid = p.AsReference();
            }

            return analog;
		}

		#endregion
	}
}
