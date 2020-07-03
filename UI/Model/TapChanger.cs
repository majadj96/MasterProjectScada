using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public class TapChanger: IEquipment
    {
        private string mrid;
        private string gid;
        private string name;
        private string description;
        private string time;
        private int highStep;
        private int lowStep;
        private int normalStep;

        public TapChanger(string mRID, string gID, string name, string description, string time, int highStep, int lowStep, int normalStep)
        {
            MRID = mRID;
            GID = gID;
            Name = name;
            Description = description;
            Time = time;
            HighStep = highStep;
            LowStep = lowStep;
            NormalStep = normalStep;
        }

        public TapChanger() { }

        public string MRID { get => mrid; set => mrid = value; }
        public string GID { get => gid; set => gid = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Time { get => time; set => time = value; }
        public int HighStep { get => highStep; set => highStep = value; }
        public int LowStep { get => lowStep; set => lowStep = value; }
        public int NormalStep { get => normalStep; set => normalStep = value; }
    }
}
