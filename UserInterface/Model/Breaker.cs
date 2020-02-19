using System;
using UserInterface.BaseError;

namespace UserInterface.Model
{
    public class Breaker : ValidationBase, IEquipment
    {
        private string mrid;
        private string gid;
        private string name;
        private string description;
        private string time;
        private DiscreteState state;
        private DiscreteState newState;

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
            set
            {
                newState = value;
                OnPropertyChanged("NewState");
            }
        }

        protected override void ValidateSelf()
        {
            /*if (NewState > 1 || NewState < 0)
            {
                this.ValidationErrors["NewState"] = "NewState cannot be less then 0 or more then 1...";
            }*/
        }

        public void AddErrorNewState(string message)
        {
            this.ValidationErrors["NewState"] = message;
            this.OnPropertyChanged("IsValid");
            this.OnPropertyChanged("ValidationErrors");
        }
    }
}
