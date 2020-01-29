using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using UserInterface.Command;
using UserInterface.Subscription;

namespace UserInterface
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel(Window window)
        {
            SubNMS subNMS = new SubNMS();
            subNMS.OnSubscribe();
            setUpLayout();
            setUpInitState();
            Messenger.Default.Register<NotificationMessage>(this, (message) => {PopulateModel(message.Notification); });
            DisconectorCommand = new DisconectorCommand(this);
            BreakerCommand = new BreakerCommand(this);
        }

        public void setUpLayout()
        {
            // window.WindowState = WindowState.Maximized;
            //  window.WindowStyle = WindowStyle.None;
        }

        public void setUpInitState()
        {
            disconector = breaker = 20;
            breaker_state = disconector_state = "ON";
            dis_color = breaker_color = line_1_color = line_2_color = line_3_color = "Black";
            statistics = "";
            pubSub = "pocetna vrednost";

            connectedStatusBar = "Dissconnected"; //SCADA konekcija
            timeStampStatusBar = DateTime.Now.ToLongDateString();  //SCADA konekcija

        }


        #region Properties
        public Window Window { get; set; }
        public int breaker { get; set; }
        public int disconector { get; set; }
        public string disconector_state { get; set; }
        public string breaker_state { get; set; }

        public string dis_color { get; set; }
        public string breaker_color { get; set; }
        public string line_1_color { get; set; }
        public string line_2_color { get; set; }
        public string line_3_color { get; set; }
        public string statistics { get; set; }
        public string pubSub { get; set; }

        public string connectedStatusBar { get; set; }
        public string timeStampStatusBar { get; set; }

        //comboSubstations lista u comboBoxu
        //SelectedSubstation izabrani u comboBoxu
        //substationItems lista
        //substationItem oznacen u listi 

        public string ConnectedStatusBar
        {
            get
            {
                return connectedStatusBar;
            }
            set
            {
                connectedStatusBar = value;
                OnPropertyChanged("ConnectedStatusBar");
            }
        }
        public string TimeStampStatusBar
        {
            get
            {
                return timeStampStatusBar;
            }
            set
            {
                timeStampStatusBar = value;
                OnPropertyChanged("TimeStampStatusBar");
            }
        }

        public string PubSub
        {
            get
            {
                return pubSub;
            }
            set
            {
                pubSub = value;
                OnPropertyChanged("PubSub");
            }
        }


        public ICommand DisconectorCommand
        {
            get;
            private set;
        }
         public ICommand BreakerCommand
        {
            get;
            private set;
        }

        public string Dis_color
        {
            get
            {
                return dis_color;
            }
            set
            {
                dis_color = value;
                OnPropertyChanged("Dis_color");
            }
        }
        public string Statistics
        {
            get
            {
                return statistics;
            }
            set
            {
                statistics = value;
                OnPropertyChanged("Statistics");
            }
        }
        public string Line_1_color
        {
            get
            {
                return line_1_color;
            }
            set
            {
                line_1_color = value;
                OnPropertyChanged("Line_1_color");
            }
        }
        public string Line_2_color
        {
            get
            {
                return line_2_color;
            }
            set
            {
                line_2_color = value;
                OnPropertyChanged("Line_2_color");
            }
        }
        public string Line_3_color
        {
            get
            {
                return line_3_color;
            }
            set
            {
                line_3_color = value;
                OnPropertyChanged("Line_3_color");
            }
        }
          public string Breaker_color
        {
            get
            {
                return breaker_color;
            }
            set
            {
                breaker_color = value;
                OnPropertyChanged("Breaker_color");
            }
        }


        public string Disconector_state
        {
            get
            {
                return disconector_state;
            }
            set
            {
                disconector_state = value;
                OnPropertyChanged("Disconector_state");
            }
        }

        public string Breaker_state
        {
            get
            {
                return breaker_state;
            }
            set
            {
                breaker_state = value;
                OnPropertyChanged("Breaker_state");
            }
        }

        public int Disconector
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
        
        public int Breaker
        {
            get
            {
                return breaker;
            }
            set
            {
                breaker = value;
                OnPropertyChanged("Breaker");
            }
        }

        #endregion

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

  
        public void DisconectorOperation()
        {
            if (Disconector == 0)
            {
                Disconector = 20;
                Disconector_state = "ON";
                Dis_color = Line_1_color = Breaker_color = Line_2_color = Line_3_color = "Black";
                Statistics = "Disconector status: OFF";
            }
            else if (Disconector == 20)
            {
                Disconector = 0;
                Disconector_state = "OFF";
                Dis_color = Line_1_color = "Yellow";
                Statistics = "Disconector status: ON";

                if (Breaker == 0)
                {
                    Breaker_color = Line_2_color = Line_3_color = "Yellow";
                }
            }
        }

    
        public void BreakerOperation()
        {
            if (Breaker == 0)
            {
                Breaker = 20;
                Breaker_state = "ON";
                Statistics = "Breaker status: OFF";
                Breaker_color = Line_2_color = Line_3_color = "Black";


            }
            else if (Breaker == 20)
            {
                Breaker = 0;
                Breaker_state = "OFF";
                Statistics = "Breaker status: ON";

                if(Disconector == 0)
                {
                    Breaker_color = Line_2_color = Line_3_color = "Yellow";

                } else if(Disconector == 20)
                {
                    Breaker_color = Line_2_color = Line_3_color = "Black";

                }

            }
        }
        public void PopulateModel(string newValue)
        {
            PubSub = newValue;
        }
    }
}
