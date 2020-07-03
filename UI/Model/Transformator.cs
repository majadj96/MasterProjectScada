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
        private long analogVoltageGID;
        private long analogCurrentGID;
        private double current;
        private double minCurrent;
        private double maxCurrent;
        private double voltage;
        private double minVoltage;
        private double maxVoltage;
        private long analogTapChangerGID;
        private long tapChangerValue;
        private double minValueTapChanger;
        private double maxValueTapChanger;

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
        public long AnalogVoltageGID
        {
            get { return analogVoltageGID; }
            set { analogVoltageGID = value; }
        }
        public long AnalogCurrentGID
        {
            get { return analogCurrentGID; }
            set { analogCurrentGID = value; }
        }
        public double Current
        {
            get { return current; }
            set { current = value; }
        }
        public double Voltage
        {
            get { return voltage; }
            set { voltage = value; }
        }
        public double MinCurrent
        {
            get { return minCurrent; }
            set { minCurrent = value; }
        }
        public double MinVoltage
        {
            get { return minVoltage; }
            set { minVoltage = value; }
        }
        public double MaxCurrent
        {
            get { return maxCurrent; }
            set { maxCurrent = value; }
        }
        public double MaxVoltage
        {
            get { return maxVoltage; }
            set { maxVoltage = value; }
        }
        public long AnalogTapChangerGID
        {
            get { return analogTapChangerGID; }
            set { analogTapChangerGID = value; }
        }
        public long TapChangerValue
        {
            get { return tapChangerValue; }
            set { tapChangerValue = value; }
        }
        public double MinValueTapChanger
        {
            get { return minValueTapChanger; }
            set { minValueTapChanger = value; }
        }
        public double MaxValueTapChanger
        {
            get { return maxValueTapChanger; }
            set { maxValueTapChanger = value; }
        }
    }
}
