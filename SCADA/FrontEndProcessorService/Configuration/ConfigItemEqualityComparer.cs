using System;
using System.Collections.Generic;

namespace FrontEndProcessorService.Configuration
{
    internal class ConfigItemEqualityComparer : IEqualityComparer<ConfigItem>
	{
		public bool Equals(ConfigItem x, ConfigItem y)
		{
			if (string.Compare(x.Description, y.Description) == 0)
			{
				throw new ArgumentException("Configuration item description must be unique!");
			}
			if (x.StartIndex == y.StartIndex)
			{
				throw new ArgumentException("Configuration item start address must be unique!");
			}
			if (x.StartIndex != y.StartIndex)
			{
				ConfigItem lessAddress = x.StartIndex < y.StartIndex ? x : y;
				ConfigItem greaterAddress = x.StartIndex > y.StartIndex ? x : y;
				if ((ushort)(lessAddress.StartIndex + lessAddress.NumberOfRegisters) > greaterAddress.StartIndex)
				{
					throw new ArgumentException($"Address ranges are overlapping for point types of {x.RegistryType}({x.Description}) and {y.RegistryType}({y.Description})");
				}
			}
			return false;
		}

		public int GetHashCode(ConfigItem obj)
		{
			return base.GetHashCode();
		}
	}
}