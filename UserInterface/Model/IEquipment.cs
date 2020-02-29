using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface.Model
{
    public interface IEquipment
    {
        string MRID { get; set; }

        string GID { get; set; }

        string Name { get; set; }
        
        string Description { get; set; }

        string Time { get; set; }

    }
}
