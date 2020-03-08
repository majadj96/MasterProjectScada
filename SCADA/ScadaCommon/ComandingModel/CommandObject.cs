using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.ComandingModel
{
    public class CommandObject
    {
        private DateTime commandingTime;
        private long signalGid;
        private float eguValue;
        private string commandOwner;

        public DateTime CommandingTime {
            get
            {
                return commandingTime;
            }
            set
            {
                commandingTime = value;
            }
        }
        public long SignalGid {
            get
            {
                return signalGid;
            }
            set
            {
                signalGid = value;
            }
        }
        public string CommandOwner {
            get
            {
                return commandOwner;
            }
            set
            {
                commandOwner = value;
            }
        }
        public float EguValue
        {
            get
            {
                return eguValue;
            }
            set
            {
                eguValue = value;
            }
        }
    }
}
