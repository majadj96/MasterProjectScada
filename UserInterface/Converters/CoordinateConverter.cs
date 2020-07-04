using System;
using System.Windows.Data;

namespace UserInterface.Converters
{

	public class CoordinateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int y = int.Parse(parameter.ToString());

			double percentage = 100 / (1080 / (double)y);

			double newValue = (System.Windows.SystemParameters.PrimaryScreenHeight * percentage / 100);

			return newValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return 0;
		}
	}
}
