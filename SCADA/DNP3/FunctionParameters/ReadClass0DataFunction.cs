﻿using DNP3.DNP3Functions;
using ScadaCommon;
using ScadaCommon.CRCCalculator;
using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;

namespace DNP3.FunctionParameters
{
    public class ReadClass0DataFunction : DNP3Functions
    {
        public ReadClass0DataFunction(DNP3ApplicationObjectParameters commandParameters) : base(commandParameters)
        {
        }

        public override byte[] PackRequest()
        {
            byte[] dnp3Request = new byte[18];

            Buffer.BlockCopy(BitConverter.GetBytes((short)CommandParameters.Start), 0, dnp3Request, 0, 2);
            dnp3Request[2] = 11;
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

            ushort crc2 = 0;
            for (int i = 10; i < 16; i++)
            {
                CRCCalculator.computeCRC(dnp3Request[i], ref crc2);
            }
            crc2 = (ushort)(~crc2);
            Buffer.BlockCopy(BitConverter.GetBytes(crc2), 0, dnp3Request, 16, 2);

            return dnp3Request;
        }

        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] receivedBytes)
        {
            int len = 0, idx = 14, cnt = 0, crcCnt = 10;
            ushort typeField = 0, numberOfReg = 0, address = 0, value = 0;
            byte val = 0, mask = 0, valueByte = 0;
            Dictionary<Tuple<PointType, ushort>, ushort> dic = new Dictionary<Tuple<PointType, ushort>, ushort>();
            bool end = true;

            while (end)
            {
                val = receivedBytes[++idx];
                typeField = BitConverter.ToUInt16(new byte[2] { receivedBytes[++idx], val }, 0);
                idx++;
                if ((TypeField)typeField == TypeField.BINARY_INPUT_PACKED_FORMAT)
                {
                    val = receivedBytes[++idx];
                    numberOfReg = (ushort)(BitConverter.ToUInt16(new byte[2] { receivedBytes[++idx], val }, 0) + 1);
                    len = (ushort)Math.Ceiling((decimal)numberOfReg / 8);
                    idx++; //qualifier
                    for (int i = 0; i < len; i++)
                    {
                        val = receivedBytes[idx++];
                        for (int j = 0; j < 8; j++)
                        {
                            mask = (byte)(j + 1);
                            valueByte = (byte)((val & mask) >> (byte)j);
                            if(valueByte != 0)
                                dic.Add(new Tuple<PointType, ushort>(PointType.BINARY_INPUT, (ushort)(cnt)), BitConverter.ToUInt16(new byte[2] { valueByte, 0x00 }, 0));
                            else
                                dic.Add(new Tuple<PointType, ushort>(PointType.BINARY_INPUT, (ushort)(cnt)), 0x00);
                            cnt++;
                        }
                    }
                    idx--;
                }
                else if ((TypeField)typeField == TypeField.COUNTER_16BIT)
                {
                    idx += 3; //crc
                    val = receivedBytes[idx];
                    numberOfReg = (ushort)(BitConverter.ToUInt16(new byte[2] { receivedBytes[++idx], val }, 0) + 1);
                    len = numberOfReg * 2;
                    idx++; //qualifier
                    //parsiranje
                    idx += len + 1; //crc
                }
                else if ((TypeField)typeField == TypeField.FROZEN_COUNTER_16BIT)
                {
                    val = receivedBytes[++idx];
                    numberOfReg = (ushort)(BitConverter.ToUInt16(new byte[2] { receivedBytes[++idx], val }, 0) + 1);
                    len = numberOfReg * 2;
                    idx++; //qualifier
                    //parsiranje
                    idx += len + 1; // crc
                }
                else if ((TypeField)typeField == TypeField.ANALOG_INPUT_16BIT)
                {
                    val = receivedBytes[++idx];
                    numberOfReg = (ushort)(BitConverter.ToUInt16(new byte[2] { receivedBytes[++idx], val }, 0) + 1);
                    len = numberOfReg * 2;
                    idx++; //qualifier
                    for (int i = 0; i < len - 1; i++)
                    {
                        address = (ushort)i;
                        val = (byte)receivedBytes[idx];
                        value = BitConverter.ToUInt16(new byte[2] { receivedBytes[++idx], val }, 0);
                        dic.Add(new Tuple<PointType, ushort>(PointType.ANALOG_INPUT_16, address), value);
                    }
                }
                else if ((TypeField)typeField == TypeField.BINARY_OUTPUT_PACKED_FORMAT)
                {
                    val = receivedBytes[++idx];
                    numberOfReg = (ushort)(BitConverter.ToUInt16(new byte[2] { receivedBytes[++idx], val }, 0) + 1);
                    len = (ushort)Math.Floor((decimal)numberOfReg / 8);
                    idx++; //qualifier
                    cnt = 0;
                    for (int i = 0; i < len; i++)
                    {
                        val = receivedBytes[idx++];
                        for (int j = 0; j < 8; j++)
                        {
                            mask = (byte)j;
                            valueByte = (byte)((val & mask) >> (byte)j);
                            if (valueByte != 0)
                                dic.Add(new Tuple<PointType, ushort>(PointType.BINARY_OUTPUT, (ushort)(cnt)), BitConverter.ToUInt16(new byte[2] { valueByte, 0x00 }, 0));
                            else
                                dic.Add(new Tuple<PointType, ushort>(PointType.BINARY_OUTPUT, (ushort)(cnt)), 0x00);
                            cnt++;
                        }
                    }
                }
                else if ((TypeField)typeField == TypeField.ANALOG_OUTPUT_STATUS_16BIT)
                {
                    val = receivedBytes[++idx];
                    numberOfReg = (ushort)(BitConverter.ToUInt16(new byte[2] { receivedBytes[++idx], val }, 0) + 1);
                    len = numberOfReg * 2;
                    idx++; //qualifier
                    for (int i = 0; i < len - 2; i++)
                    {
                        address = (ushort)i;
                        i++; //quality
                        val = (byte)receivedBytes[i];
                        value = BitConverter.ToUInt16(new byte[2] { receivedBytes[i++], val }, 0);
                        dic.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT_16, address), value);
                    }
                    idx += len;
                    end = false;
                }
                else
                    end = false;
            }
            return dic;
        }
    }
}
