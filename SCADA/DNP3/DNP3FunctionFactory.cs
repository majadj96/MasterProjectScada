using DNP3.DNP3Functions;
using DNP3.FunctionParameters;
using ScadaCommon;
using ScadaCommon.Interfaces;
using System;

namespace DNP3
{
    public class DNP3FunctionFactory
    {
        public static IDNP3Functions CreateDNP3Function(DNP3ApplicationObjectParameters commandParameters, string commandOwner)
        {
            //Ovde se vracaju konkretne funkcije (klase) sto nasledjuju ovaj interfejs i u njima je popunjen niz bajtova...
            switch ((DNP3FunctionCode)commandParameters.FunctionCode)
            {
                case DNP3FunctionCode.READ:
                    switch ((TypeField)commandParameters.TypeField)
                    {
                        case TypeField.BINARY_INPUT_PACKED_FORMAT:
                            return new ReadDiscreteInFunction(commandParameters) { CommandOwner = commandOwner};

                        case TypeField.BINARY_OUTPUT_PACKED_FORMAT:
                            return new ReadDiscreteOutFunction(commandParameters) { CommandOwner = commandOwner };

                        case TypeField.ANALOG_OUTPUT_STATUS_16BIT:
                            return new ReadAnalogOutputFunction(commandParameters) { CommandOwner = commandOwner };

                        case TypeField.ANALOG_INPUT_16BIT:
                            return new ReadAnalogInFunction(commandParameters) { CommandOwner = commandOwner };

                        case TypeField.COUNTER_16BIT:
                            return null;

                        case TypeField.CLASS_0_DATA:
                            return new ReadClass0DataFunction(commandParameters) { CommandOwner = commandOwner };

                        default:
                            return null;
                    }

                case DNP3FunctionCode.DIRECT_OPERATE:
                case DNP3FunctionCode.WRITE:
                    switch ((TypeField)commandParameters.TypeField)
                    {
                        case TypeField.BINARY_COMMAND:
                            return new WriteDiscreteOutFunction(commandParameters) { CommandOwner = commandOwner };

                        case TypeField.ANALOG_OUTPUT_16BIT:
                            return new WriteAnalogOutputFunction(commandParameters) { CommandOwner = commandOwner };
                            
                        case TypeField.COUNTER_16BIT:
                            return null;

                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }

        public static IDNP3Functions CreateDNP3Message(DNP3ApplicationObjectParameters commandParameters)
        {
            switch ((DNP3FunctionCode)commandParameters.FunctionCode)
            {
                case DNP3FunctionCode.CONFIRM:
                    return new SendConfirmMessage(commandParameters);

				case DNP3FunctionCode.WRITE:
                    if ((TypeField)commandParameters.TypeField == TypeField.TIME_MESSAGE)
                        return new TimeMessage(commandParameters);
                    else if ((TypeField)commandParameters.TypeField == TypeField.INTERNAL_INDICATIONS)
                        return new InternalIndicationsMessage(commandParameters);
                    else
                        return null;

                case DNP3FunctionCode.DISABLE_UNSOLICITED:
                    return new DisableUnsolicitedMessages(commandParameters);

                case DNP3FunctionCode.READ:
                    return new Class0123(commandParameters);

                case DNP3FunctionCode.DELAY_MEASUREMENT:
                    return new DelayMeasurement(commandParameters);

                case DNP3FunctionCode.ENABLE_UNSOLICITED:
                    return new EnableUnsolicitedMessages(commandParameters);

                default:
                    return null;
            }
        }
    }
}
