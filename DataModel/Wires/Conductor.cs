﻿using DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Wires
{
    public class Conductor : ConductingEquipment
    {
        public Conductor(long gID) : base(gID)
        {
        }

        public override bool Equals(object x)
        {
            return base.Equals(x);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
