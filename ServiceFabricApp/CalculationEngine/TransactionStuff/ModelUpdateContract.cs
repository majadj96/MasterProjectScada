﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalculationEngine.Model;
using Common;
using Common.GDA;
using TransactionManagerContracts;

namespace CalculationEngine
{
    public class ModelUpdateContract : IModelUpdateContract
	{
		public UpdateResult UpdateModel(Delta delta)
		{
			Console.WriteLine("Update model invoked");
            ServiceEventSource.Current.Message("Update model invoked");

            ConcreteModel.CurrentModel_Copy = new Dictionary<long, IdObject>(ConcreteModel.CurrentModel);

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
                ServiceEventSource.Current.Message("CE Enlist failed");
                UpdateResult ret = new UpdateResult() { Result = ResultType.Failed, Message = e.Message };
                return ret;
                //return Task.FromResult(ret);
			}

            UpdateResult retr = new UpdateResult() { Result = ResultType.Succeeded };
            return retr;
            //return Task.FromResult(retr);
        }

		#region Populate Entities Methods

		private void RemoveEntity(ResourceDescription rd)
		{
			if (ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
			{
				ConcreteModel.CurrentModel_Copy.Remove(rd.Id);
			}
		}

		private void InsertEntity(ResourceDescription rd)
		{
			if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.ANALOG)
			{
				if (!ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
				{
					Analog analog = PopulateAnalogProperties(rd);

					ConcreteModel.CurrentModel_Copy.Add(analog.GID, analog);
				}
			}
			else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.DISCRETE)
			{
				if (!ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
				{
					Discrete discrete = PopulateDiscreteProperties(rd);

					ConcreteModel.CurrentModel_Copy.Add(discrete.GID, discrete);
				}
			}
			else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.ASYNCHRONOUSMACHINE)
			{
				if (!ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
				{
					AsyncMachine asyncMachine = PopulateAsyncMachineProperties(rd);

					ConcreteModel.CurrentModel_Copy.Add(asyncMachine.GID, asyncMachine);
				}
			}
			else
			{
				if (!ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
				{
					IdObject idObject = PopulateIdObjectProperties(rd);

					ConcreteModel.CurrentModel_Copy.Add(idObject.GID, idObject);
				}
			}
		}

		private void UpdateEntity(ResourceDescription rd)
		{
			if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.ANALOG)
			{
				if (ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
				{
					Analog analog = PopulateAnalogProperties(rd);

					ConcreteModel.CurrentModel_Copy[analog.GID] = analog;
				}
			}
			else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.DISCRETE)
			{
				if (!ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
				{
					Discrete discrete = PopulateDiscreteProperties(rd);

					ConcreteModel.CurrentModel_Copy[discrete.GID] = discrete;
				}
			}
			else if ((DMSType)(ModelCodeHelper.ExtractTypeFromGlobalId(rd.Id)) == DMSType.ASYNCHRONOUSMACHINE)
			{
				if (!ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
				{
					AsyncMachine asyncMachine = PopulateAsyncMachineProperties(rd);

					ConcreteModel.CurrentModel_Copy[asyncMachine.GID] = asyncMachine;
				}
			}
			else
			{
				if (!ConcreteModel.CurrentModel_Copy.ContainsKey(rd.Id))
				{
					IdObject idObject = PopulateIdObjectProperties(rd);

					ConcreteModel.CurrentModel_Copy[idObject.GID] = idObject;
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