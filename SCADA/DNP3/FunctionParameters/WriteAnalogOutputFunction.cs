using System;
using System.Collections.Generic;
using System.Net;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.CRCCalculator;

namespace DNP3.FunctionParameters
{
    public class WriteAnalogOutputFunction : DNP3Functions
    {
        public WriteAnalogOutputFunction(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters) { }

        public override byte[] PackRequest()
        {
            byte[] dnp3Request = new byte[25];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Request, 0, 2);
            dnp3Request[2] = 18;//len
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
            Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToUInt16(CommandParameters.Range)), 0, dnp3Request, 16, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToUInt16(CommandParameters.ObjectPrefix)), 0, dnp3Request, 18, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToUInt16(CommandParameters.ObjectValue)), 0, dnp3Request, 20, 2);
            dnp3Request[22] = 0x00;

            ushort crc1 = 0;
            for (int i = 10; i < 23; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc1);
            }
            crc1 = (ushort)(~crc1);

            Buffer.BlockCopy(BitConverter.GetBytes(crc1), 0, dnp3Request, 23, 2);

            return dnp3Request;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes) // 27 bytes
        {
            DNP3ApplicationObjectParameters writeCommandParameters = CommandParameters;

            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();
            dic.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT_16, Convert.ToUInt16(writeCommandParameters.ObjectPrefix)), Convert.ToUInt16(writeCommandParameters.ObjectValue));

            return dic;
        }
    }
}
