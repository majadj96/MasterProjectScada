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
            byte[] dnp3Request = new byte[35];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Request, 0, 2);
            dnp3Request[2] = 26;
            dnp3Request[3] = CommandParameters.Control;
            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Destination), 0, dnp3Request, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Source), 0, dnp3Request, 6, 2);

            ushort crc = 0;
            for (int i = 0; i < 8; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc);
            }
            crc = (ushort)(~crc);

            Buffer.BlockCopy(BitConverter.GetBytes((short)crc), 0, dnp3Request, 8, 2);
            dnp3Request[10] = CommandParameters.TransportHeader;
            dnp3Request[11] = CommandParameters.AplicationControl;
            dnp3Request[12] = CommandParameters.FunctionCode;
            dnp3Request[13] = BitConverter.GetBytes((short)CommandParameters.TypeField)[1];
            dnp3Request[14] = BitConverter.GetBytes((short)CommandParameters.TypeField)[0];
            dnp3Request[15] = CommandParameters.Qualifier;
            Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToInt16(CommandParameters.Range)), 0, dnp3Request, 16, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToInt16(CommandParameters.ObjectPrefix)), 0, dnp3Request, 18, 2);

            if (CommandParameters.ObjectValue == 1)
            {
                dnp3Request[20] = 0x41;
            }
            else
            {
                dnp3Request[20] = 0x81;
            }

            dnp3Request[21] = 0x01;
            dnp3Request[22] = 0xe8;
            dnp3Request[23] = 0x03;
            dnp3Request[24] = 0x00;
            dnp3Request[25] = 0x00;

            ushort crc2 = 0;
            for (int i = 10; i < 26; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc2);
            }
            crc2 = (ushort)(~crc2);
            Buffer.BlockCopy(BitConverter.GetBytes(crc2), 0, dnp3Request, 26, 2);

            dnp3Request[28] = 0xe8;
            dnp3Request[29] = 0x03;
            dnp3Request[30] = 0x00;
            dnp3Request[31] = 0x00;
            dnp3Request[32] = 0x00;

            ushort crc1 = 0;
            for (int i = 28; i < 33; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc1);
            }
            crc1 = (ushort)(~crc1);
            
            Buffer.BlockCopy(BitConverter.GetBytes(crc1), 0, dnp3Request, 33, 2);

            return dnp3Request;
        }
    }
}
