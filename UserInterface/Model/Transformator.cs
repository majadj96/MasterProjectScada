using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public class Transformator: IEquipment
    {
        private string mrid;
        private string gid;
        private string name;
        private string description;
        private string time;
        private bool state;

        public Transformator(string mRID, string gID, string name, string description, string time)
        {
            MRID = mRID;
            GID = gID;
            Name = name;
            Description = description;
            Time = time;
        }

        public Transformator() { }

        //Should be addded more properties

        public string MRID { get => mrid; set => mrid = value; }
        public string GID { get => gid; set => gid = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Time { get => time; set => time = value; }
        public bool State { get { return state; } set { state = value; } }
    }
}
