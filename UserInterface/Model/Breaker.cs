using Common;
using System;
using UserInterface.BaseError;

namespace UserInterface.Model
{
    public class Breaker : IEquipment
    {
        private string mrid;
        private string gid;
        private string name;
        private string description;
        private string time;
        private DiscreteState state;
        private DiscreteState newState;
        private long discreteGID;
        private PointFlag flag;
        private bool inAlarm;
        private bool operaterCommanded;
        private bool autoCommanded;

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
        public DiscreteState NewState
        {
            get { return newState; }
            set { newState = value; }
        }
        public long DiscreteGID
        {
            get { return discreteGID; }
            set { discreteGID = value; }
        }

        public PointFlag Flag
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;

                if (flag.HasFlag(PointFlag.Alarm)) { InAlarm = true; } else { InAlarm = false; }
                if (flag.HasFlag(PointFlag.AutoCommanded)) { AutoCommanded = true; } else { AutoCommanded = false; }
                if (flag.HasFlag(PointFlag.OperaterCommanded)) { OperaterCommanded = true; } else { OperaterCommanded = false; }
            }
        }

        public bool AutoCommanded { get => autoCommanded; set => autoCommanded = value; }
        public bool OperaterCommanded { get => operaterCommanded; set => operaterCommanded = value; }
        public bool InAlarm { get => inAlarm; set => inAlarm = value; }
    }
}
