using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public class UIModel
    {
        public UIModel() {}

        public UIModel(string name, string description, string time, string value, string alarm, string gID, string mRID)
        {
            Name = name;
            Description = description;
            Time = time;
            Value = value;
            Alarm = alarm;
            GID = gID;
            MRID = mRID;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Time { get; set; }

        public string Value { get; set; }

        public string Alarm { get; set; }

        public string  GID { get; set; }

        public string MRID { get; set; }

    }

   
}
