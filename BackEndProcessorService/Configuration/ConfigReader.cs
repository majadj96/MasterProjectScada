using ScadaCommon;
using ScadaCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndProcessorService.Configuration
{
    internal class ConfigReader : INDSConfiguration
    {
        private Dictionary<PointType, INDSConfigItem> itemToConfiguration = new Dictionary<PointType, INDSConfigItem>();
        private string path = "../../../BackEndProcessorService/NDSConfig.txt";

        public ConfigReader()
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("File does not exists");
            }

            ReadConfiguration();
        }

        private void ReadConfiguration()
        {
            using (TextReader tr = new StreamReader(path))
            {
                string s = string.Empty;
                while ((s = tr.ReadLine()) != null)
                {
                    string[] splited = s.Split(' ', '\t');
                    List<string> filtered = splited.ToList().FindAll(t => !string.IsNullOrEmpty(t));
                    if (filtered.Count == 0)
                    {
                        continue;
                    }
                    try
                    {
                        ConfigItem ci = new ConfigItem(filtered);
                        itemToConfiguration.Add(ci.RegistryType, ci);
                    }
                    catch (ArgumentException argEx)
                    {
                        throw new Exception($"Configuration error: {argEx.Message}", argEx);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (itemToConfiguration.Count == 0)
                {
                    throw new Exception("Configuration error! Check RtuCfg.txt file!");
                }
            }
        }

        public List<INDSConfigItem> GetConfigurationItems()
        {
            return new List<INDSConfigItem>(itemToConfiguration.Values);
        }

        public uint GetDeviation(PointType pointType)
        {
            INDSConfigItem item;
            if (itemToConfiguration.TryGetValue(pointType, out item))
            {
                return item.Deviation;
            }
            throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointType)));
        }

        public uint GetScalingFactor(PointType pointType)
        {
            INDSConfigItem item;
            if (itemToConfiguration.TryGetValue(pointType, out item))
            {
                return item.ScalingFactor;
            }
            throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointType)));
        }

        public uint GetEguMin(PointType pointType)
        {
            INDSConfigItem item;
            if (itemToConfiguration.TryGetValue(pointType, out item))
            {
                return item.EguMin;
            }
            throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointType)));
        }

        public uint GetEguMax(PointType pointType)
        {
            INDSConfigItem item;
            if (itemToConfiguration.TryGetValue(pointType, out item))
            {
                return item.EguMax;
            }
            throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointType)));
        }
    }
}
