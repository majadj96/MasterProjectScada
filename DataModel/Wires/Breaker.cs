﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Wires
{
    public class Breaker : ProtectedSwitch
    {
        public Breaker(long gID) : base(gID)
        {
        }
    }
}
