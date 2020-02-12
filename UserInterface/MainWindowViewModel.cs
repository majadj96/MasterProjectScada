using Common;
using Common.GDA;
using GalaSoft.MvvmLight.Messaging;
using PubSubCommon;
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
using UserInterface.Model;
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
            Messenger.Default.Register<NotificationMessage>(this, (message) => {PopulateModel(message.Target); });
            CommandDis1 = CommandDis2 = new DisconectorCommand(this);
            CommandBreaker = new BreakerCommand(this);
            CommandPT = new TapChangerCommand(this);
            CommandOpenCommand = new CommandOpenCommand(this);
        }

        public void setUpLayout()
        {
            // window.WindowState = WindowState.Maximized;
            // window.WindowStyle = WindowStyle.None;
        }

        public void setUpInitState()
        {        
            populateUI();
            setState();
            connectedStatusBar = "Dissconnected"; //SCADA konekcija
            timeStampStatusBar = DateTime.Now.ToLongDateString();  //SCADA konekcija
        }

        public void setState()
        {
            breakerState = disconector1State = disconector2State =     
            ptState = pump1State = pump2State = pumpState = true;
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
            pumpImage = pump1Image = pump2Image = "Assets/pump-on.png";
        }

        //flags for elements state
        private bool breakerState { get; set; }
        private bool disconector1State { get; set; }
        private bool disconector2State { get; set; }
        private bool ptState { get; set; }
        private bool pumpState { get; set; }
        private bool pump1State { get; set; }
        private bool pump2State { get; set; }
        private string disc1Id { get; set; }
        private string disc2Id { get; set; }
        private string breakerId { get; set; }

        #region Properties
        //Lines
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
        
        public Window Window { get; set; }
        public string statistics { get; set; }
        public string pubSub { get; set; }
        public string connectedStatusBar { get; set; }
        public string timeStampStatusBar { get; set; }

        //Depends on model - one or two async machine
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

        //Elements on EEM
        #region Elements_Of_Mesh
        //Elements
        public string disconector1Image { get; set; }
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
        public string disconector2Image { get; set; }
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
        public string breakerImage { get; set; }
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
        public string pumpImage { get; set; }
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
        public string pump1Image { get; set; }
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
        public string pump2Image { get; set; }
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
        public string ptImage { get; set; }
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
        
        //Lines properties
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

        public BindingList<UIModel> substationItems { get; set; }

        public BindingList<UIModel> SubstationItems
        {
            get
            {
                return substationItems;
            }
            set
            {
                substationItems = value;
                OnPropertyChanged("SubstationItems");
            }
        }

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

        #region Commands
        
        public ICommand CommandOpenCommand
        {
            get;
            private set;
        }
        public ICommand CommandDis1
        {
            get;
            private set;
        }
        public ICommand CommandDis2
        {
            get;
            private set;
        }
        public ICommand CommandBreaker
        {
            get;
            private set;
        }
        public ICommand CommandPT
        {
            get;
            private set;
        }
        #endregion

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
                    } else
                    {
                        disconector1State = true;
                        Disconector1Image = "Assets/recloser-on.png";
                        LineUpDis1 = LineDownDis1 = LineSecond = "#FF7DFB4E";

                        if (breakerState)
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
                        if (disconector1State && breakerState)
                        drawDis2On();
                    }
                    break;
                default:
                    break;
            }
        }
        
        public void BreakerOperation()
        {
            if (!breakerState)
            {
                breakerState = true;
                BreakerImage = "Assets/breaker-on.png";
                drawBreakerOn();
            }
            else
            {
                breakerState = false;
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
        }
        private void drawDis2Off()
        {
            LineUpDis2 = LineDownDis2 = LineFourth = LineUpPT = LineDownPT = LineUpPump = LineUpMultiPump = LineUpFirstPump = LineUpSecondPump = "#FFFF634D";
            PTImage = "Assets/transformator-off.png";
            PumpImage = Pump1Image = Pump2Image = "Assets/pump-off.png";
        }
        private void drawBreakerOff()
        {
            LineDownBreaker = LineUpBreaker = LineThird = "#FFFF634D";
            drawDis2Off();

        }
        #endregion

        public void OpenCommandWindow()
        {
            View.Command commandWindow = new View.Command();
            commandWindow.Show();
        }

        public void PopulateModel(object resources)
        {
            NMSModel nMSModel = (NMSModel)resources;
            SubstationItems = toUIModelList(nMSModel.ResourceDescs);
        }

        public BindingList<UIModel> toUIModelList(List<ResourceDescription> resources)
        {
            BindingList<UIModel> response = new BindingList<UIModel>();
            int disconectors = 1;

            foreach (ResourceDescription resource in resources.Where(x => (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.DISCONNECTOR) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.BREAKER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.RATIOTAPCHANGER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.ASYNCHRONOUSMACHINE))) 
            {
                UIModel model = new UIModel();

                foreach (Property property in resource.Properties)
                {
                    switch (property.Id)
                    {
                        case Common.ModelCode.IDOBJ_GID:
                            if (ModelCodeHelper.ExtractTypeFromGlobalId(resource.Id) == (short)DMSType.DISCONNECTOR)
                            {
                                if(disconectors == 1)
                                {
                                    Disc1Id = property.GetValue().ToString();
                                    disconectors++;
                                } else if (disconectors == 2)
                                {
                                    Disc2Id = property.GetValue().ToString();
                                    disconectors = 0;
                                }
                            } else if (ModelCodeHelper.ExtractTypeFromGlobalId(resource.Id) == (short)DMSType.BREAKER)
                            {
                                BreakerId = property.GetValue().ToString();
                            }
                                model.GID = property.GetValue().ToString();
                            break;
                        case Common.ModelCode.IDOBJ_DESC:
                            model.Description = property.GetValue().ToString();
                            break;
                        case Common.ModelCode.IDOBJ_MRID:
                            model.MRID = property.GetValue().ToString();
                            break;
                        case Common.ModelCode.IDOBJ_NAME:
                            model.Name = property.GetValue().ToString();
                            break;
                        
                        case Common.ModelCode.ASYNCMACHINE_COSPHI:
                            model.Value = property.GetValue().ToString();
                            break;
                        case Common.ModelCode.ASYNCMACHINE_RATEDP:
                            break;
                        case Common.ModelCode.TAPCHANGER_HIGHSTEP:
                            break;
                        case Common.ModelCode.TAPCHANGER_LOWSTEP:
                            break;
                        case Common.ModelCode.TAPCHANGER_NORMALSTEP:
                            model.Value = property.GetValue().ToString();
                            break;
                        case Common.ModelCode.MEASUREMENT_DIRECTION:
                            break;
                        case Common.ModelCode.MEASUREMENT_MEASTYPE:
                            break;
                        case Common.ModelCode.ANALOG_MAXVALUE:
                            break;
                        case Common.ModelCode.ANALOG_MINVALUE:
                            break;
                        case Common.ModelCode.ANALOG_NORMALVALUE:
                            model.Value = property.GetValue().ToString();
                            break;
                        case Common.ModelCode.DISCRETE_MAXVALUE:
                            break;
                        case Common.ModelCode.DISCRETE_MINVALUE:
                            break;
                        case Common.ModelCode.DISCRETE_NORMALVALUE:
                            model.Value = property.GetValue().ToString();
                            break;
                        default:
                            break;
                    }
                }
                response.Add(model);
            }
            return response;
        }
    }
}
