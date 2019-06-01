using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaCommon.Interfaces
{
    public interface IDNP3Functions
    {
        /// <summary>
        /// Parses the DNP3 response.
        /// </summary>
        /// <param name="receivedBytes">The received DNP3 response.</param>
        /// <returns>Parsed values grouped by type and address.</returns>
        Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes);

        /// <summary>
        /// Packs the DNP3 request.
        /// </summary>
        /// <returns>The packet DNP3 request.</returns>
		byte[] PackRequest();
    }
}
