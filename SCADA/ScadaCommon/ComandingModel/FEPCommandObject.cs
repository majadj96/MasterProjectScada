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
