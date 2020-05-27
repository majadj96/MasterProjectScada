using Common;
using Common.AlarmEvent;
using Common.GDA;
using GalaSoft.MvvmLight.Messaging;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Converters;
using UserInterface.Model;
using UserInterface.ProxyPool;
using UserInterface.Subscription;
using UserInterface.ViewModel;

namespace UserInterface
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> ButtonTablesCommand { get; private set; }
        public MyICommand<string> SearchSubsCommand { get; private set; }
        public MyICommand<string> DissmisSubsCommand { get; private set; }
        public MyICommand<string> AnalyticsOpenCommand { get; private set; }
        public MyICommand<string> LoadSubstationCommand { get; private set; }

        private MeshViewModel meshViewModel = new MeshViewModel();

        private ObservableCollection<RadioButton> radioButtons = new ObservableCollection<RadioButton>();
        private ObservableCollection<Substation> substationsList = new ObservableCollection<Substation>();
        private ObservableCollection<string> searchType = new ObservableCollection<string> { "Name", "GID" };
        List<Substation> searchedSubs = new List<Substation>();
        private AlarmHandler alarmHandler;

        private DispatcherTimer AlarmButtonTimer = new DispatcherTimer();
        //private Thread threadAlarms;

        #region Variables
        private BindableBase currentMeshViewModel;
        private BindableBase currentTableViewModel;

        private Dictionary<long, Substation> substations;
        private ObservableCollection<UIModel> substationItems = new ObservableCollection<UIModel>();

        private Substation substationCurrent;

        private Substation selectedSubstation;

        public string statistics { get; set; }
        public string pubSub { get; set; }
        public string connectedStatusBar { get; set; }
        public string timeStampStatusBar { get; set; }
        public string gaugeValue { get; set; }
        public string anguarValue { get; set; }
        public string gaugeClasic1 { get; set; }
        public string gaugeClasic2 { get; set; }
        public string searchTerm { get; set; }
        public string searchTypeSelected { get; set; }
        private string transformerCurrent;
        private string transformerVoltage;
        private string transformerTapChanger;
        public string gaugeClasicOpacity;
        private string gridElectricity;
        private string gridPower;
        private string waterPresureWidget;

        private bool BlinkOnFlag = false, FlagToStartBlinking = false;
        private bool meshVisible = true;
        private SolidColorBrush buttonAlarmBrush = Brushes.Crimson;
        #endregion

        #region Props
        public string GaugeClasicOpacity
        {
            get { return gaugeClasicOpacity; }
            set { gaugeClasicOpacity = value; OnPropertyChanged("GaugeClasicOpacity"); }
        }
        public string GridPower
        {
            get { return gridPower; }
            set { gridPower = value; OnPropertyChanged("GridPower"); }
        }

        public string GridElectricity
        {
            get { return gridElectricity; }
            set { gridElectricity = value; OnPropertyChanged("GridElectricity"); }
        }

        public string WaterPresureWidget
        {
            get { return waterPresureWidget; }
            set { waterPresureWidget = value; OnPropertyChanged("WaterPresureWidget"); }
        }

        public BindableBase CurrentMeshViewModel
        {
            get { return currentMeshViewModel; }
            set { SetProperty(ref currentMeshViewModel, value); }
        }
        public BindableBase CurrentTableViewModel
        {
            get { return currentTableViewModel; }
            set { SetProperty(ref currentTableViewModel, value); }
        }

        public Substation SubstationCurrent
        {
            get { return substationCurrent; }
            set { substationCurrent = value; OnPropertyChanged("SubstationCurrent"); }
        }

        public Dictionary<long, Substation> Substations
        {
            get
            {
                return substations;
            }
            set
            {
                substations = value;
                OnPropertyChanged("Substations");
            }
        }
        public ObservableCollection<UIModel> SubstationItems
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
        public ObservableCollection<Substation> SubstationsList
        {
            get
            {
                return substationsList;
            }
            set
            {
                substationsList = value;
                OnPropertyChanged("SubstationsList");
            }
        }
        public ObservableCollection<string> SearchType
        {
            get
            {
                return searchType;
            }
            set
            {
                searchType = value;
                OnPropertyChanged("SearchType");
            }
        }
        public ObservableCollection<RadioButton> RadioButtons
        {
            get
            {
                return radioButtons;
            }
            set
            {
                radioButtons = value;
                OnPropertyChanged("RadioButtons");
            }
        }

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
        public Substation SelectedSubstation
        {
            get
            {
                return selectedSubstation;
            }
            set
            {
                selectedSubstation = value;
                OnPropertyChanged("SelectedSubstation");
            }
        }
        public string GaugeClasic1
        {
            get
            {
                return gaugeClasic1;
            }
            set
            {
                gaugeClasic1 = value;
                OnPropertyChanged("GaugeClasic1");
            }
        }
        public string GaugeClasic2
        {
            get
            {
                return gaugeClasic2;
            }
            set
            {
                gaugeClasic2 = value;
                OnPropertyChanged("GaugeClasic2");
            }
        }
        public string SearchTypeSelected
        {
            get
            {
                return searchTypeSelected;
            }
            set
            {
                searchTypeSelected = value;
                OnPropertyChanged("SearchTypeSelected");
            }
        }
        public string SearchTerm
        {
            get
            {
                return searchTerm;
            }
            set
            {
                searchTerm = value;
                OnPropertyChanged("SearchTerm");
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
        public string GaugeValue
        {
            get
            {
                return gaugeValue;
            }
            set
            {
                gaugeValue = value;
                OnPropertyChanged("GaugeValue");
            }
        }
        public string AnguarValue
        {
            get
            {
                return anguarValue;
            }
            set
            {
                anguarValue = value;
                OnPropertyChanged("AnguarValue");
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
        public string TransformerCurrent
        {
            get { return transformerCurrent; }
            set { transformerCurrent = value; OnPropertyChanged("TransformerCurrent"); }
        }
        public string TransformerVoltage
        {
            get { return transformerVoltage; }
            set { transformerVoltage = value; OnPropertyChanged("TransformerVoltage"); }
        }
        public string TransformerTapChanger
        {
            get { return transformerTapChanger; }
            set { transformerTapChanger = value; OnPropertyChanged("TransformerTapChanger"); }
        }


        public SolidColorBrush ButtonAlarmBrush
        {
            get
            {
                return buttonAlarmBrush;
            }
            set
            {
                buttonAlarmBrush = value;
                OnPropertyChanged("ButtonAlarmBrush");
            }
        }
        public bool MeshVisible
        {
            get
            {
                return meshVisible;
            }
            set
            {
                meshVisible = value;
                OnPropertyChanged("MeshVisible");
            }
        }
        #endregion

        DispatcherTimer timer = new DispatcherTimer();
        Random rand = new Random();

        public MainWindowViewModel()
        {
            CurrentMeshViewModel = meshViewModel;

            ButtonTablesCommand = new MyICommand<string>(OnNavigation);
            LoadSubstationCommand = new MyICommand<string>(changeGrid);
            SearchSubsCommand = new MyICommand<string>(searchSubstation);
            DissmisSubsCommand = new MyICommand<string>(dissmisSubstation);
            AnalyticsOpenCommand = new MyICommand<string>(openAnalytics);

            alarmHandler = new AlarmHandler();
            alarmHandler.Alarms = ProxyServices.AlarmEventServiceProxy.GetAllAlarms();

            Sub subNMS = new Sub();
            subNMS.OnSubscribe("nms");
            subNMS.OnSubscribe("scada");
            subNMS.OnSubscribe("alarm");
            setUpInitState();

            substations = new Dictionary<long, Substation>();

            timer.Tick += new EventHandler(Test);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            AlarmButtonTimer.Tick += ButtonBlinks;
            AlarmButtonTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            AlarmButtonTimer.Start();

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                if ((string)message.Sender == "commandTransformer")
                    CommandTransformerCurrentVoltage(message.Target, message.Notification);
                else
                    PopulateModel(message.Target, message.Notification);
            });

            GridElectricity = "-";
            GridPower = "-";
            GaugeClasicOpacity = "100";
        }

        private void changeGrid(string a)
        {
            SetCurrentSubstation();
            meshViewModel.UpdateSubstationModel(SubstationCurrent);
        }
        private void searchSubstation(string a)
        {
            Console.WriteLine(SearchTerm);
            Console.WriteLine(SearchTypeSelected);

            if (SearchTypeSelected == "Name")
            {
                searchedSubs = substations.Values.Where(x => x.Name.Contains(SearchTerm)).ToList();
                SubstationsList = new ObservableCollection<Substation>(searchedSubs);
            }
            else
            {
                searchedSubs = substations.Values.Where(x => x.Gid.Contains(SearchTerm)).ToList();
                SubstationsList = new ObservableCollection<Substation>(searchedSubs);
            }

        }

        private void openAnalytics(string a)
        {
            AnalyticsWindow analyticsWindow = new AnalyticsWindow();

            AnalyticsWindowViewModel analyticsWindowViewModel = new AnalyticsWindowViewModel(substations);

            analyticsWindow.DataContext = analyticsWindowViewModel;

            analyticsWindow.Show();
        }
        private void dissmisSubstation(string a)
        {
            SubstationsList = new ObservableCollection<Substation>( substations.Values.ToList());
            SearchTerm = "";
        }


        /*private void CheckForAlarms(object stateInfo)
        {
            while (true)
            {
                int numberOfAlarms = ProxyServices.AlarmEventServiceProxy.GetAllAlarms().Where(x => x.AlarmAck == false).Count();
                if (numberOfAlarms > 0)
                {
                    FlagToStartBlinking = true;
                }
                else
                {
                    FlagToStartBlinking = false;
                    ButtonAlarmBrush = Brushes.Crimson;
                    Thread.Sleep(1000);
                }
            }
        }*/

        private void Test(object sender, EventArgs e)
        {
            GaugeValue = rand.Next(0, 100).ToString();

            AnguarValue = rand.Next(-7, 7).ToString();

           // GaugeClasic = rand.Next(300, 1000).ToString();
        }

        private void ButtonBlinks(object sender, EventArgs e)
        {
            if (FlagToStartBlinking)
            {
                if (BlinkOnFlag)
                    ButtonAlarmBrush = Brushes.Black;
                else
                    ButtonAlarmBrush = Brushes.Crimson;

                BlinkOnFlag = !BlinkOnFlag;
            }
        }

        private void OnNavigation(string destination)
        {
            TablesWindow tablesWindow = new TablesWindow();

            TablesWindowViewModel tablesWindowViewModel = new TablesWindowViewModel(SubstationItems, this.alarmHandler, Substations.Values.ToList());

            tablesWindow.DataContext = tablesWindowViewModel;

            tablesWindowViewModel.SetView(destination);

            tablesWindow.Show();
        }

        public void FillSubstationItems()
        {
            int i = 0;
            foreach (var v in Substations.Values)
            {
                foreach (var vv in v.Disconectors)
                {
                    foreach (var c in SubstationItems)
                    {
                        if (c.GID == vv.GID)
                        {
                            c.Value = vv.State.ToString();
                            c.Time = vv.Time;
                            i++;
                        }
                    }
                }
                foreach (var vv in v.Breakers)
                {
                    foreach (var c in SubstationItems)
                    {
                        if (c.GID == vv.GID)
                        {
                            c.Value = vv.State.ToString();
                            c.Time = vv.Time;
                            i++;
                        }
                    }
                }
                if (SubstationItems[i].GID == v.TapChanger.GID)
                {
                    SubstationItems[i].Value = v.TapChanger.NormalStep.ToString();
                    SubstationItems[i].Time = v.TapChanger.Time;
                    i++;
                }
                foreach (var vv in v.AsynchronousMachines)
                {
                    if (SubstationItems[i].GID == vv.GID)
                    {
                        SubstationItems[i].Value = ConverterState.ConvertToDiscreteState(vv.State).ToString();
                        SubstationItems[i].Time = vv.Time;
                        i++;
                    }
                }
                if (SubstationItems[i].GID == v.Transformator.GID)
                {
                    SubstationItems[i].Time = v.Transformator.Time;
                    SubstationItems[i].Value = v.Transformator.TapChangerValue.ToString();
                    i++;
                }
            }
        }

        public void setUpInitState()
        {
            connectedStatusBar = "Dissconnected"; //SCADA konekcija
            timeStampStatusBar = DateTime.Now.ToLongDateString();  //SCADA konekcija
        }

        public void SetCurrentSubstation()
        {
            if (Substations.Count > 0)
            {
                if (SelectedSubstation != null)
                    SubstationCurrent = SelectedSubstation;
                else
                    SubstationCurrent = Substations.Values.First();
                
                TransformerCurrent = SubstationCurrent.Transformator.Current.ToString();
                TransformerVoltage = SubstationCurrent.Transformator.Voltage.ToString();
                TransformerTapChanger = SubstationCurrent.Transformator.TapChangerValue.ToString();

                GridPower = substationCurrent.Transformator.Voltage.ToString();
                
                if(substationCurrent.AsynchronousMachines[0] != null)
                {
                    GaugeClasic1 = SubstationCurrent.AsynchronousMachines[0].CosPhi.ToString();
                }

                if (substationCurrent.AsynchronousMachines.Count == 2)
                {
                    GaugeClasic1 = SubstationCurrent.AsynchronousMachines[1].CosPhi.ToString();
                    GaugeClasicOpacity = "100";
                }
                else
                {
                    GaugeClasicOpacity = "0";
                }
                //Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = AlarmEventType.UI, GiD = long.Parse(substationCurrent.Gid), Message = "Substation selected.", PointName = SubstationCurrent.Name };
                //ProxyServices.AlarmEventServiceProxy.AddEvent(e);
            }
        }

        public void PopulateModel(object resources, string topic)
        {
            if (topic == "nms")
            {
                NMSModel nMSModel = (NMSModel)resources;
                SubstationItems = toUIModelList(nMSModel.ResourceDescs);
                setModel(nMSModel.ResourceDescs);
                SetCurrentSubstation();
                FillSubstationItems();
                if (SubstationCurrent != null)
                {
                    meshViewModel.UpdateSubstationModel(SubstationCurrent);
                }

                //Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = AlarmEventType.UI, GiD = 0,
                //    Message = "Model arrived and loaded from NMS.", PointName = "" };
                //ProxyServices.AlarmEventServiceProxy.AddEvent(e);
            }
            else if (topic == "scada")
            {
                ScadaUIExchangeModel[] models = (ScadaUIExchangeModel[])resources;
                List<ScadaUIExchangeModel> measurements = ((ScadaUIExchangeModel[])resources).ToList();
                foreach (var measure in measurements)
                {
                    //measure.Gid to da se poklapa sa nekim od gidova od asinhrone ili od brejkera ili od diskonektora ili od ono za fluide itd..trebaju nam merenja
                    //Prodje se kroz sve podstanice i onda kroz svaki od elemenata i onda radi radnju

                    foreach(Substation sub in Substations.Values)
                    {
                        if (sub.Gid == SubstationCurrent.Gid)
                        {
                            ChangesFromScadaCommand(SubstationCurrent, measure);
                        }
                        ChangesFromScadaCommand(sub, measure);
                    }
                }

                foreach(Substation s in Substations.Values)
                {
                    if(s.Gid == SubstationCurrent.Gid)
                    {
                        meshViewModel.UpdateSubstationModel(SubstationCurrent);
                    }
                }
                FillSubstationItems();

                //Event e = new Event() {  EventReported = DateTime.Now, EventReportedBy = AlarmEventType.UI, GiD = 0,
                //    Message = "Acquisition arrived from SCADA.", PointName = "" };
                //ProxyServices.AlarmEventServiceProxy.AddEvent(e);

                Console.WriteLine(resources);
            }
            else if(topic == "alarm")
            {
                AlarmDescription alarmDesc = (AlarmDescription)resources;
                alarmHandler.ProcessAlarm(alarmDesc);
            }

            MeshVisible = false;

            /*threadAlarms = new Thread(CheckForAlarms);
            threadAlarms.Start();*/
        }

        public void ChangesFromScadaCommand(Substation sub, ScadaUIExchangeModel newValue)
        {
            int i = 0;
            if (sub.Breakers.Count > 0)
            {
                foreach (Breaker br in sub.Breakers)
                {
                    i++;
                    if (br.DiscreteGID == newValue.Gid)
                    {
                        br.NewState = ConverterState.ConvertToDiscreteState(newValue.Value);
                        br.State = br.NewState;
                        br.Time = newValue.Time.ToLongDateString();
                        if (sub.Breakers.Count > 2 && i != 1)
                            i += 2;
                        meshViewModel.ChangeStatesOfElements("Breaker" + i.ToString(), br);
                    }
                }
                i = 0;
            }
            if (sub.Disconectors.Count > 0)
            {
                foreach (Disconector dis in sub.Disconectors)
                {
                    i++;
                    if (dis.DiscreteGID == newValue.Gid)
                    {
                        dis.NewState = ConverterState.ConvertToDiscreteState(newValue.Value);
                        dis.State = dis.NewState;
                        dis.Time = newValue.Time.ToLongDateString();
                        meshViewModel.ChangeStatesOfElements("Disconector" + i.ToString(), dis);
                    }
                }
                i = 0;
            }
            if (sub.AsynchronousMachines.Count > 0)
            {
                foreach (AsynchronousMachine am in sub.AsynchronousMachines)
                {
                    if (am.SignalGid == newValue.Gid)
                    {
                        am.Time = newValue.Time.ToLongDateString();
                        CommandToAM(newValue.Value);
                    }
                }
            }
            if(sub.Transformator.AnalogCurrentGID == newValue.Gid)
            {
                sub.Transformator.Time = newValue.Time.ToString();
                sub.Transformator.Current = (float)newValue.Value;
                TransformerCurrent = newValue.Value.ToString();
            }
            if (sub.Transformator.AnalogVoltageGID == newValue.Gid)
            {
                sub.Transformator.Time = newValue.Time.ToLongDateString();
                sub.Transformator.Voltage = (float)newValue.Value;
                TransformerVoltage = newValue.Value.ToString();
            }
            if (sub.Transformator.AnalogTapChangerGID == newValue.Gid)
            {
                sub.Transformator.Time = newValue.Time.ToLongDateString();
                sub.Transformator.TapChangerValue = (long)newValue.Value;
                TransformerTapChanger = newValue.Value.ToString();
            }
        }

        public void CommandToAM(double value)
        {
            GaugeClasic1 = value.ToString();
        }

        public void CommandTransformerCurrentVoltage(object transformer, string type)
        {
            Transformator t = ((Transformator)transformer);

            if (type.Contains("Current"))
            {
                TransformerCurrent = t.Current.ToString();
            }
            if (type.Contains("Voltage"))
            {
                TransformerVoltage = t.Voltage.ToString();
            }
            if (type.Contains("TapChanger"))
            {
                TransformerTapChanger = t.TapChangerValue.ToString();
                int i = 0;
                foreach (Substation s in Substations.Values)
                {
                    if (s.Transformator.GID == t.GID)
                    {
                        if (s.Breakers.Count == 2)
                            i = 1;
                        else
                            i = 2;
                        break;
                    }
                }
                if(i == 1)
                {
                    SubstationItems.Where(x => x.Description.Contains("TapChanger")).ToList()[0].Value = t.TapChangerValue.ToString();
                    Messenger.Default.Send(new NotificationMessage("TapChanger", SubstationItems.Where(x => x.Description.Contains("TapChanger")).ToList()[0], "UpdateTelemetry"));
                }
                else if (i == 2)
                {
                    SubstationItems.Where(x => x.Description.Contains("TapChanger")).ToList()[1].Value = t.TapChangerValue.ToString();
                    Messenger.Default.Send(new NotificationMessage("TapChanger", SubstationItems.Where(x => x.Description.Contains("TapChanger")).ToList()[1], "UpdateTelemetry"));
                }
            }
        }

        public void populateEquipment(IEquipment equipment, List<Property> properties)
        {
            foreach (Property prop in properties)
            {
                switch (prop.Id)
                {
                    case Common.ModelCode.IDOBJ_GID:
                        equipment.GID = prop.GetValue().ToString();
                        break;
                    case Common.ModelCode.IDOBJ_DESC:
                        equipment.Description = prop.GetValue().ToString();
                        break;
                    case Common.ModelCode.IDOBJ_MRID:
                        equipment.MRID = prop.GetValue().ToString();
                        break;
                    case Common.ModelCode.IDOBJ_NAME:
                        equipment.Name = prop.GetValue().ToString();
                        break;
                }
            }
        }

        public void populateMachine(AsynchronousMachine asynchronousMachine, List<Property> properties)
        {
            foreach (Property prop in properties)
            {
                switch (prop.Id)
                {
                    case Common.ModelCode.ASYNCMACHINE_COSPHI:
                        asynchronousMachine.CosPhi = Double.Parse(prop.GetValue().ToString());
                        break;
                }
            }
        }
        public void populatTapChanger(TapChanger tapChanger, List<Property> properties)
        {
            foreach (Property prop in properties)
            {
                switch (prop.Id)
                {
                    case Common.ModelCode.TAPCHANGER_HIGHSTEP:
                        tapChanger.HighStep = Int32.Parse(prop.GetValue().ToString());
                        break;
                    case Common.ModelCode.TAPCHANGER_LOWSTEP:
                        tapChanger.LowStep = Int32.Parse(prop.GetValue().ToString());
                        break;
                    case Common.ModelCode.TAPCHANGER_NORMALSTEP:
                        tapChanger.NormalStep = Int32.Parse(prop.GetValue().ToString());
                        break;
                }
            }
        }

        public Substation getMySubstation(List<Property> properties)
        {
            Property subGid = properties.Where(x => x.Id == ModelCode.EQUIPMENT_EQUIPCONTAINER).FirstOrDefault();
            return Substations[long.Parse(subGid.PropertyValue.LongValue.ToString())];
        }

        public Substation getSubstationForTapChaner(List<Property> properties, List<ResourceDescription> resources)
        {
            Property twGid = properties.Where(x => x.Id == ModelCode.RATIOTAPCHANGER_TRWINDING).FirstOrDefault();
            ResourceDescription tw = resources.Where(x => x.Properties.Where(y => y.Id == ModelCode.IDOBJ_GID).FirstOrDefault().PropertyValue == twGid.PropertyValue).FirstOrDefault();
            Property ptGid = tw.Properties.Where(x => x.Id == ModelCode.TRANSFORMERWINDING_POWERTR).FirstOrDefault();
            ResourceDescription pt = resources.Where(x => x.Properties.Where(y => y.Id == ModelCode.IDOBJ_GID).FirstOrDefault().PropertyValue == ptGid.PropertyValue).FirstOrDefault();
            return getMySubstation(pt.Properties);
        }

        public void setRadioButtons()
        {
            ObservableCollection<RadioButton> test = new ObservableCollection<RadioButton>();
            int i = 0;
            foreach (KeyValuePair<long, Substation> sub in substations)
            {
                RadioButton rb = new RadioButton() { Content = (i + 1) + ". " + sub.Value.Name + "(" + sub.Key + ")", IsChecked = i == 0 };
                rb.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE8D856"));
                rb.Checked += (sender, args) =>
                {
                    foreach (RadioButton radioBut in test)
                    {
                        if (radioBut.Tag != (sender as RadioButton).Tag)
                        {
                            radioBut.IsChecked = false;
                        }
                    }

                    String[] gidList = (sender as RadioButton).Content.ToString().Split('(');
                    String[] gidL = gidList[1].Split(')');
                    String gid = gidL[0];
                    setSelectedSubstation(gid);


                    RadioButtons = test;
                };
                rb.Tag = i;
                i++;
                test.Add(rb);
            }
            RadioButtons = test;
        }

        public void setSelectedSubstation(String gid) { }

        public void setModel(List<ResourceDescription> resources)
        {
            int numberOfSubstations = resources.Where(x => (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.SUBSTATION)).Count();

            foreach (ResourceDescription sub in resources.Where(x => ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.SUBSTATION))
            {
                Property name = sub.Properties.Where(x => x.Id == ModelCode.IDOBJ_NAME).FirstOrDefault();
                Property description = sub.Properties.Where(x => x.Id == ModelCode.IDOBJ_DESC).FirstOrDefault();
                Property gid = sub.Properties.Where(x => x.Id == ModelCode.IDOBJ_GID).FirstOrDefault();
                Substation substation = new Substation(name.GetValue().ToString(), description.GetValue().ToString(), gid.GetValue().ToString());
                if(!substations.ContainsKey((long)gid.GetValue()))
                    substations.Add((long)gid.GetValue(), substation);
            }
            SubstationsList = new ObservableCollection<Substation>(substations.Values);
            // setRadioButtons();
            foreach (ResourceDescription resource in resources.Where(x => (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.DISCONNECTOR) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.BREAKER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.RATIOTAPCHANGER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.POWERTRANSFORMER) ||
                                                                         (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.ANALOG) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.ASYNCHRONOUSMACHINE) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.DISCRETE)))
            {

                switch (ModelCodeHelper.ExtractTypeFromGlobalId(resource.Id))
                {
                    case (short)DMSType.DISCONNECTOR:
                        Disconector disconector = new Disconector();
                        populateEquipment(disconector, resource.Properties);
                        getMySubstation(resource.Properties).Disconectors.Add(disconector);
                        break;
                    case (short)DMSType.BREAKER:
                        Breaker breaker = new Breaker();
                        populateEquipment(breaker, resource.Properties);
                        getMySubstation(resource.Properties).Breakers.Add(breaker);
                        break;
                    case (short)DMSType.RATIOTAPCHANGER:
                        TapChanger tapChanger = new TapChanger();
                        populateEquipment(tapChanger, resource.Properties);
                        populatTapChanger(tapChanger, resource.Properties);
                        getSubstationForTapChaner(resource.Properties, resources).TapChanger = tapChanger;
                        break;
                    case (short)DMSType.ASYNCHRONOUSMACHINE:
                        AsynchronousMachine asynchronousMachine = new AsynchronousMachine();
                        populateEquipment(asynchronousMachine, resource.Properties);
                        populateMachine(asynchronousMachine, resource.Properties);
                        getMySubstation(resource.Properties).AsynchronousMachines.Add(asynchronousMachine);
                        break;
                    case (short)DMSType.POWERTRANSFORMER:
                        Transformator transformator = new Transformator();
                        populateEquipment(transformator, resource.Properties);
                        getMySubstation(resource.Properties).Transformator = transformator;
                        break;
                    case (short)DMSType.DISCRETE:
                        foreach(var v in resource.Properties.Where(x => x.Id == ModelCode.MEASUREMENT_PSR))
                        {
                            float value = (resource.Properties.Where(x => x.Id == ModelCode.DISCRETE_NORMALVALUE).First()).PropertyValue.FloatValue;

                            long gid = v.PropertyValue.LongValue;
                            ResourceDescription res = resources.Where(x => x.Id == gid).ToList<ResourceDescription>()[0];
                            if ((ModelCodeHelper.ExtractTypeFromGlobalId(res.Id) == (short)DMSType.BREAKER))
                            {
                                foreach (Substation s in Substations.Values)
                                {
                                    foreach (Breaker b in s.Breakers)
                                    {
                                        if (b.GID == gid.ToString())
                                        {
                                            b.DiscreteGID = resource.Id;
                                            b.State = ConverterState.ConvertToDiscreteState(value);
                                            b.NewState = ConverterState.ConvertToDiscreteState(value);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if ((ModelCodeHelper.ExtractTypeFromGlobalId(res.Id) == (short)DMSType.DISCONNECTOR))
                            {
                                foreach (Substation s in Substations.Values)
                                {
                                    foreach(Disconector d in s.Disconectors)
                                    {
                                        if (d.GID == gid.ToString())
                                        {
                                            d.DiscreteGID = resource.Id;
                                            d.State = ConverterState.ConvertToDiscreteState(value);
                                            d.NewState = ConverterState.ConvertToDiscreteState(value);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case (short)DMSType.ANALOG:
                        foreach (var analog in resource.Properties.Where(x=>x.Id == ModelCode.MEASUREMENT_PSR))
                        {
                            float value = (resource.Properties.Where(x => x.Id == ModelCode.ANALOG_NORMALVALUE).First()).PropertyValue.FloatValue;
                            double maxValue = (resource.Properties.Where(x => x.Id == ModelCode.ANALOG_MAXVALUE).First()).PropertyValue.FloatValue;
                            double minValue = (resource.Properties.Where(x => x.Id == ModelCode.ANALOG_MINVALUE).First()).PropertyValue.FloatValue;

                            long gid = analog.PropertyValue.LongValue;
                            
                            ResourceDescription resource1 = resources.Where(x => x.Id == gid).ToList().First();
                            
                            if(ModelCodeHelper.ExtractTypeFromGlobalId(resource1.Id) == (short)DMSType.ASYNCHRONOUSMACHINE)
                            {
                                foreach(Substation s in Substations.Values)
                                {
                                    if (s.AsynchronousMachines.Count > 0)
                                    {
                                        foreach(var am in s.AsynchronousMachines)
                                        {
                                            if(am.GID == gid.ToString())
                                            {
                                                am.SignalGid = resource.Id;
                                                am.State = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else if(ModelCodeHelper.ExtractTypeFromGlobalId(resource1.Id) == (short)DMSType.POWERTRANSFORMER)
                            {
                                string type = resource.Properties.Where(x => x.Id == ModelCode.IDOBJ_MRID).First().PropertyValue.StringValue;

                                foreach(Substation s in Substations.Values)
                                {
                                    if(s.Transformator != null)
                                    {
                                        if(s.Transformator.GID == gid.ToString())
                                        {
                                            if (type.Contains("Voltage"))
                                            {
                                                s.Transformator.AnalogVoltageGID = resource.Id;
                                                s.Transformator.Current = value;
                                                s.Transformator.MaxCurrent = 10000;
                                                s.Transformator.MinCurrent = minValue;
                                            }
                                            else if (type.Contains("Current"))
                                            {
                                                s.Transformator.AnalogCurrentGID = resource.Id;
                                                s.Transformator.Voltage = value;
                                                s.Transformator.MaxVoltage = maxValue;
                                                s.Transformator.MinVoltage = minValue;
                                            }
                                            else if (type.Contains("TapChanger"))
                                            {
                                                s.Transformator.AnalogTapChangerGID = resource.Id;
                                                s.Transformator.TapChangerValue = (long)value;
                                                s.Transformator.MaxValueTapChanger = maxValue;
                                                s.Transformator.MinValueTapChanger = minValue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }

            }
            Console.Write(substations);
        }

        public ObservableCollection<UIModel> toUIModelList(List<ResourceDescription> resources)
        {
            ObservableCollection<UIModel> response = new ObservableCollection<UIModel>();

            foreach (ResourceDescription resource in resources.Where(x => (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.DISCONNECTOR) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.BREAKER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.RATIOTAPCHANGER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.ASYNCHRONOUSMACHINE) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.POWERTRANSFORMER)))
            {
                UIModel model = new UIModel();
                
                foreach (Property property in resource.Properties)
                {
                    switch (property.Id)
                    {
                        case Common.ModelCode.IDOBJ_GID:
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
                model.Time = DateTime.Now.ToString();

                response.Add(model);
            }
            return response;
        }

    }
}
