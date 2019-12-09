﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Core
{
    public class IdentifiedObject
    {
        private string gID;
        private string mRID;
        private string name;
        private string description;

        public IdentifiedObject(string gID)
        {
            GID = gID;
        }

        public string MRID { get => mRID; set => mRID = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string GID { get => gID; set => gID = value; }
    }
}
