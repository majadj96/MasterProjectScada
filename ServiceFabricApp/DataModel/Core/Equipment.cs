using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.GDA;

namespace DataModel.Core
{
    [DataContract]
    public class Equipment : PowerSystemResource
    {
        public Equipment(long gID) : base(gID)
        {
        }

        private long equipmentContainer = 0;

        [DataMember]
        public long EquipmentContainer { get => equipmentContainer; set => equipmentContainer = value; }

        public override bool Equals(object x)
        {
            if (base.Equals(x))
            {
                Equipment e = (Equipment)x;
                return e.equipmentContainer == this.equipmentContainer;
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

        #region IAccess implementation	

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.EQUIPMENT_EQUIPCONTAINER:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.EQUIPMENT_EQUIPCONTAINER:
                    property.SetValue(equipmentContainer);
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
                case ModelCode.EQUIPMENT_EQUIPCONTAINER:
                    equipmentContainer = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (equipmentContainer != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.EQUIPMENT_EQUIPCONTAINER] = new List<long>
                {
                    equipmentContainer
                };
            }

            base.GetReferences(references, refType);
        }

        #endregion
    }
}
