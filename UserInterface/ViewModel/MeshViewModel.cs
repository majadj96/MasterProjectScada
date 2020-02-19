using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;
using System.Windows.Threading;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    public class MeshViewModel : BindableBase
    {
        //flags for elements state
        #region Variables
        private bool disconector1State { get; set; }
        private bool disconector2State { get; set; }
        private bool ptState { get; set; }
        private bool pumpState { get; set; }
        private bool pump1State { get; set; }
        private bool pump2State { get; set; }
        private string disc1Id { get; set; }
        private string disc2Id { get; set; }
        private string breakerId { get; set; }

        private Breaker breaker;
        private Disconector disconector1;
        private Disconector disconector2;
        private Transformator transformator;
        private AsynchronousMachine asynchronousMachine1;
        private AsynchronousMachine asynchronousMachine2;
        private Substation substationCurrent;
        #endregion

        #region Line_colors
        public string lineStart { get; set; }
        public string lineFirst { get; set; }
        public string lineSecond { get; set; }
        public string lineThird { get; set; }
        public string lineFourth { get; set; }
        public string lineUpDis1 { get; set; }
        public string lineUpDis2 { get; set; }
        public string lineDownDis1 { get; set; }
        public string lineDownDis2 { get; set; }
        public string lineUpPT { get; set; }
        public string lineDownPT { get; set; }
        public string lineUpBreaker { get; set; }
        public string lineDownBreaker { get; set; }
        public string lineUpPump { get; set; }
        public string lineUpFirstPump { get; set; }
        public string lineUpSecondPump { get; set; }
        public string lineUpMultiPump { get; set; }
        #endregion

        #region Props
        public Breaker Breaker
        {
            get { return breaker; }
            set { breaker = value; OnPropertyChanged("Breaker"); }
        }
        public Disconector Disconector1 { get; set; }//TODO PROPS

        public Substation SubstationCurrent
        {
            get { return substationCurrent; }
            set { substationCurrent = value; OnPropertyChanged("SubstationCurrent"); }
        }

        public string two_AM_Visible { get; set; }
        public string Two_AM_Visible
        {
            get
            {
                return two_AM_Visible;
            }
            set
            {
                two_AM_Visible = value;
                OnPropertyChanged("Two_AM_Visible");
            }
        }
        public string Disc1Id
        {
            get
            {
                return disc1Id;
            }
            set
            {
                disc1Id = value;
                OnPropertyChanged("Disc1Id");
            }
        }
        public string Disc2Id
        {
            get
            {
                return disc2Id;
            }
            set
            {
                disc2Id = value;
                OnPropertyChanged("Disc2Id");
            }
        }
        public string BreakerId
        {
            get
            {
                return breakerId;
            }
            set
            {
                breakerId = value;
                OnPropertyChanged("BreakerId");
            }
        }
        #endregion

        #region Elements_Of_Mesh
        private string disconector1Image;
        public string Disconector1Image
        {
            get
            {
                return disconector1Image;
            }
            set
            {
                disconector1Image = value;
                OnPropertyChanged("Disconector1Image");
            }
        }
        private string disconector2Image;
        public string Disconector2Image
        {
            get
            {
                return disconector2Image;
            }
            set
            {
                disconector2Image = value;
                OnPropertyChanged("Disconector2Image");
            }
        }
        private string breakerImage;
        public string BreakerImage
        {
            get
            {
                return breakerImage;
            }
            set
            {
                breakerImage = value;
                OnPropertyChanged("BreakerImage");
            }
        }
        private string pumpImage;
        public string PumpImage
        {
            get
            {
                return pumpImage;
            }
            set
            {
                pumpImage = value;
                OnPropertyChanged("PumpImage");
            }
        }
        private string pump1Image;
        public string Pump1Image
        {
            get
            {
                return pump1Image;
            }
            set
            {
                pump1Image = value;
                OnPropertyChanged("Pump1Image");
            }
        }
        private string pump2Image;
        public string Pump2Image
        {
            get
            {
                return pump2Image;
            }
            set
            {
                pump2Image = value;
                OnPropertyChanged("Pump2Image");
            }
        }
        private string ptImage;
        public string PTImage
        {
            get
            {
                return ptImage;
            }
            set
            {
                ptImage = value;
                OnPropertyChanged("PTImage");
            }
        }
        #endregion

        #region Line_prop_colors
        public string LineStart
        {
            get
            {
                return lineStart;
            }
            set
            {
                lineStart = value;
                OnPropertyChanged("LineStart");
            }
        }
        public string LineFirst
        {
            get
            {
                return lineFirst;
            }
            set
            {
                lineFirst = value;
                OnPropertyChanged("LineFirst");
            }
        }
        public string LineSecond
        {
            get
            {
                return lineSecond;
            }
            set
            {
                lineSecond = value;
                OnPropertyChanged("LineSecond");
            }
        }
        public string LineThird
        {
            get
            {
                return lineThird;
            }
            set
            {
                lineThird = value;
                OnPropertyChanged("LineThird");
            }
        }
        public string LineFourth
        {
            get
            {
                return lineFourth;
            }
            set
            {
                lineFourth = value;
                OnPropertyChanged("LineFourth");
            }
        }
        public string LineUpDis1
        {
            get
            {
                return lineUpDis1;
            }
            set
            {
                lineUpDis1 = value;
                OnPropertyChanged("LineUpDis1");
            }
        }
        public string LineUpDis2
        {
            get
            {
                return lineUpDis2;
            }
            set
            {
                lineUpDis2 = value;
                OnPropertyChanged("LineUpDis2");
            }
        }
        public string LineDownDis1
        {
            get
            {
                return lineDownDis1;
            }
            set
            {
                lineDownDis1 = value;
                OnPropertyChanged("LineDownDis1");
            }
        }
        public string LineDownDis2
        {
            get
            {
                return lineDownDis2;
            }
            set
            {
                lineDownDis2 = value;
                OnPropertyChanged("LineDownDis2");
            }
        }
        public string LineUpBreaker
        {
            get
            {
                return lineUpBreaker;
            }
            set
            {
                lineUpBreaker = value;
                OnPropertyChanged("LineUpBreaker");
            }
        }
        public string LineDownBreaker
        {
            get
            {
                return lineDownBreaker;
            }
            set
            {
                lineDownBreaker = value;
                OnPropertyChanged("LineDownBreaker");
            }
        }
        public string LineDownPT
        {
            get
            {
                return lineDownPT;
            }
            set
            {
                lineDownPT = value;
                OnPropertyChanged("LineDownPT");
            }
        }
        public string LineUpPT
        {
            get
            {
                return lineUpPT;
            }
            set
            {
                lineUpPT = value;
                OnPropertyChanged("LineUpPT");
            }
        }
        public string LineUpPump
        {
            get
            {
                return lineUpPump;
            }
            set
            {
                lineUpPump = value;
                OnPropertyChanged("LineUpPump");
            }
        }
        public string LineUpFirstPump
        {
            get
            {
                return lineUpFirstPump;
            }
            set
            {
                lineUpFirstPump = value;
                OnPropertyChanged("LineUpFirstPump");
            }
        }
        public string LineUpSecondPump
        {
            get
            {
                return lineUpSecondPump;
            }
            set
            {
                lineUpSecondPump = value;
                OnPropertyChanged("LineUpSecondPump");
            }
        }
        public string LineUpMultiPump
        {
            get
            {
                return lineUpMultiPump;
            }
            set
            {
                lineUpMultiPump = value;
                OnPropertyChanged("LineUpMultiPump");
            }
        }
        #endregion

        #region Commands
        public MyICommand<string> Commanding { get; private set; }
        public MyICommand<string> CommandDis1 { get; private set; }
        public MyICommand<string> CommandDis2 { get; private set; }
        public MyICommand CommandBreaker { get; private set; }
        public MyICommand CommandPT { get; private set; }
        #endregion

        DispatcherTimer timer = new DispatcherTimer();

        public MeshViewModel()
        {
            setUpInitState();
            CommandDis1 = CommandDis2 = new MyICommand<string>(DisconectorOperation);
            CommandBreaker = new MyICommand(BreakerOperation);
            CommandPT = new MyICommand(TapChangerOperation);
            Commanding = new MyICommand<string>(OpenCommandWindow);

            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            DataSharingWithCommandingViewModels();
        }

        public void UpdateSubstationModel(Substation substation)
        {
            SubstationCurrent = substation;
            Breaker = substation.Breaker;
            //TODO
        }

        public void DataSharingWithCommandingViewModels()
        {
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                if (string.Compare(message.Notification, "Breaker") == 0)
                {
                    Breaker.State = ((Breaker)message.Target).NewState;
                    BreakerOperation();
                }
            });
        }

        public void setUpInitState()
        {
            Breaker = new Breaker();

            populateUI();
            setState();
        }

        public void setState()
        {
            Breaker.State = DiscreteState.ON;
            disconector1State = disconector2State =
            ptState = true;
            pump1State = true;
            pump2State = true;
            pumpState = true;
        }
        public void populateUI()
        {
            //Green color: #FF7DFB4E
            //Red color: #FFFF634D
            lineFirst = lineSecond = lineThird = lineUpDis1 = lineDownDis1 = lineUpBreaker = lineDownBreaker = lineUpDis2 =
            lineDownDis2 = lineUpPT = lineDownPT = lineUpPump = lineFourth = lineStart = lineUpFirstPump = lineUpMultiPump =
            lineUpSecondPump = "#FF7DFB4E";
            Two_AM_Visible = "Visible";
            breakerImage = "Assets/breaker-on.png";
            disconector1Image = disconector2Image = "Assets/recloser-on.png";
            ptImage = "Assets/transformator-on.png";
            PumpImage = "Assets/pump-on-rotate.png";
            Pump1Image = "Assets/pump-on-rotate.png";
            Pump2Image = "Assets/pump-on-rotate.png";
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (pump1State)
            {
                if (Pump1Image.Contains("rotate"))
                    Pump1Image = "Assets/pump-on.png";
                else
                    Pump1Image = "Assets/pump-on-rotate.png";
            }
            if (pump2State)
            {
                if (Pump2Image.Contains("rotate"))
                    Pump2Image = "Assets/pump-on.png";
                else
                    Pump2Image = "Assets/pump-on-rotate.png";
            }
            if (pumpState)
            {
                if (PumpImage.Contains("rotate"))
                    PumpImage = "Assets/pump-on.png";
                else
                    PumpImage = "Assets/pump-on-rotate.png";
            }
        }

        public void OpenCommandWindow(string window)
        {
            CommandingWindow commandingWindow = new CommandingWindow();

            CommandingWindowViewModel commandingWindowViewModel = new CommandingWindowViewModel(SubstationCurrent);

            commandingWindow.DataContext = commandingWindowViewModel;

            commandingWindowViewModel.SetView(window);

            commandingWindow.Show();
        }

        public void DisconectorOperation(string type)
        {
            switch (type)
            {
                case "1":
                    if (disconector1State)
                    {
                        disconector1State = false;
                        Disconector1Image = "Assets/resloser-off.png";
                        LineUpDis1 = LineDownDis1 = LineSecond = "#FFFF634D";
                        drawBreakerOff();
                    }
                    else
                    {
                        disconector1State = true;
                        Disconector1Image = "Assets/recloser-on.png";
                        LineUpDis1 = LineDownDis1 = LineSecond = "#FF7DFB4E";

                        //if (breakerState)
                        if(Breaker.State == DiscreteState.ON)
                        {
                            drawBreakerOn();
                        }
                    }
                    break;
                case "2":
                    if (disconector2State)
                    {
                        disconector2State = false;
                        Disconector2Image = "Assets/resloser-off.png";
                        drawDis2Off();
                    }
                    else
                    {
                        disconector2State = true;
                        Disconector2Image = "Assets/recloser-on.png";
                        if (disconector1State && Breaker.State == DiscreteState.ON)
                            drawDis2On();
                    }
                    break;
                default:
                    break;
            }
        }

        public void BreakerOperation()
        {
            if (Breaker.State == DiscreteState.OFF)
            {
                Breaker.State = DiscreteState.ON;
                BreakerImage = "Assets/breaker-on.png";
                drawBreakerOn();
            }
            else
            {
                Breaker.State = DiscreteState.OFF;
                BreakerImage = "Assets/breaker-off.png";
                drawBreakerOff();
            }
        }

        public void TapChangerOperation()
        {
            if (Two_AM_Visible == "Visible")
            {
                Two_AM_Visible = "Hidden";
            }
            else
            {
                Two_AM_Visible = "Visible";
            }
        }

        #region Coloring lines
        private void drawBreakerOn()
        {
            if (disconector1State)
            {
                LineUpBreaker = LineDownBreaker = LineThird = "#FF7DFB4E";
                if (disconector2State)
                    drawDis2On();
            }
        }
        private void drawDis2On()
        {
            LineUpDis2 = LineDownDis2 = LineFourth = LineUpPT = LineDownPT = LineUpPump =
                LineUpMultiPump = LineUpFirstPump = LineUpSecondPump = "#FF7DFB4E";
            PTImage = "Assets/transformator-on.png";
            PumpImage = Pump1Image = Pump2Image = "Assets/pump-on.png";
            pump1State = pump2State = pumpState = true;
        }
        private void drawDis2Off()
        {
            LineUpDis2 = LineDownDis2 = LineFourth = LineUpPT = LineDownPT = LineUpPump = LineUpMultiPump = LineUpFirstPump = LineUpSecondPump = "#FFFF634D";
            PTImage = "Assets/transformator-off.png";
            PumpImage = Pump1Image = Pump2Image = "Assets/pump-off.png";
            pump1State = pump2State = pumpState = false;
        }
        private void drawBreakerOff()
        {
            LineDownBreaker = LineUpBreaker = LineThird = "#FFFF634D";
            drawDis2Off();

        }
        #endregion
    }
}
