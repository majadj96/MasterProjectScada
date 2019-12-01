using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DNP3.DNP3Functions;
using ScadaCommon;

namespace DNP3.FunctionParameters
{
    public class ReadAnalogInFunction : DNP3Functions
    {
        public ReadAnalogInFunction(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters)
        {

        }
        public override byte[] PackRequest()
        {
            byte[] dnp3Request = new byte[30];

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)CommandParameters.Start)), 0, dnp3Request, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)CommandParameters.Length)), 0, dnp3Request, 2, 2);
            dnp3Request[4] = CommandParameters.Control;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)CommandParameters.Destination)), 0, dnp3Request, 5, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)CommandParameters.Source)), 0, dnp3Request, 7, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)CommandParameters.Crc)), 0, dnp3Request, 9, 2);
            dnp3Request[11] = CommandParameters.TransportHeader;
            dnp3Request[12] = CommandParameters.AplicationControl;
            dnp3Request[13] = CommandParameters.FunctionCode;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)CommandParameters.TypeField)), 0, dnp3Request, 14, 2);
            dnp3Request[16] = CommandParameters.Qualifier;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)CommandParameters.Range)), 0, dnp3Request, 17, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)CommandParameters.ObjectPrefix)), 0, dnp3Request, 21, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)CommandParameters.ObjectValue)), 0, dnp3Request, 25, 4);

            return dnp3Request;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            throw new NotImplementedException();
        }
    }
}
