using System;
using System.Collections.Generic;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.CRCCalculator;

namespace DNP3.FunctionParameters
{
    public class InternalIndicationsMessage : DNP3Functions
    {
        public InternalIndicationsMessage(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters) { }

        public override byte[] PackRequest()
        {
            byte[] dnp3Request = new byte[21];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Request, 0, 2);//0x0564
            dnp3Request[2] = 0x0e;
            dnp3Request[3] = CommandParameters.Control; //0xc4
            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Destination), 0, dnp3Request, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Source), 0, dnp3Request, 6, 2);

            ushort crc = 0;
            for (int i = 0; i < 8; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc);
            }
            crc = (ushort)(~crc);

            Buffer.BlockCopy(BitConverter.GetBytes((short)crc), 0, dnp3Request, 8, 2);
            dnp3Request[10] = CommandParameters.TransportHeader;//0xc2
            dnp3Request[11] = CommandParameters.AplicationControl;//0xc1
            dnp3Request[12] = CommandParameters.FunctionCode;//0x02-Write
            dnp3Request[13] = BitConverter.GetBytes((short)CommandParameters.TypeField)[1];//0x5001 - INTERNAL_INDICATIONS
            dnp3Request[14] = BitConverter.GetBytes((short)CommandParameters.TypeField)[0];
            dnp3Request[15] = CommandParameters.Qualifier;//0x00
            Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToUInt16(CommandParameters.Range)), 0, dnp3Request, 16, 2);//0x0707
            dnp3Request[18] = (byte)CommandParameters.ObjectValue;//0x00

            ushort crc2 = 0;
            for (int i = 10; i < 19; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc2);
            }
            crc2 = (ushort)(~crc2);
            Buffer.BlockCopy(BitConverter.GetBytes(crc2), 0, dnp3Request, 19, 2);

            return dnp3Request;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            throw new NotImplementedException();
        }
    }
}
