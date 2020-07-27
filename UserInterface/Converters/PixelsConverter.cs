using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UserInterface.Converters
{
	public class PixelsConverter : IValueConverter
	{
		private int resolutionHeight = 900;
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int y = int.Parse(parameter.ToString());

			double percentage = 100 / (resolutionHeight / (double)y);

			double newValue = (System.Windows.SystemParameters.PrimaryScreenHeight * percentage / 100);

			return newValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return 0;
		}
	}
}
