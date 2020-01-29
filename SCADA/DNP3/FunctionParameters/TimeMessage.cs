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
            
            //Convert DateTime to 6 octets
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTime = new DateTime(1970, 1, 1);

            TimeSpan diff = dateTimeNow - dateTime;
            long milis = (long)diff.TotalMilliseconds;
            string s = "";
            for(int i = 0; i < 6; i++)
            {
                s = ((milis >> (i * 8)) & 0xff).ToString("X");
                time[i] = byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                dnp3Message[17 + i] = time[i];
            }

            //Convert 6 Octects to DateTime - NOT WORKING, YET

            //s = ""; long miliss = 0; string ss = "0";
            //for (int i = 0; i < 6; i++)
            //{
            //    ss = (miliss | (time[i]) << (i * 16)).ToString("X");
            //    miliss = long.Parse(ss, System.Globalization.NumberStyles.HexNumber);
            //}
            //DateTime dd = new DateTime(miliss / 1000);

            ushort crc1 = 0;
            for (int i = 10; i < 23; i++)
            {
                CRCCalculator.computeCRC(dnp3Message[i], ref crc1);
            }
            crc1 = (ushort)(~crc1);

            Buffer.BlockCopy(BitConverter.GetBytes(crc1), 0, dnp3Message, 23, 2);

            return dnp3Message;
        }
    }
}
