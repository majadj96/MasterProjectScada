using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP3.FunctionParameters
{
    public abstract class DNP3Functions : IDNP3Functions
    {
        private DNP3ApplicationObjectParameters commandParameters;

        public DNP3Functions(DNP3ApplicationObjectParameters commandParameters)
        {
            this.commandParameters = commandParameters;
        }
        /// <summary>
        /// Gets or sets the command parameters.
        /// </summary>
		public DNP3ApplicationObjectParameters CommandParameters
        {
            get
            {
                return commandParameters;
            }
            set
            {
                commandParameters = value;
            }
        }

        public abstract byte[] PackRequest();

        public abstract Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes);
    }
}
