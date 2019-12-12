using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
            byte[] dnp3Message = new byte[25];
            byte[] time = new byte[6];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Message, 0, 2);
            dnp3Message[2] = 18;
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

            DateTime dt = new DateTime();
            dt = DateTime.Now;
            string str = dt.ToString("ddMMyyyyhhmmss");
            long decValue = Convert.ToInt64(str);
            string hexValue = decValue.ToString("X");
            string byteVal = "";

            long decAgain = Int64.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

            if (hexValue.Length == 11)
                hexValue = "0" + hexValue;

            for (int i = 0; i < 6; i++)
            {
                byteVal = hexValue.Substring(i * 2, 2);

                time[i] = byte.Parse(byteVal, System.Globalization.NumberStyles.HexNumber);
                dnp3Message[17 + i] = time[i];
            }
            
            ushort crc1 = 0;
            for (int i = 10; i < 23; i++)
            {
                CRCCalculator.computeCRC(dnp3Message[i], ref crc1);
            }
            crc1 = (ushort)(~crc1);

            Buffer.BlockCopy(BitConverter.GetBytes(crc1), 0, dnp3Message, 23, 2);

            return dnp3Message;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            throw new NotImplementedException();
        }
    }
}
