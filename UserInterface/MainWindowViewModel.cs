using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UserInterface
{
    public class MyLine
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
    }

    class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel(Window window)
        {
            window.WindowState = WindowState.Maximized;
            window.WindowStyle = WindowStyle.None;
            Line_1 = new MyLine();
            Line_1.X1 = Line_1.X2 = Line_1.Y1 = 0;
            Line_1.Y2 = 40;
            //setMesh();
        }
        public Window Window { get; set; }
        public MyLine line_1 { get; set; }
        public MyLine disconector { get; set; }
        public MyLine breaker { get; set; }
        public MyLine line_2 { get; set; }
        public MyLine line_3 { get; set; }


        public MyLine Line_1
        {
            get
            {
                return line_1 ;
            }
            set
            {
                line_1 = value;
                OnPropertyChanged("Line_1");
            }
        }
        public MyLine Breker
        {
            get
            {
                return breaker;
            }
            set
            {
                breaker = value;
                OnPropertyChanged("Breker");
            }
        }
        public MyLine Disconector
        {
            get
            {
                return disconector;
            }
            set
            {
                disconector = value;
                OnPropertyChanged("Disconector");
            }
        }
        public MyLine Line_2
        {
            get
            {
                return line_2;
            }
            set
            {
                line_2 = value;
                OnPropertyChanged("Line_2");
            }
        }
        public MyLine Line_3
        {
            get
            {
                return line_3;
            }
            set
            {
                line_3 = value;
                OnPropertyChanged("Line_3");
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private SolidColorBrush getColor(Color color)
        {
            return new SolidColorBrush(color);
        }

        private void setMesh()
        {
            if (disconector.X1 == 0)
            {
               // line_1.Stroke = getColor(Colors.Yellow);
            }
        }
        public void Prijava(string loz)
        {

        }

        public void Registracija()
        {

        }

    }
}
