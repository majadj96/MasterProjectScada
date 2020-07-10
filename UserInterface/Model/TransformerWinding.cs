using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
	public class TransformerWinding : IEquipment
	{
		public TransformerWinding()
		{
		}

		public TransformerWinding(string mRID, string gID, string name, string description,
            string time, float current, float voltage, long analogCurrentGID, long analogVoltageGID)
		{
			MRID = mRID;
			GID = gID;
			Name = name;
			Description = description;
			Time = time;
            Current = current;
            Voltage = voltage;
            AnalogVoltageGID = analogVoltageGID;
            AnalogCurrentGID = analogCurrentGID;
        }

        public override bool Equals(object transformerWinding)
        {
            TransformerWinding tw = (TransformerWinding)transformerWinding;
            if(this.GID == tw.GID && this.Description == tw.Description && 
                this.MRID == tw.MRID && this.Name == tw.Name && this.Time == tw.Time)
            {
                return true;
            }
            return false;
        }

        public string MRID { get; set; }
		public string GID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Time { get; set; }
        public double CurrentMinValue { get; set; }
        public double CurrentMaxValue { get; set; }
        public double VoltageMinValue { get; set; }
        public double VoltageMaxValue { get; set; }
        public float Current { get; set; }
        public float Voltage { get; set; }
        public long AnalogCurrentGID { get; set; }
        public long AnalogVoltageGID { get; set; }
	}
}
