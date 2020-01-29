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
        /// Packs the DNP3 request.
        /// </summary>
        /// <returns>The packet DNP3 request.</returns>
		byte[] PackRequest();
    }
}
