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
		private string strujaW1 = "0 A";
		private string strujaW2 = "0 A";
		private string naponW1 = "0 A";
		private string naponW2 = "0 A";
		private string struja2W1 = "0 A";
		private string struja2W2 = "0 A";
		private string napon2W1 = "0 A";
		private string napon2W2 = "0 A";
		private string sub1Visibility = "Visible";
		private string sub2Visibility = "Hidden";
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

		public string StrujaW1
		{
			get
			{
				return strujaW1;
			}
			set
			{
				strujaW1 = value;
				OnPropertyChanged(nameof(StrujaW1));
			}
		}

		public string Struja2W1
		{
			get
			{
				return struja2W1;
			}
			set
			{
				struja2W1 = value;
				OnPropertyChanged(nameof(Struja2W1));
			}
		}

		public string StrujaW2
		{
			get
			{
				return strujaW2;
			}
			set
			{
				strujaW2 = value;
				OnPropertyChanged(nameof(StrujaW2));
			}
		}

		public string Struja2W2
		{
			get
			{
				return struja2W2;
			}
			set
			{
				struja2W2 = value;
				OnPropertyChanged(nameof(Struja2W2));
			}
		}

		public string NaponW1
		{
			get
			{
				return naponW1;
			}
			set
			{
				naponW1 = value;
				OnPropertyChanged(nameof(NaponW1));
			}
		}

		public string Napon2W1
		{
			get
			{
				return napon2W1;
			}
			set
			{
				napon2W1 = value;
				OnPropertyChanged(nameof(Napon2W1));
			}
		}

		public string NaponW2
		{
			get
			{
				return naponW2;
			}
			set
			{
				naponW2 = value;
				OnPropertyChanged(nameof(NaponW2));
			}
		}

		public string Napon2W2
		{
			get
			{
				return napon2W2;
			}
			set
			{
				napon2W2 = value;
				OnPropertyChanged(nameof(Napon2W2));
			}
		}

		public string Sub2Visibility
		{
			get
			{
				return sub2Visibility;
			}
			set
			{
				sub2Visibility = value;
				if(sub2Visibility == "Hidden")
				{
					sub1Visibility = "Visible";
				}
				else
				{
					sub1Visibility = "Hidden";
				}
				OnPropertyChanged(nameof(Sub2Visibility));
			}
		}

		public string Sub1Visibility
		{
			get
			{
				return sub1Visibility;
			}
			set
			{
				sub1Visibility = value;

				if (sub1Visibility == "Hidden")
				{
					sub2Visibility = "Visible";
				}
				else
				{
					sub2Visibility = "Hidden";
				}

				OnPropertyChanged(nameof(Sub1Visibility));
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

            Messenger.Default.Register<NotificationMessage>(this, (message) => 
            {
                if(message.Notification != "scada" && message.Notification != "nms" && message.Notification != "commandTransformer")
                    ChangeStatesOfElements(message.Notification, message.Target);
            });

            SubstationCurrent = new Substation();
        }

        public void UpdateSubstationModel(Substation substation)
        {
            SubstationCurrent = substation;
            
            LeaveStatesAfterChangingSubstations();

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
                SubstationCurrent.Breakers[0].NewState = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[0].State = SubstationCurrent.Breakers[0].NewState;
                BreakerOperation("1");

                Messenger.Default.Send(new NotificationMessage(element, target, "UpdateTelemetry"));
            }
            else if (string.Compare(element, "Breaker2") == 0)
            {
                SubstationCurrent.Breakers[1].NewState = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[1].State = SubstationCurrent.Breakers[1].NewState;
                BreakerOperation("2");

                Messenger.Default.Send(new NotificationMessage(element, target, "UpdateTelemetry"));

            }
            else if (string.Compare(element, "Breaker3") == 0)
            {
                SubstationCurrent.Breakers[0].NewState = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[0].State = SubstationCurrent.Breakers[0].NewState;
                BreakerOperation("1");

                Messenger.Default.Send(new NotificationMessage(element, target, "UpdateTelemetry"));

            }
            else if (string.Compare(element, "Breaker4") == 0)
            {
                SubstationCurrent.Breakers[1].NewState = ((Breaker)target).NewState;
                SubstationCurrent.Breakers[1].State = SubstationCurrent.Breakers[1].NewState;
                BreakerOperation("4");

                Messenger.Default.Send(new NotificationMessage(element, target, "UpdateTelemetry"));

            }
            else if (string.Compare(element, "Breaker5") == 0)
            {
                if (SubstationCurrent.Breakers.Count > 2)
                {
                    SubstationCurrent.Breakers[2].NewState = ((Breaker)target).NewState;
                    SubstationCurrent.Breakers[2].State = SubstationCurrent.Breakers[2].NewState;
                    BreakerOperation("5");

                    Messenger.Default.Send(new NotificationMessage(element, target, "UpdateTelemetry"));

                }
            }
            else if (string.Compare(element, "Disconector1") == 0)
            {
                SubstationCurrent.Disconectors[0].NewState = ((Disconector)target).NewState;
                SubstationCurrent.Disconectors[0].State = SubstationCurrent.Disconectors[0].NewState;
                DisconectorOperation("1");

                Messenger.Default.Send(new NotificationMessage(element, target, "UpdateTelemetry"));

            }
            else if (string.Compare(element, "Disconector2") == 0)
            {
                SubstationCurrent.Disconectors[1].NewState = ((Disconector)target).NewState;
                SubstationCurrent.Disconectors[1].State = SubstationCurrent.Disconectors[1].NewState;
                DisconectorOperation("2");

                Messenger.Default.Send(new NotificationMessage(element, target, "UpdateTelemetry"));

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
                    v.State = false;
                foreach (var v in SubstationCurrent.Breakers)
                    v.State = DiscreteState.OFF;
                foreach (var v in SubstationCurrent.Disconectors)
                    v.State = DiscreteState.OFF;
            }
        }

        public void populateUI()
        {
            //Green color: #FF7DFB4E  -- #FF29BF30 -- ON
            //Red color: #FFFF634D  -- Blue #FF29A2B5 -- OFF
            lineFirst = lineSecond = lineThird = lineUpDis1 = lineDownDis1 = lineUpBreaker = lineDownBreaker = lineUpDis2 =
            lineDownDis2 = lineUpPT = lineDownPT = lineUpBreaker2 = lineFourth = lineStart = lineUpFirstPump = lineUpMultiPump =
            lineUpSecondPump = lineUpPumpOne = lineUpBreakerFirstPump = lineUpBreakerSecondPump = lineDownBreaker2 =
            lineUpBreakerSecondPumpCN = lineUpBreakerFirstPumpCN = "#FF29A2B5";
            Two_AM_Visible = "Hidden";
            breakerImage = breakerPumpOneImage = breaker_PM1Image = breaker_PM2Image = "Assets/breaker-off1.png";
            disconector1Image = disconector2Image = "Assets/resloser-off1.png";
            ptImage = "Assets/transformator-off1.png";
            PumpImage = "Assets/pump-off1.png"; // "Assets/pump-on-rotate1.png";
            Pump1Image = "Assets/pump-off1.png";
            Pump2Image = "Assets/pump-off1.png";
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (SubstationCurrent != null && SubstationCurrent.AsynchronousMachines != null)
            {
                if (SubstationCurrent.AsynchronousMachines[0].State)
                {
                    if (PumpImage.Contains("rotate") || Pump1Image.Contains("rotate"))
                    {
                        PumpImage = "Assets/pump-on1.png";
                        Pump1Image = "Assets/pump-on1.png";
                    }
                    else
                    {
                        PumpImage = "Assets/pump-on-rotate1.png";
                        Pump1Image = "Assets/pump-on-rotate1.png";
                    }
                }
                if (SubstationCurrent.AsynchronousMachines.Count > 1)
                {
                    if (SubstationCurrent.AsynchronousMachines[1].State)
                    {
                        if (Pump2Image.Contains("rotate"))
                            Pump2Image = "Assets/pump-on1.png";
                        else
                            Pump2Image = "Assets/pump-on-rotate1.png";
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
                            SetFirstDisconectorOn();
                        }
                        else
                        {
                            SetFirstDisconectorOff();
                        }
                        break;
                    case "2":
                        if (SubstationCurrent.Disconectors[1].State == DiscreteState.ON)
                        {
                            SetSecondDisconectorOn();
                        }
                        else
                        {
                            SetSecondDisconectorOff();
                        }
                        break;
                    default:
                        break;
                }
            }
        } 

        public void SetFirstDisconectorOff()
        {
            SubstationCurrent.Disconectors[0].State = DiscreteState.OFF;
            Disconector1Image = "Assets/resloser-off1.png";
            LineUpDis1 = LineDownDis1 = LineSecond = "#FF29A2B5";
            drawBreakerOff();

            if (SubstationCurrent.Breakers.Count > 2)
            {
                DrawBreaker4Off();
                DrawBreaker5Off();
            }
        }

        public void SetFirstDisconectorOn()
        {
            SubstationCurrent.Disconectors[0].State = DiscreteState.ON;
            Disconector1Image = "Assets/recloser-on1.png";
            LineUpDis1 = LineDownDis1 = LineSecond = "#FF29BF30";

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

        public void SetSecondDisconectorOff()
        {
            SubstationCurrent.Disconectors[1].State = DiscreteState.OFF;
            Disconector2Image = "Assets/resloser-off1.png";
            drawDis2Off();
            if (SubstationCurrent.Breakers.Count > 2)
            {
                DrawBreaker4Off();
                DrawBreaker5Off();
            }
        }

        public void SetSecondDisconectorOn()
        {
            SubstationCurrent.Disconectors[1].State = DiscreteState.ON;
            Disconector2Image = "Assets/recloser-on1.png";

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

        public void LeaveStatesAfterChangingSubstations()
        {
            if (SubstationCurrent != null && SubstationCurrent.Disconectors != null && SubstationCurrent.Breakers != null)
            {
                if (SubstationCurrent.Disconectors[0].State == DiscreteState.OFF)
                {
                    SetFirstDisconectorOff();
                }
                else
                {
                    SetFirstDisconectorOn();
                }
                if (SubstationCurrent.Disconectors[1].State == DiscreteState.OFF)
                {
                    SetSecondDisconectorOff();
                }
                else
                {
                    SetSecondDisconectorOn();
                }

                if (SubstationCurrent.Breakers[0].State == DiscreteState.OFF)
                {
                    SetFirstBreakerOff();
                }
                else
                {
                    SetFirstBreakerOn();
                }
                if (SubstationCurrent.Breakers.Count == 2)
                {
                    if (SubstationCurrent.Breakers[1].State == DiscreteState.OFF)
                    {
                        SetSecondBreakerOff();
                    }
                    else
                    {
                        SetSecondBreakerOn();
                    }
                }
                else
                {
                    if (SubstationCurrent.Breakers[1].State == DiscreteState.OFF)
                    {
                        SetFourthBreakerOff();
                    }
                    else
                    {
                        SetFourthBreakerOn();
                    }
                    if (SubstationCurrent.Breakers[2].State == DiscreteState.OFF)
                    {
                        SetFifthBreakerOff();
                    }
                    else
                    {
                        SetFifthBreakerOn();
                    }
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
                                SetFirstBreakerOff();
                            }
                            else
                            {
                                SetFirstBreakerOn();
                            }
                            break;
                        }
                    case "2":
                        {
                            if (SubstationCurrent.Breakers[1].State == DiscreteState.OFF)
                            {
                                SetSecondBreakerOff();
                            }
                            else
                            {
                                SetSecondBreakerOn();
                            }
                            break;
                        }
                    case "4":
                        {
                            if (SubstationCurrent.Breakers[1].State == DiscreteState.OFF)
                            {
                                SetFourthBreakerOff();
                            }
                            else
                            {
                                SetFourthBreakerOn();
                            }
                            break;
                        }
                    case "5":
                        {
                            if (SubstationCurrent.Breakers[2].State == DiscreteState.OFF)
                            {
                                SetFifthBreakerOff();
                            }
                            else
                            {
                                SetFifthBreakerOn();
                            }
                            break;
                        }
                    default:
                        break;

                }
            }
        }

        public void SetFirstBreakerOn()
        {
            SubstationCurrent.Breakers[0].State = DiscreteState.ON;
            BreakerImage = "Assets/breaker-on1.png";
            if (substationCurrent.Disconectors[0].State == DiscreteState.ON)
            {
                drawBreakerOn();

                if (substationCurrent.Disconectors[1].State == DiscreteState.ON)
                {
                    drawDis2On();

                    if (substationCurrent.Breakers[1].State == DiscreteState.ON)
                        DrawBreaker2On();

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
            }
        }

        public void SetFirstBreakerOff()
        {
            SubstationCurrent.Breakers[0].State = DiscreteState.OFF;
            BreakerImage = "Assets/breaker-off1.png";
            drawBreakerOff();

            if (SubstationCurrent.Breakers.Count > 2)
            {
                DrawBreaker4Off();
                DrawBreaker5Off();
            }
        }

        public void SetSecondBreakerOn()
        {
            SubstationCurrent.Breakers[1].State = DiscreteState.ON;
            BreakerPumpOneImage = "Assets/breaker-on1.png";
            if (substationCurrent.Disconectors[0].State == DiscreteState.ON &&
                substationCurrent.Breakers[0].State == DiscreteState.ON &&
                substationCurrent.Disconectors[1].State == DiscreteState.ON)
                DrawBreaker2On();
        }

        public void SetSecondBreakerOff()
        {
            SubstationCurrent.Breakers[1].State = DiscreteState.OFF;
            BreakerPumpOneImage = "Assets/breaker-off1.png";
            DrawBreaker2Off();
        }

        public void SetFourthBreakerOn()
        {
            SubstationCurrent.Breakers[1].State = DiscreteState.ON;
            Breaker_PM1Image = "Assets/breaker-on1.png";

            if (substationCurrent.Disconectors[0].State == DiscreteState.ON &&
                substationCurrent.Breakers[0].State == DiscreteState.ON &&
                substationCurrent.Disconectors[1].State == DiscreteState.ON)
                DrawBreaker4On();
        }

        public void SetFourthBreakerOff()
        {
            SubstationCurrent.Breakers[1].State = DiscreteState.OFF;
            Breaker_PM1Image = "Assets/breaker-off1.png";
            DrawBreaker4Off();
        }

        public void SetFifthBreakerOn()
        {
            SubstationCurrent.Breakers[2].State = DiscreteState.ON;
            Breaker_PM2Image = "Assets/breaker-on1.png";

            if (substationCurrent.Disconectors[0].State == DiscreteState.ON &&
                substationCurrent.Breakers[0].State == DiscreteState.ON &&
                substationCurrent.Disconectors[1].State == DiscreteState.ON)
                DrawBreaker5On();
        }

        public void SetFifthBreakerOff()
        {
            SubstationCurrent.Breakers[2].State = DiscreteState.OFF;
            Breaker_PM2Image = "Assets/breaker-off1.png";
            DrawBreaker5Off();
        }

        public void TapChangerOperation()
        {
        }

        #region Coloring lines
        private void drawBreakerOn()
        {
            LineUpBreaker = LineDownBreaker = LineThird = "#FF29BF30";
        }
        private void drawDis2On()
        {
            LineUpDis2 = LineDownDis2 = LineFourth = LineUpPT = LineDownPT = "#FF29BF30";
            PTImage = "Assets/transformator-on1.png";
        }
        public void DrawBreaker2On()
        {
            LineUpBreaker2 = LineDownBreaker2 = LineUpPumpOne = "#FF29BF30";
            DrawPumpOn();
        }
        public void DrawBreaker4On()
        {
            LineUpFirstPump = LineUpBreakerFirstPump = LineUpBreakerFirstPumpCN = "#FF29BF30";
            SubstationCurrent.AsynchronousMachines[0].State = true;
            Pump1Image = "Assets/pump-on1.png";
        }
        public void DrawBreaker5On()
        {
            LineUpSecondPump = LineUpBreakerSecondPump = LineUpBreakerSecondPumpCN = "#FF29BF30";
            SubstationCurrent.AsynchronousMachines[1].State = true;
            Pump2Image = "Assets/pump-on1.png";
        }
        private void DrawPumpOn()
        {
            LineUpMultiPump = LineUpFirstPump = LineUpSecondPump = "#FF29BF30";
            if (SubstationCurrent.AsynchronousMachines.Count == 1)
            {
                PumpImage = "Assets/pump-on1.png";
                SubstationCurrent.AsynchronousMachines[0].State = true;
            }
        }

        private void drawBreakerOff()
        {
            LineDownBreaker = LineUpBreaker = LineThird = "#FF29A2B5";
            drawDis2Off();
        }
        private void drawDis2Off()
        {
            LineUpDis2 = LineDownDis2 = LineFourth = LineUpPT = LineDownPT = "#FF29A2B5";
            PTImage = "Assets/transformator-off1.png";
            DrawBreaker2Off();
        }
        public void DrawBreaker2Off()
        {
            LineUpBreaker2 = LineDownBreaker2 = LineUpPumpOne = "#FF29A2B5";
            DrawPumpOff();
        }
        public void DrawBreaker4Off()
        {
            LineUpFirstPump = LineUpBreakerFirstPump = LineUpBreakerFirstPumpCN = "#FF29A2B5";
            SubstationCurrent.AsynchronousMachines[0].State = false;
            Pump1Image = "Assets/pump-off1.png";
        }
        public void DrawBreaker5Off()
        {
            LineUpSecondPump = LineUpBreakerSecondPump = LineUpBreakerSecondPumpCN = "#FF29A2B5";
            SubstationCurrent.AsynchronousMachines[1].State = false;
            Pump2Image = "Assets/pump-off1.png";
        }
        private void DrawPumpOff()
        {
            LineUpMultiPump = LineUpFirstPump = LineUpSecondPump = "#FF29A2B5";
            if (SubstationCurrent.AsynchronousMachines.Count == 1)
            {
                PumpImage = "Assets/pump-off1.png";
                SubstationCurrent.AsynchronousMachines[0].State = false;
            }
        }
        #endregion
    }
}
