using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainViewModel(this);
            InitializeComponent();
        }


        //private void breaker_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (breaker.X1 == 0)
        //    {
        //        breaker.X1 = 20;
        //        statistics.Text = "Breaker status: OFF";
        //        breaker.Stroke = line_2.Stroke = line_3.Stroke = getColor(Colors.Black);
        //    }
        //    else if (breaker.X1 == 20)
        //    {
        //        breaker.X1 = 0;
        //        if (disconector.X1 == 0)
        //        {
        //            breaker.Stroke = line_2.Stroke = line_3.Stroke = getColor(Colors.Yellow);
        //        }
        //        else if (disconector.X1 == 20)
        //        {
        //            breaker.Stroke = line_2.Stroke = line_3.Stroke = getColor(Colors.Black);
        //        }
        //        statistics.Text = "Breaker status: ON";
        //    }

        //}

        //private void disconector_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (disconector.X1 == 0)
        //    {
        //        disconector.X1 = 20;
        //        disconector.Stroke =
        //        disconector.Stroke = line_1.Stroke = breaker.Stroke = line_2.Stroke = line_3.Stroke = getColor(Colors.Black);
        //        statistics.Text = "Disconector status: OFF";
        //    }
        //    else if (disconector.X1 == 20)
        //    {
        //        disconector.X1 = 0;
        //        disconector.Stroke = line_1.Stroke  = getColor(Colors.Yellow);
        //        if (breaker.X1 == 0)
        //        {
        //            breaker.Stroke = line_2.Stroke = line_3.Stroke = getColor(Colors.Yellow);
        //        }
        //        statistics.Text = "Disconector status: ON";

        //    }
        //}


    }
}
