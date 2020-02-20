using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ComandingModel
{
    public class FEPCommandObject
    {
        private ushort adress;
        private string commandOwner;
        private int rawValue;

        public ushort Address
        {
            get
            {
                return adress;
            }
            set
            {
                adress = value;
            }
        }
        public string CommandOwner
        {
            get
            {
                return commandOwner;
            }
            set
            {
                commandOwner = value;
            }
        }
        public int RawValue
        {
            get
            {
                return rawValue;
            }
            set
            {
                rawValue = value;
            }
        }
    }
}
