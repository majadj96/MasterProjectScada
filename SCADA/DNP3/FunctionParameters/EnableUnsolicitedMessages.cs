using System;
using System.Collections.Generic;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.CRCCalculator;
namespace DNP3.FunctionParameters
{
    public class EnableUnsolicitedMessages : DNP3Functions
    {
        public EnableUnsolicitedMessages(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters) { }

        public override byte[] PackRequest()
        {
            byte[] dnp3Request = new byte[24];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Request, 0, 2);//0x0564
            dnp3Request[2] = 0x11;
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
            dnp3Request[10] = CommandParameters.TransportHeader;//0xc7
            dnp3Request[11] = CommandParameters.AplicationControl;//0xc6
            dnp3Request[12] = CommandParameters.FunctionCode;//0x14-ENABLE_UNSOLICITED
            //Class1 0x3c02 (2) 
            Buffer.BlockCopy(BitConverter.GetBytes(0x023c), 0, dnp3Request, 13, 2);
            dnp3Request[15] = CommandParameters.Qualifier;//Qual   0x06
            //Class2 0x3c03 (2) 
            Buffer.BlockCopy(BitConverter.GetBytes(0x033c), 0, dnp3Request, 16, 2);
            dnp3Request[18] = CommandParameters.Qualifier;//Qual   0x06
            //Class3 0x3c04 (2) 
            Buffer.BlockCopy(BitConverter.GetBytes(0x043c), 0, dnp3Request, 19, 2);
            dnp3Request[21] = CommandParameters.Qualifier;//Qual   0x06

            ushort crc2 = 0;
            for (int i = 10; i < 22; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc2);
            }
            crc2 = (ushort)(~crc2);
            Buffer.BlockCopy(BitConverter.GetBytes(crc2), 0, dnp3Request, 22, 2);

            return dnp3Request;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            throw new NotImplementedException();
        }
    }
}
