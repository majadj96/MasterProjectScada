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
        private string disc1Id { get; set; }
        private string disc2Id { get; set; }
        private string breakerId { get; set; }
        
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
        public string lineUpBreaker2 { get; set; }
        public string lineDownBreaker2 { get; set; }
        public string lineUpFirstPump { get; set; }
        public string lineUpSecondPump { get; set; }
        public string lineUpMultiPump { get; set; }
        public string lineUpPumpOne { get; set; }
        public string lineUpBreakerFirstPump { get; set; }
        public string lineUpBreakerSecondPump  { get; set; }
        public string lineUpBreakerFirstPumpCN { get; set; }
        public string lineUpBreakerSecondPumpCN { get; set; }
        #endregion

        #region Props
        public string LineUpBreakerSecondPumpCN
        {
            get
            {
                return lineUpBreakerSecondPumpCN;
            }
            set
            {
                lineUpBreakerSecondPumpCN = value;
                OnPropertyChanged("LineUpBreakerSecondPumpCN");
            }
        }
        public string LineUpBreakerFirstPumpCN
        {
            get
            {
                return lineUpBreakerFirstPumpCN;
            }
            set
            {
                lineUpBreakerFirstPumpCN = value;
                OnPropertyChanged("LineUpBreakerFirstPumpCN");
            }
        }
        public string LineUpBreakerSecondPump
        {
            get
            {
                return lineUpBreakerSecondPump;
            }
            set
            {
                lineUpBreakerSecondPump = value;
                OnPropertyChanged("LineUpBreakerSecondPump");
            }
        }
        public string LineDownBreaker2
        {
            get
            {
                return lineDownBreaker2;
            }
            set
            {
                lineDownBreaker2 = value;
                OnPropertyChanged("LineDownBreaker2");
            }
        }
        public string LineUpPumpOne
        {
            get
            {
                return lineUpPumpOne;
            }
            set
            {
                lineUpPumpOne = value;
                OnPropertyChanged("LineUpPumpOne");
            }
        }
        public string LineUpBreakerFirstPump
        {
            get
            {
                return lineUpBreakerFirstPump;
            }
            set
            {
                lineUpBreakerFirstPump = value;
                OnPropertyChanged("LineUpBreakerFirstPump");
            }
        }

        public Substation SubstationCurrent
        {
            get { return substationCurrent; }
            set { substationCurrent = value; OnPropertyChanged("SubstationCurrent"); }
        }

        public string singlePumpCN { get; set; }
        public string SinglePumpCN
        {
            get
            {
                return singlePumpCN;
            }
            set
            {
                singlePumpCN = value;
                OnPropertyChanged("SinglePumpCN");
            }
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
        private string breakerPumpOneImage;
        public string BreakerPumpOneImage
        {
            get
            {
                return breakerPumpOneImage;
            }
            set
            {
                breakerPumpOneImage = value;
                OnPropertyChanged("BreakerPumpOneImage");
            }
        }

        private string breaker_PM1Image;
        public string Breaker_PM1Image
        {
            get
            {
                return breaker_PM1Image;
            }
            set
            {
                breaker_PM1Image = value;
                OnPropertyChanged("Breaker_PM1Image");
            }
        }
        private string breaker_PM2Image;
        public string Breaker_PM2Image
        {
            get
            {
                return breaker_PM2Image;
            }
            set
            {
                breaker_PM2Image = value;
                OnPropertyChanged("Breaker_PM2Image");
            }
        }

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
        public string LineUpBreaker2
        {
            get
            {
                return lineUpBreaker2;
            }
            set
            {
                lineUpBreaker2 = value;
                OnPropertyChanged("LineUpBreaker2");
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
        public MyICommand<string> CommandDisconnector { get; private set; }
        public MyICommand<string> CommandBreaker { get; private set; }
        public MyICommand CommandPT { get; private set; }
        #endregion

        DispatcherTimer timer = new DispatcherTimer();

        public MeshViewModel()
        {
            setUpInitState();
            CommandDisconnector = new MyICommand<string>(DisconectorOperation);
            CommandBreaker = new MyICommand<string>(BreakerOperation);
            CommandPT = new MyICommand(TapChangerOperation);
            Commanding = new MyICommand<string>(OpenCommandWindow);

            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            Messenger.Default.Register<NotificationMessage>(this, (message) => { ChangeStatesOfElements(message.Notification, message.Target); });

            SubstationCurrent = new Substation();
        }

        public void UpdateSubstationModel(Substation substation)
        {
            SubstationCurrent = substation;

            foreach (Breaker b in SubstationCurrent.Breakers)
                b.State = DiscreteState.ON;

            foreach (AsynchronousMachine a in SubstationCurrent.AsynchronousMachines)
                a.State = true;

            SubstationCurrent.Disconectors[0].State = DiscreteState.ON;
            if (SubstationCurrent.Disconectors.Count > 1)
                SubstationCurrent.Disconectors[1].State = DiscreteState.ON;

            if (SubstationCurrent.AsynchronousMachines.Count == 1)
            {
                Two_AM_Visible = "Hidden";
                SinglePumpCN = "Visible";
            }
            else
            {
                Two_AM_Visible = "Visible";
                SinglePumpCN = "Hidden";
            }
        }

        public void ChangeStatesOfElements(string element, object target)
        {
            if (string.Compare(element, "Breaker1") == 0)
            {
                SubstationCurrent.Breakers[0].State = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[0].NewState = SubstationCurrent.Breakers[0].State;
                BreakerOperation("1");
            }
            else if (string.Compare(element, "Breaker2") == 0)
            {
                SubstationCurrent.Breakers[1].State = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[1].NewState = SubstationCurrent.Breakers[1].State;
                BreakerOperation("2");
            }
            else if (string.Compare(element, "Breaker3") == 0)
            {
                SubstationCurrent.Breakers[0].State = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[0].NewState = SubstationCurrent.Breakers[0].State;
                BreakerOperation("3");
            }
            else if (string.Compare(element, "Breaker4") == 0)
            {
                SubstationCurrent.Breakers[1].State = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[1].NewState = SubstationCurrent.Breakers[1].State;
                BreakerOperation("4");
            }
            else if (string.Compare(element, "Breaker5") == 0)
            {
                SubstationCurrent.Breakers[2].State = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[2].NewState = SubstationCurrent.Breakers[2].State;
                BreakerOperation("5");
            }
            else if (string.Compare(element, "Disconector1") == 0)
            {
                SubstationCurrent.Disconectors[0].State = ((Disconector)target).NewState;
                SubstationCurrent.Disconectors[0].NewState = SubstationCurrent.Disconectors[0].State;
                DisconectorOperation("1");
            }
            else if (string.Compare(element, "Disconector2") == 0)
            {
                SubstationCurrent.Disconectors[1].State = ((Disconector)target).NewState;
                SubstationCurrent.Disconectors[1].NewState = SubstationCurrent.Disconectors[1].State;
                DisconectorOperation("2");
            }
            //TODO add elements
        }

        public void setUpInitState()
        {
            populateUI();
            setState();
        }

        public void setState()
        {
            if (SubstationCurrent != null)
            {
                SubstationCurrent.Transformator.State = true;
                foreach (var v in SubstationCurrent.AsynchronousMachines)
                    v.State = true;
                foreach (var v in SubstationCurrent.Breakers)
                    v.State = DiscreteState.ON;
                foreach (var v in SubstationCurrent.Disconectors)
                    v.State = DiscreteState.ON;
            }
        }

        public void populateUI()
        {
            //Green color: #FF7DFB4E
            //Red color: #FFFF634D
            lineFirst = lineSecond = lineThird = lineUpDis1 = lineDownDis1 = lineUpBreaker = lineDownBreaker = lineUpDis2 =
            lineDownDis2 = lineUpPT = lineDownPT = lineUpBreaker2 = lineFourth = lineStart = lineUpFirstPump = lineUpMultiPump =
            lineUpSecondPump = lineUpPumpOne = lineUpBreakerFirstPump = lineUpBreakerSecondPump = lineDownBreaker2 =
            lineUpBreakerSecondPumpCN = lineUpBreakerFirstPumpCN = "#FF7DFB4E";
            Two_AM_Visible = "Hidden";
            breakerImage = breakerPumpOneImage = breaker_PM1Image = breaker_PM2Image = "Assets/breaker-on.png";
            disconector1Image = disconector2Image = "Assets/recloser-on.png";
            ptImage = "Assets/transformator-on.png";
            PumpImage = "Assets/pump-on-rotate.png";
            Pump1Image = "Assets/pump-on-rotate.png";
            Pump2Image = "Assets/pump-on-rotate.png";
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (SubstationCurrent != null && SubstationCurrent.AsynchronousMachines != null)
            {
                if (SubstationCurrent.AsynchronousMachines[0].State)
                {
                    if (PumpImage.Contains("rotate") || Pump1Image.Contains("rotate"))
                    {
                        PumpImage = "Assets/pump-on.png";
                        Pump1Image = "Assets/pump-on.png";
                    }
                    else
                    {
                        PumpImage = "Assets/pump-on-rotate.png";
                        Pump1Image = "Assets/pump-on-rotate.png";
                    }
                }
                if (SubstationCurrent.AsynchronousMachines.Count > 1)
                {
                    if (SubstationCurrent.AsynchronousMachines[1].State)
                    {
                        if (Pump2Image.Contains("rotate"))
                            Pump2Image = "Assets/pump-on.png";
                        else
                            Pump2Image = "Assets/pump-on-rotate.png";
                    }
                }
            }
        }

        public void OpenCommandWindow(string window)
        {
            if (SubstationCurrent != null)
            {
                CommandingWindow commandingWindow = new CommandingWindow();

                CommandingWindowViewModel commandingWindowViewModel = new CommandingWindowViewModel(SubstationCurrent);

                commandingWindow.DataContext = commandingWindowViewModel;

                commandingWindowViewModel.SetView(window);

                commandingWindow.Show();
            }
        }

        public void DisconectorOperation(string type)
        {
            if (SubstationCurrent != null && SubstationCurrent.Disconectors != null)
            {
                switch (type)
                {
                    case "1":
                        if (SubstationCurrent.Disconectors[0].State == DiscreteState.ON)
                        {
                            SubstationCurrent.Disconectors[0].State = DiscreteState.OFF;
                            Disconector1Image = "Assets/resloser-off.png";
                            LineUpDis1 = LineDownDis1 = LineSecond = "#FFFF634D";
                            drawBreakerOff();

                            if (SubstationCurrent.Breakers.Count > 2)
                            {
                                DrawBreaker4Off();
                                DrawBreaker5Off();
                            }
                        }
                        else
                        {
                            SubstationCurrent.Disconectors[0].State = DiscreteState.ON;
                            Disconector1Image = "Assets/recloser-on.png";
                            LineUpDis1 = LineDownDis1 = LineSecond = "#FF7DFB4E";

                            if (SubstationCurrent.Breakers[0].State == DiscreteState.ON)
                            {
                                drawBreakerOn();
                                if (SubstationCurrent.Disconectors[1].State == DiscreteState.ON)
                                {
                                    drawDis2On();

                                    if (SubstationCurrent.Breakers[1].State == DiscreteState.ON)
                                    {
                                        DrawBreaker2On();
                                        DrawBreaker4On();
                                    }

                                    if (SubstationCurrent.Breakers.Count == 3)
                                        if (SubstationCurrent.Breakers[2].State == DiscreteState.ON)
                                            DrawBreaker5On();
                                }
                            }
                        }
                        break;
                    case "2":
                        if (SubstationCurrent.Disconectors[1].State == DiscreteState.ON)
                        {
                            SubstationCurrent.Disconectors[1].State = DiscreteState.OFF;
                            Disconector2Image = "Assets/resloser-off.png";
                                drawDis2Off();
                            if (SubstationCurrent.Breakers.Count > 2)
                            {
                                DrawBreaker4Off();
                                DrawBreaker5Off();
                            }
                        }
                        else
                        {
                            SubstationCurrent.Disconectors[1].State = DiscreteState.ON;
                            Disconector2Image = "Assets/recloser-on.png";

                            if (SubstationCurrent.Disconectors[0].State == DiscreteState.ON && 
                                SubstationCurrent.Breakers[0].State == DiscreteState.ON)
                            {
                                drawDis2On();
                                if (SubstationCurrent.Breakers[1].State == DiscreteState.ON)
                                {
                                    DrawBreaker2On();
                                    DrawBreaker4On();
                                }

                                if (SubstationCurrent.Breakers.Count == 3)
                                    if (SubstationCurrent.Breakers[2].State == DiscreteState.ON)
                                        DrawBreaker5On();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        } 

        public void BreakerOperation(string type)
        {
            if (SubstationCurrent != null && SubstationCurrent.Breakers != null)
            {
                switch (type)
                {
                    case "1":
                        {
                            if (SubstationCurrent.Breakers[0].State == DiscreteState.OFF)
                            {
                                SubstationCurrent.Breakers[0].State = DiscreteState.ON;
                                BreakerImage = "Assets/breaker-on.png";
                                if (substationCurrent.Disconectors[0].State == DiscreteState.ON)
                                    drawBreakerOn();

                                if (SubstationCurrent.Breakers.Count > 2)
                                {
                                    if (SubstationCurrent.Disconectors[1].State == DiscreteState.ON)
                                    {
                                        if (SubstationCurrent.Breakers[1].State == DiscreteState.ON)
                                            DrawBreaker4On();

                                        if (SubstationCurrent.Breakers[2].State == DiscreteState.ON)
                                            DrawBreaker5On();
                                    }
                                }
                            }
                            else
                            {
                                SubstationCurrent.Breakers[0].State = DiscreteState.OFF;
                                BreakerImage = "Assets/breaker-off.png";
                                drawBreakerOff();

                                if (SubstationCurrent.Breakers.Count > 2)
                                {
                                    DrawBreaker4Off();
                                    DrawBreaker5Off();
                                }
                            }
                            break;
                        }
                    case "2":
                        {
                            if (SubstationCurrent.Breakers[1].State == DiscreteState.OFF)
                            {
                                SubstationCurrent.Breakers[1].State = DiscreteState.ON;
                                BreakerPumpOneImage = "Assets/breaker-on.png";
                                if (substationCurrent.Disconectors[0].State == DiscreteState.ON &&
                                    substationCurrent.Breakers[0].State == DiscreteState.ON &&
                                    substationCurrent.Disconectors[1].State == DiscreteState.ON)
                                    DrawBreaker2On();
                            }
                            else
                            {
                                SubstationCurrent.Breakers[1].State = DiscreteState.OFF;
                                BreakerPumpOneImage = "Assets/breaker-off.png";
                                DrawBreaker2Off();
                            }
                            break;
                        }
                    case "4":
                        {
                            if (SubstationCurrent.Breakers[1].State == DiscreteState.OFF)
                            {
                                SubstationCurrent.Breakers[1].State = DiscreteState.ON;
                                Breaker_PM1Image = "Assets/breaker-on.png";

                                if (substationCurrent.Disconectors[0].State == DiscreteState.ON &&
                                    substationCurrent.Breakers[0].State == DiscreteState.ON &&
                                    substationCurrent.Disconectors[1].State == DiscreteState.ON)
                                    DrawBreaker4On();
                            }
                            else
                            {
                                SubstationCurrent.Breakers[1].State = DiscreteState.OFF;
                                Breaker_PM1Image = "Assets/breaker-off.png";
                                DrawBreaker4Off();
                            }
                            break;
                        }
                    case "5":
                        {
                            if (SubstationCurrent.Breakers[2].State == DiscreteState.OFF)
                            {
                                SubstationCurrent.Breakers[2].State = DiscreteState.ON;
                                Breaker_PM2Image = "Assets/breaker-on.png";

                                if (substationCurrent.Disconectors[0].State == DiscreteState.ON &&
                                    substationCurrent.Breakers[0].State == DiscreteState.ON &&
                                    substationCurrent.Disconectors[1].State == DiscreteState.ON)
                                    DrawBreaker5On();
                            }
                            else
                            {
                                SubstationCurrent.Breakers[2].State = DiscreteState.OFF;
                                Breaker_PM2Image = "Assets/breaker-off.png";
                                DrawBreaker5Off();
                            }
                            break;
                        }
                    default:
                        break;

                }
            }
        }

        public void TapChangerOperation()
        {
        }

        #region Coloring lines
        private void drawBreakerOn()
        {
            LineUpBreaker = LineDownBreaker = LineThird = "#FF7DFB4E";
        }
        private void drawDis2On()
        {
            LineUpDis2 = LineDownDis2 = LineFourth = LineUpPT = LineDownPT = "#FF7DFB4E";
            PTImage = "Assets/transformator-on.png";
        }
        public void DrawBreaker2On()
        {
            LineUpBreaker2 = LineDownBreaker2 = LineUpPumpOne = "#FF7DFB4E";
            DrawPumpOn();
        }
        public void DrawBreaker4On()
        {
            LineUpFirstPump = LineUpBreakerFirstPump = LineUpBreakerFirstPumpCN = "#FF7DFB4E";
            SubstationCurrent.AsynchronousMachines[0].State = true;
            Pump1Image = "Assets/pump-on.png";
        }
        public void DrawBreaker5On()
        {
            LineUpSecondPump = LineUpBreakerSecondPump = LineUpBreakerSecondPumpCN = "#FF7DFB4E";
            SubstationCurrent.AsynchronousMachines[1].State = true;
            Pump2Image = "Assets/pump-on.png";
        }
        private void DrawPumpOn()
        {
            LineUpMultiPump = LineUpFirstPump = LineUpSecondPump = "#FF7DFB4E";
            if (SubstationCurrent.AsynchronousMachines.Count == 1)
            {
                PumpImage = "Assets/pump-on.png";
                SubstationCurrent.AsynchronousMachines[0].State = true;
            }
        }

        private void drawBreakerOff()
        {
            LineDownBreaker = LineUpBreaker = LineThird = "#FFFF634D";
            drawDis2Off();
        }
        private void drawDis2Off()
        {
            LineUpDis2 = LineDownDis2 = LineFourth = LineUpPT = LineDownPT = "#FFFF634D";
            PTImage = "Assets/transformator-off.png";
            DrawBreaker2Off();
        }
        public void DrawBreaker2Off()
        {
            LineUpBreaker2 = LineDownBreaker2 = LineUpPumpOne = "#FFFF634D";
            DrawPumpOff();
        }
        public void DrawBreaker4Off()
        {
            LineUpFirstPump = LineUpBreakerFirstPump = LineUpBreakerFirstPumpCN = "#FFFF634D";
            SubstationCurrent.AsynchronousMachines[0].State = false;
            Pump1Image = "Assets/pump-off.png";
        }
        public void DrawBreaker5Off()
        {
            LineUpSecondPump = LineUpBreakerSecondPump = LineUpBreakerSecondPumpCN = "#FFFF634D";
            SubstationCurrent.AsynchronousMachines[1].State = false;
            Pump2Image = "Assets/pump-off.png";
        }
        private void DrawPumpOff()
        {
            LineUpMultiPump = LineUpFirstPump = LineUpSecondPump = "#FFFF634D";
            if (SubstationCurrent.AsynchronousMachines.Count == 1)
            {
                PumpImage = "Assets/pump-off.png";
                SubstationCurrent.AsynchronousMachines[0].State = false;
            }
        }
        #endregion
    }
}
