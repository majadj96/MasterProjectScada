using System;
using System.Collections.Generic;
using System.Net;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.CRCCalculator;

namespace DNP3.FunctionParameters
{
    public class WriteDiscreteOutFunction : DNP3Functions
    {
        public WriteDiscreteOutFunction(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters)
        {

        }
        public override byte[] PackRequest()
        {
            byte[] dnp3Request = new byte[24];

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)CommandParameters.Start)), 0, dnp3Request, 0, 2);
            dnp3Request[2] = CommandParameters.Length;
            dnp3Request[3] = CommandParameters.Control;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)CommandParameters.Destination)), 0, dnp3Request, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)CommandParameters.Source)), 0, dnp3Request, 6, 2);

            ushort crc = 0;
            for (int i = 0; i < 8; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc);
            }
            crc = (ushort)(~crc);

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)crc)), 0, dnp3Request, 8, 2);
            dnp3Request[10] = CommandParameters.TransportHeader;
            dnp3Request[11] = CommandParameters.AplicationControl;
            dnp3Request[12] = CommandParameters.FunctionCode;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)CommandParameters.TypeField)), 0, dnp3Request, 13, 2);
            dnp3Request[15] = CommandParameters.Qualifier;
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((int)CommandParameters.Range)), 0, dnp3Request, 16, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((int)CommandParameters.ObjectPrefix)), 0, dnp3Request, 18, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((int)CommandParameters.ObjectValue)), 0, dnp3Request, 20, 2);

            ushort crc1 = 0;
            for (int i = 0; i < 8; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc1);
            }
            crc1 = (ushort)(~crc1);

            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)crc1)), 0, dnp3Request, 22, 2);
            return dnp3Request;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            throw new NotImplementedException();
        }
    }
}
