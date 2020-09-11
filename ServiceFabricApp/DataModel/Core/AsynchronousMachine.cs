using Common;
using Common.GDA;
using DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Core
{
    [DataContract]
    public class AsynchronousMachine : RegulatingCondEq
    {
        public AsynchronousMachine(long gID) : base(gID)
        {
        }

        private float cosPhi;
        private float ratedP;

        [DataMember]
        public float CosPhi { get => cosPhi; set => cosPhi = value; }
        [DataMember]
        public float RatedP { get => ratedP; set => ratedP = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                AsynchronousMachine x = (AsynchronousMachine)obj;
                return ((x.cosPhi == this.cosPhi) && (x.ratedP == this.ratedP));
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
                case ModelCode.ASYNCMACHINE_COSPHI:
                case ModelCode.ASYNCMACHINE_RATEDP:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }
        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ASYNCMACHINE_COSPHI:
                    property.SetValue(cosPhi);
                    break;
                case ModelCode.ASYNCMACHINE_RATEDP:
                    property.SetValue(ratedP);
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
                case ModelCode.ASYNCMACHINE_COSPHI:
                    cosPhi = property.AsFloat();
                    break;
                case ModelCode.ASYNCMACHINE_RATEDP:
                    ratedP = property.AsFloat();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion
    }
}
