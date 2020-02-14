﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public class Breaker: IEquipment
    {
        private string mrid;
        private string gid;
        private string name;
        private string description;
        private string time;
        private DiscreteState state;

        public Breaker(string mRID, string gID, string name, string description, string time, DiscreteState state)
        {
            MRID = mRID;
            GID = gID;
            Name = name;
            Description = description;
            Time = time;
            State = state;
        }

        public Breaker() { }

        public string MRID { get => mrid; set => mrid = value; }
        public string GID { get => gid; set => gid = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Time { get => time; set => time = value; }
        public DiscreteState State { get => state; set => state = value; }

    }
}
