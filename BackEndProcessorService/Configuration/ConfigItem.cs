using ScadaCommon;
using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndProcessorService.Configuration
{
    internal class ConfigItem : INDSConfigItem
    {
        #region Fields
        private PointType registryType;

        private uint scalingFactor;
        private uint deviation;
        private uint normalValue;
        private uint lowLimit;
        private uint highLimit;
        private uint eguMin;
        private uint eguMax;
        #endregion Fields

        #region Properties
        public PointType RegistryType
        {
            get
            {
                return registryType;
            }

            set
            {
                registryType = value;
            }
        }

        public uint ScalingFactor
        {
            get
            {
                return scalingFactor;
            }

            set
            {
                scalingFactor = value;
            }
        }

        public uint Deviation
        {
            get
            {
                return deviation;
            }

            set
            {
                deviation = value;
            }
        }

        public uint NormalValue
        {
            get
            {
                return normalValue;
            }

            set
            {
                normalValue = value;
            }
        }

        public uint LowLimit
        {
            get
            {
                return lowLimit;
            }

            set
            {
                lowLimit = value;
            }
        }

        public uint HighLimit
        {
            get
            {
                return highLimit;
            }

            set
            {
                highLimit = value;
            }
        }
        public uint EguMin
        {
            get
            {
                return eguMin;
            }

            set
            {
                eguMin = value;
            }
        }

        public uint EguMax
        {
            get
            {
                return eguMax;
            }

            set
            {
                eguMax = value;
            }
        }
        #endregion Properties

        public ConfigItem(List<string> configurationParameters)
        {
            RegistryType = GetRegistryType(configurationParameters[0]);
            uint temp;
            UInt32.TryParse(configurationParameters[1], out temp);
            ScalingFactor = temp;
            UInt32.TryParse(configurationParameters[2], out temp);
            Deviation = temp;
            UInt32.TryParse(configurationParameters[3], out temp);
            NormalValue = temp;
            UInt32.TryParse(configurationParameters[4], out temp);
            LowLimit = temp;
            UInt32.TryParse(configurationParameters[5], out temp);
            HighLimit = temp;
            UInt32.TryParse(configurationParameters[6], out temp);
            EguMin = temp;
            UInt32.TryParse(configurationParameters[7], out temp);
            EguMax = temp;
        }

        private PointType GetRegistryType(string registryTypeName)
        {
            PointType registryType;
            switch (registryTypeName)
            {
                case "BO_REG":
                    registryType = PointType.BINARY_OUTPUT;
                    break;

                case "BI_REG":
                    registryType = PointType.BINARY_INPUT;
                    break;

                case "AI_INT16":
                    registryType = PointType.ANALOG_INPUT_16;
                    break;

                case "AO_INT16":
                    registryType = PointType.ANALOG_OUTPUT_16;
                    break;

                case "CI_REG16":
                    registryType = PointType.COUNTER_INPUT_16;
                    break;

                default:
                    registryType = PointType.COUNTER_INPUT_16;
                    break;
            }
            return registryType;
        }
    }
}
