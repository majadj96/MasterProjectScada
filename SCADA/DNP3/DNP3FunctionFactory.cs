﻿using DNP3.DNP3Functions;
using DNP3.FunctionParameters;
using ScadaCommon;
using ScadaCommon.Interfaces;
using System;

namespace DNP3
{
    public class DNP3FunctionFactory
    {
        public static IDNP3Functions CreateDNP3Function(DNP3ApplicationObjectParameters commandParameters)
        {
            //TODO
            //Ovde se vracaju konkretne funkcije (klase) sto nasledjuju ovaj interfejs i u njima je popunjen niz bajtova...
            switch ((DNP3FunctionCode)commandParameters.FunctionCode)
            {
                case DNP3FunctionCode.READ:
                    switch ((TypeField)commandParameters.TypeField)
                    {
                        case TypeField.BINARY_INPUT_PACKED_FORMAT:
                            return new ReadDiscreteInFunction(commandParameters);

                        case TypeField.BINARY_OUTPUT_PACKED_FORMAT:
                            return null;

                        case TypeField.ANALOG_OUTPUT_16BIT:
                            return new ReadAnalogOutputFunction(commandParameters);

                        case TypeField.ANALOG_INPUT_16IT:
                            return null;

                        case TypeField.COUNTER_16BIT:
                            return null;

                        default:
                            return null;
                    }

                case DNP3FunctionCode.DIRECT_OPERATE:
                case DNP3FunctionCode.WRITE:
                    switch ((TypeField)commandParameters.TypeField)
                    {
                        case TypeField.BINARY_INPUT_PACKED_FORMAT:
                            return null;

                        case TypeField.BINARY_COMMAND:
                            return new WriteDiscreteOutFunction(commandParameters);

                        case TypeField.ANALOG_OUTPUT_16BIT:
                            return new WriteAnalogOutputFunction(commandParameters);

                        case TypeField.ANALOG_INPUT_16IT:
                            return null;

                        case TypeField.COUNTER_16BIT:
                            return null;

                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }
    }
}
