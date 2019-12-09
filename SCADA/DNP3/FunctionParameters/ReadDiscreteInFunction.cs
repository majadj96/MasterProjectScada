using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.CRCCalculator;

namespace DNP3.FunctionParameters
{
    public class ReadDiscreteInFunction : DNP3Functions
    {
        public ReadDiscreteInFunction(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters) { }

        public override byte[] PackRequest()
        {
            byte[] dnp3Request = new byte[20];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Request, 0, 2);
            dnp3Request[2] = 0x0d;
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
            dnp3Request[16] = (byte)CommandParameters.Range;
            dnp3Request[17] = (byte)CommandParameters.Range;

            ushort crc2 = 0;
            for (int i = 10; i < 18; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc2);
            }
            crc2 = (ushort)(~crc2);
            Buffer.BlockCopy(BitConverter.GetBytes(crc2), 0, dnp3Request, 18, 2);

            return dnp3Request;
        }
        //RADI
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();
            ushort address = BitConverter.ToUInt16(new byte[2] { receivedBytes[19], 0x00 }, 0);
            byte mask = 0x80;
            byte val = (byte)((receivedBytes[20] & mask) >> 7);
            ushort value = BitConverter.ToUInt16(new byte[2] { val, 0x00 }, 0);
            dic.Add(new Tuple<PointType, ushort>(PointType.BINARY_INPUT, address), value);

            return dic;
        }
    }
}
