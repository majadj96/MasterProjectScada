using Common;
using Common.GDA;
using DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Wires
{
    public class TransformerWinding : ConductingEquipment
    {
        private long powerTransformer = 0;
        private long ratioTapChanger = 0;

        public TransformerWinding(long gID) : base(gID)
        {
        }

        public long PowerTransformer { get => powerTransformer; set => powerTransformer = value; }
        public long RatioTapChanger { get => ratioTapChanger; set => ratioTapChanger = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                TransformerWinding x = (TransformerWinding)obj;
                return ((x.powerTransformer == this.powerTransformer) && (x.ratioTapChanger == this.ratioTapChanger));
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess
        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.TRANSFORMERWINDING_POWERTR:
                case ModelCode.TRANSFORMERWINDING_RATIOTC:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TRANSFORMERWINDING_POWERTR:
                    property.SetValue(powerTransformer);
                    break;
                case ModelCode.TRANSFORMERWINDING_RATIOTC:
                    property.SetValue(ratioTapChanger);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TRANSFORMERWINDING_POWERTR:
                    powerTransformer = property.AsReference();
                    break;
                case ModelCode.TRANSFORMERWINDING_RATIOTC:
                    ratioTapChanger = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion

        #region IReference
        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (powerTransformer != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TRANSFORMERWINDING_POWERTR] = new List<long>();
                references[ModelCode.TRANSFORMERWINDING_POWERTR].Add(powerTransformer);
            }

            if (ratioTapChanger != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.TRANSFORMERWINDING_RATIOTC] = new List<long>();
                references[ModelCode.TRANSFORMERWINDING_RATIOTC].Add(ratioTapChanger);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.RATIOTAPCHANGER_TRWINDING:
                    ratioTapChanger = globalId;
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.RATIOTAPCHANGER_TRWINDING:

                    if (ratioTapChanger == globalId)
                    {
                        ratioTapChanger = 0;
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GID, globalId);
                    }

                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion
    }
}
