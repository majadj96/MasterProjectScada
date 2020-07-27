//using System;
//using System.Globalization;
//using System.Windows.Data;
//using System.Windows.Media;
//using UserInterface.Dialog;

//namespace UserInterface.Converters
//{
//    class DialogMessageToColorConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (value == null) return null;
//            if (value is DialogMessage message)
//            {
//                return message.Type == DialogMessageType.Info ?
//                    new SolidColorBrush(Colors.LightBlue) :
//                    new SolidColorBrush(Colors.OrangeRed);
//            }

//            throw new ArgumentException(nameof(value));
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
