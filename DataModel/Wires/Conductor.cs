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
    }
}