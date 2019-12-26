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
    public class SendConfirmMessage : DNP3Functions
    {
        public SendConfirmMessage(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters)
        {
              

        }
        public override byte[] PackRequest()
        {
            byte[] dnp3Message = new byte[15];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Message, 0, 2);
            dnp3Message[2] = 8;//len
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

            ushort crc1 = 0;
            for (int i = 10; i < 13; i++)
            {
                CRCCalculator.computeCRC(dnp3Message[i], ref crc1);
            }
            crc1 = (ushort)(~crc1);

            Buffer.BlockCopy(BitConverter.GetBytes(crc1), 0, dnp3Message, 13, 2);

            return dnp3Message;
        }
    }
}
