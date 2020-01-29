using ScadaCommon;
using System;
using System.ComponentModel;
using System.Windows.Threading;
using ScadaCommon.Interfaces;

namespace FrontEndProcessorService.PointDataModel
{
    internal abstract class BasePointItem
	{
		protected PointType type;
		protected ushort address;
		private DateTime timestamp = DateTime.Now;
		private string name = string.Empty;
		private ushort rawValue;

        protected IConfigItem configItem;

        int pointId;

		public BasePointItem(IConfigItem c, int i)
		{
			this.configItem = c;

			this.type = c.RegistryType;
			this.address = (ushort)(c.StartIndex+i);
			this.name = $"{configItem.Description} [{i}]";
			this.rawValue = configItem.DefaultValue;
			this.pointId = PointIdentifierHelper.GetNewPointId(new PointIdentifier(this.type, this.address));
		}

		#region Properties

		public PointType Type
		{
			get
			{
				return type;
			}

			set
			{
				type = value;
			}
		}

		/// <summary>
		/// Address of point on MdbSim Simulator
		/// </summary>
		public ushort Address
		{
			get
			{
				return address;
			}

			set
			{
				address = value;
			}
		}

		public DateTime Timestamp
		{
			get
			{
				return timestamp;
			}

			set
			{
				timestamp = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}

		public virtual string DisplayValue
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Raw value, read from MdbSim
		/// </summary>
		public virtual ushort RawValue
		{
			get
			{
				return rawValue;
			}
			set
			{
				rawValue = value;
			}
		}

        public IConfigItem ConfigItem
        {
            get
            {
                return configItem;
            }
        }

        #endregion Properties

        #region Input validation

        public string Error
		{
			get
			{
				return string.Empty;
			}
		}

		public int PointId
		{
			get
			{
				return pointId;
			}
		}

		#endregion Input validation
	}
}