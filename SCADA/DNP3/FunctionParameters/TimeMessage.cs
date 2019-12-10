using System;
using System.Collections.Generic;
using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.CRCCalculator;

namespace DNP3.FunctionParameters
{
    public class TimeMessage : DNP3Functions
    {
        public TimeMessage(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters) { }

        public override byte[] PackRequest()
        {
            byte[] dnp3Message = new byte[28];
            byte[] time = new byte[8];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Message, 0, 2);
            dnp3Message[2] = 21;
            dnp3Message[3] = CommandParameters.Control;
            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Destination), 0, dnp3Message, 4, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Source), 0, dnp3Message, 6, 2);

            ushort crc = 0;
            for (int i = 0; i < 8; i++)
            {
                CRCCalculator.computeCRC(dnp3Message[i], ref crc);
            }
            crc = (ushort)(~crc);

            Buffer.BlockCopy(BitConverter.GetBytes((short)crc), 0, dnp3Message, 8, 2);
            dnp3Message[10] = CommandParameters.TransportHeader;
            dnp3Message[11] = CommandParameters.AplicationControl;
            dnp3Message[12] = CommandParameters.FunctionCode;
            dnp3Message[13] = BitConverter.GetBytes((short)CommandParameters.TypeField)[1];
            dnp3Message[14] = BitConverter.GetBytes((short)CommandParameters.TypeField)[0];
            dnp3Message[15] = CommandParameters.Qualifier;
            Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToInt16(CommandParameters.Range)), 0, dnp3Message, 16, 2);

            DateTime dt = Convert.ToDateTime(DateTime.Now);
            time = BitConverter.GetBytes(dt.Ticks);

            Buffer.BlockCopy(time, 0, dnp3Message, 18, 8);

            ushort crc1 = 0;
            for (int i = 10; i < 26; i++)
            {
                CRCCalculator.computeCRC(dnp3Message[i], ref crc1);
            }
            crc1 = (ushort)(~crc1);

            Buffer.BlockCopy(BitConverter.GetBytes(crc1), 0, dnp3Message, 26, 2);

            return dnp3Message;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            throw new NotImplementedException();
        }
    }
}
