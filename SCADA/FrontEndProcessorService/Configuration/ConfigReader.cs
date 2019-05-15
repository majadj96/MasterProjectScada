using ScadaCommon;
using FrontEndProcessorService.Exceptions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScadaCommon.Interfaces;

namespace FrontEndProcessorService.Configuration
{
    internal class ConfigReader : IConfiguration
	{
		private ushort transactionId = 0;

		private byte unitAddress;
		private int tcpPort;
        private int class0Acquisition;
        private int class1Acquisition;
        private int class2Acquisition;
        private int class3Acquisition;

        private Dictionary<string, IConfigItem> pointTypeToConfiguration = new Dictionary<string, IConfigItem>();

		private string path = "RtuCfg.txt";

		public ConfigReader()
		{
			if (!File.Exists(path))
			{
				OpenConfigFile();
			}

			ReadConfiguration();
		}

		public int GetAcquisitionInterval(string pointDescription)
		{
			IConfigItem ci;
			if (pointTypeToConfiguration.TryGetValue(pointDescription, out ci))
			{
				return ci.AcquisitionInterval;
			}
			throw new ArgumentException(string.Format("Invalid argument:{0}", nameof(pointDescription)));
		}

		private void OpenConfigFile()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Multiselect = false;
			dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			dlg.FileOk += Dlg_FileOk;
			dlg.ShowDialog();
		}

		private void Dlg_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			path = (sender as OpenFileDialog).FileName;
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
					if (s.StartsWith("STA"))
					{
						unitAddress = Convert.ToByte(filtered[filtered.Count - 1]);
						continue;
					}
					if (s.StartsWith("TCP"))
					{
						TcpPort = Convert.ToInt32(filtered[filtered.Count - 1]);
						continue;
					}
                    if (s.StartsWith("CLASS0"))
                    {
                        Class0Acquisition = Convert.ToInt32(filtered[filtered.Count - 1]);
                        continue;
                    }
                    if (s.StartsWith("CLASS1"))
                    {
                        Class1Acquisition = Convert.ToInt32(filtered[filtered.Count - 1]);
                        continue;
                    }
                    if (s.StartsWith("CLASS2"))
                    {
                        Class2Acquisition = Convert.ToInt32(filtered[filtered.Count - 1]);
                        continue;
                    }
                    if (s.StartsWith("CLASS3"))
                    {
                        Class3Acquisition = Convert.ToInt32(filtered[filtered.Count - 1]);
                        continue;
                    }
                }
			}
		}

		public ushort GetTransactionId()
		{
			return transactionId++;
		}

		public byte UnitAddress
		{
			get
			{
				return unitAddress;
			}

			private set
			{
				unitAddress = value;
			}
		}

		public int TcpPort
		{
			get
			{
				return tcpPort;
			}

			private set
			{
				tcpPort = value;
			}
		}

        public int Class0Acquisition
        {
            get
            {
                return class0Acquisition;
            }

            private set
            {
                class0Acquisition = value;
            }
        }

        public int Class1Acquisition
        {
            get
            {
                return class1Acquisition;
            }

            private set
            {
                class1Acquisition = value;
            }
        }
        public int Class2Acquisition
        {
            get
            {
                return class2Acquisition;
            }

            private set
            {
                class2Acquisition = value;
            }
        }
        public int Class3Acquisition
        {
            get
            {
                return class3Acquisition;
            }

            private set
            {
                class3Acquisition = value;
            }
        }
    }
}