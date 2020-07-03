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

		public TransformerWinding(string mRID, string gID, string name, string description, string time)
		{
			MRID = mRID;
			GID = gID;
			Name = name;
			Description = description;
			Time = time;
		}

		public string MRID { get; set; }
		public string GID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Time { get; set; }
	}
}
