using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP3.DNP3Functions;
using ScadaCommon;

namespace DNP3.FunctionParameters
{
    public class SendConfirmMessage : DNP3Functions
    {
        public SendConfirmMessage(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters)
        {

        }
        public override byte[] PackRequest()
        {
            throw new NotImplementedException();
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            throw new NotImplementedException();
        }
    }
}
