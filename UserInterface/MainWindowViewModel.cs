﻿using Common;
using Common.AlarmEvent;
using Common.GDA;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using PubSubCommon;
using ScadaCommon;
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
using UserInterface.Helper;
using UserInterface.Model;
using UserInterface.Networking;
using UserInterface.ProxyPool;
using UserInterface.Subscription;
using UserInterface.ViewModel;

namespace UserInterface
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands
        public MyICommand<string> ButtonTablesCommand { get; private set; }
        public MyICommand<string> SearchSubsCommand { get; private set; }
        public MyICommand<string> DissmisSubsCommand { get; private set; }
        public MyICommand<string> AnalyticsOpenCommand { get; private set; }
        public MyICommand<string> LoadSubstationCommand { get; private set; }
        #endregion

        #region Variables
        private MeshViewModel meshViewModel = new MeshViewModel();
        private ObservableCollection<RadioButton> radioButtons = new ObservableCollection<RadioButton>();
        private ObservableCollection<Substation> substationsList = new ObservableCollection<Substation>();
        private ObservableCollection<string> searchType = new ObservableCollection<string> { "Name", "GID" };
        List<Substation> searchedSubs = new List<Substation>();
        private AlarmHandler alarmHandler;
        private CustomEventHandler customEventHandler;
        private DispatcherTimer AlarmButtonTimer = new DispatcherTimer();
        private MeasurementProxy measurementRepository;
        private BindableBase currentMeshViewModel;
        private BindableBase currentTableViewModel;

        private Dictionary<long, Substation> substations;
        private ObservableCollection<UIModel> substationItems = new ObservableCollection<UIModel>();

        private Dictionary<long, Measurement> measurements = new Dictionary<long, Measurement>();

        private Substation substationCurrent;

        private Substation selectedSubstation;

        private string statistics;
        private string pubSub;
        private string connectedStatusBar;
        private string timeStampStatusBar;
        private double gaugeClasic = 0;
        private string searchTerm;
        private string searchTypeSelected;
        private double gaugePower1 = 0;
        private double gaugePower2 = 0;
		private string gaugePower2visibility = "Hidden";
		private string gaugePressure2visibility = "Hidden";
		private string transformerCurrent;
        private string transformerVoltage;
        private string transformerTapChanger = "7";

        private bool BlinkOnFlag = false, FlagToStartBlinking = false;
        private bool meshVisible = true;
        private SolidColorBrush buttonAlarmBrush = Brushes.Crimson;
        private bool _isConnectedScada;


        #endregion

        #region Props
        public bool IsConnectedScada
        {
            get => _isConnectedScada;
            set
            {
                Set(ref _isConnectedScada, value);
            }
        }
        public BindableBase CurrentMeshViewModel
        {
            get { return currentMeshViewModel; }
            set { Set(ref currentMeshViewModel, value); }
        }
        public BindableBase CurrentTableViewModel
        {
            get { return currentTableViewModel; }
            set { Set(ref currentTableViewModel, value); }
        }

        public Substation SubstationCurrent
        {
            get { return substationCurrent; }
            set { Set(ref substationCurrent, value); }
        }

        public Dictionary<long, Substation> Substations
        {
            get
            {
                return substations;
            }
            set
            {
                Set(ref substations, value);
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
                Set(ref substationItems, value);
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
                Set(ref substationsList, value);
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
                Set(ref searchType, value);
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
                Set(ref radioButtons, value);
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
                Set(ref connectedStatusBar, value);
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
                Set(ref selectedSubstation, value);
            }
        }
        public double GaugeClasic
        {
            get
            {
                return gaugeClasic;
            }
            set
            {
                Set(ref gaugeClasic, value);
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
                Set(ref searchTypeSelected, value);
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
                Set(ref searchTerm, value);
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
                Set(ref timeStampStatusBar, value);
            }
        }
        public double GaugePower1
        {
            get
            {
                return gaugePower1;
            }
            set
            {
                Set(ref gaugePower1, value);
            }
        }
        public double GaugePower2
        {
            get
            {
                return gaugePower2;
            }
            set
            {
                Set(ref gaugePower2, value);
            }
        }
		public string GaugePower2Visibility
		{
			get
			{
				return gaugePower2visibility;
			}
			set
			{
                Set(ref gaugePower2visibility, value);
            }
        }

		public string GaugePressure2Visibility
		{
			get
			{
				return gaugePressure2visibility;
			}
			set
            {
                Set(ref gaugePressure2visibility, value);
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
                Set(ref pubSub, value);
            }
        }
        public string TransformerCurrent
        {
            get { return transformerCurrent; }
            set
            {
                Set(ref transformerCurrent, value);
            }
        }
        public string TransformerVoltage
        {
            get { return transformerVoltage; }
            set
            {
                Set(ref transformerVoltage, value);
            }
        }
        public string TransformerTapChanger
        {
            get { return transformerTapChanger; }
            set
            {
                Set(ref transformerTapChanger, value);
            }
        }


        public SolidColorBrush ButtonAlarmBrush
        {
            get
            {
                return buttonAlarmBrush;
            }
            set
            {
                Set(ref buttonAlarmBrush, value);
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
                Set(ref meshVisible, value);
            }
        }
        
        //public DialogViewModel Dialog { get; set; } 

        public Dictionary<long, Measurement> Measurements { get => measurements; set => measurements = value; }
        #endregion

        Random rand = new Random();
        Sub subscriptionManager = null;

        public MainWindowViewModel()
        {
            meshViewModel.Measurements = Measurements;
            CurrentMeshViewModel = meshViewModel;
            ButtonTablesCommand = new MyICommand<string>(OnNavigation);
            LoadSubstationCommand = new MyICommand<string>(changeGrid);
            SearchSubsCommand = new MyICommand<string>(searchSubstation);
            DissmisSubsCommand = new MyICommand<string>(dissmisSubstation);
            AnalyticsOpenCommand = new MyICommand<string>(openAnalytics);

            IsConnectedScada = false;

            alarmHandler = new AlarmHandler(Measurements);
            customEventHandler = new CustomEventHandler();
            measurementRepository = new MeasurementProxy("MeasurementEndPoint");

            subscriptionManager = new Sub();
            subscribeTo("nms");
            subscribeTo("scada");
            subscribeTo("alarm");
            subscribeTo("connectionState");
            subscribeTo("event");
            setUpInitState();

            substations = new Dictionary<long, Substation>();
           
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
        }

        private void subscribeTo(string topic)
        {
            subscriptionManager.OnSubscribe(topic);
        }

        private void changeGrid(string a)
        {
            SetCurrentSubstation();
            SetGaugeValues();
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

            AnalyticsWindowViewModel analyticsWindowViewModel = new AnalyticsWindowViewModel(substations, measurementRepository, this.measurements);

            analyticsWindow.DataContext = analyticsWindowViewModel;

            analyticsWindow.Show();
        }
        private void dissmisSubstation(string a)
        {
            SubstationsList = new ObservableCollection<Substation>( substations.Values.ToList());
            SearchTerm = "";
        }

        #region AlarmsCheck
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
        #endregion
            
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

            TablesWindowViewModel tablesWindowViewModel = new TablesWindowViewModel(SubstationItems, this.alarmHandler, Substations.Values.ToList(), customEventHandler, Measurements);

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
            connectedStatusBar = "DISCONNECTED"; 
            timeStampStatusBar = DateTime.Now.ToLongDateString();
        }

        public void SetCurrentSubstation()
        {
            if (Substations.Count > 0)
            {
                if (SelectedSubstation != null)
                    SubstationCurrent = SelectedSubstation;
                else
                    SubstationCurrent = Substations.Values.First();

                //Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = AlarmEventType.UI, GiD = long.Parse(substationCurrent.Gid), Message = "Substation selected.", PointName = SubstationCurrent.Name };
                //ProxyServices.AlarmEventServiceProxy.AddEvent(e);
            }
        }

        private void SetGaugeValues()
        {
            TransformerCurrent = SubstationCurrent.Transformator.Current.ToString();
            TransformerVoltage = SubstationCurrent.Transformator.Voltage.ToString();
            TransformerTapChanger = SubstationCurrent.Transformator.TapChangerValue.ToString();

            foreach (Measurement meas in Measurements.Values)
            {
                bool exists = SubstationCurrent.Transformator.TransformerWindings.Any(x => x.GID == meas.PowerSystemResource.ToString());

				if (exists)
				{
					if (meas.Mrid == "PT1Current_W1")
						meshViewModel.StrujaW1 = meas.Value.ToString() + " A";
					else if (meas.Mrid == "PT1Current_W2")
						meshViewModel.StrujaW2 = meas.Value.ToString() + " A";
					else if (meas.Mrid == "PT1Voltage_W1")
						meshViewModel.NaponW1 = meas.Value.ToString() + " V";
					else if (meas.Mrid == "PT1Voltage_W2")
						meshViewModel.NaponW2 = meas.Value.ToString() + " V";
					else if (meas.Mrid == "PT2Current_W1")
						meshViewModel.Struja2W1 = meas.Value.ToString() + " A";
					else if (meas.Mrid == "PT2Current_W2")
						meshViewModel.Struja2W2 = meas.Value.ToString() + " A";
					else if (meas.Mrid == "PT2Voltage_W1")
						meshViewModel.Napon2W1 = meas.Value.ToString() + " V";
					else if (meas.Mrid == "PT2Voltage_W2")
						meshViewModel.Napon2W2 = meas.Value.ToString() + " V";
				}
                if (meas.PowerSystemResource.ToString() == SubstationCurrent.Gid)
                {
                    GaugeClasic = meas.Value;
                }
                else if (meas.PowerSystemResource.ToString() == SubstationCurrent.TapChanger.GID)
                {
                    TransformerTapChanger = meas.Value.ToString();
                }

                if (SubstationCurrent.AsynchronousMachines.Count == 1)
                {
                    if (SubstationCurrent.AsynchronousMachines[0].GID == meas.PowerSystemResource.ToString())
                    {
                        if (meas.Mrid.ToLower().Contains("power"))
                        {
                            GaugePower1 = meas.Value;
                        }
                        //else if (meas.Mrid.ToLower().Contains("pressure"))
                        //{
                        //    GaugePressure1 = meas.Value.ToString();
                        //}
                        GaugePower2 = 0;
						GaugePower2Visibility = "Hidden";
						//GaugePressure2Visibility = "Hidden";
						meshViewModel.Sub2Visibility = "Hidden";
                        //GaugePressure2 = string.Empty;
                    }
                }
                else if (SubstationCurrent.AsynchronousMachines.Count == 2)
                {
					meshViewModel.Sub2Visibility = "Visible";

					if (SubstationCurrent.AsynchronousMachines[0].GID == meas.PowerSystemResource.ToString())
                    {
                        if (meas.Mrid.ToLower().Contains("power"))
                        {
                            GaugePower1 = meas.Value;
                        }
                        //else if (meas.Mrid.ToLower().Contains("pressure"))
                        //{
                        //    GaugePressure1 = meas.Value.ToString();
                        //}
                    }
                    else if (SubstationCurrent.AsynchronousMachines[1].GID == meas.PowerSystemResource.ToString())
                    {
                        if (meas.Mrid.ToLower().Contains("power"))
                        {
							GaugePower2Visibility = "Visible";
                            GaugePower2 = meas.Value;
                        }
       //                 else if (meas.Mrid.ToLower().Contains("pressure"))
       //                 {
							//GaugePressure2Visibility = "Visible";
							//GaugePressure2 = meas.Value.ToString();
       //                 }
                    }
                }
            }
        }

        public void PopulateModel(object resources, string topic)
        {
            if (topic == "nms")
            {
                NMSModel nMSModel = (NMSModel)resources;
                if (nMSModel.ResourceDescs.Count > 0)
                {
                    SubstationItems = toUIModelList(nMSModel.ResourceDescs);
                    setModel(nMSModel.ResourceDescs);
                    SetCurrentSubstation();
                    SetGaugeValues();
                    FillSubstationItems();
                    if (SubstationCurrent != null)
                    {
                        meshViewModel.UpdateSubstationModel(SubstationCurrent);
                    }

                    //Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = AlarmEventType.UI, GiD = 0,
                    //    Message = "Model arrived and loaded from NMS.", PointName = "" };
                    //ProxyServices.AlarmEventServiceProxy.AddEvent(e);
                }
            }
            else if (topic == "scada")
            {
                ScadaUIExchangeModel[] models = (ScadaUIExchangeModel[])resources;
                List<ScadaUIExchangeModel> measurements = ((ScadaUIExchangeModel[])resources).ToList();
                foreach (var measure in measurements)
                {
                    if(Measurements.TryGetValue(measure.Gid, out Measurement meas))
                    {
                        meas.Value = measure.Value;
                        meas.Time = DateTime.UtcNow;
                        if(meas.PowerSystemResource.ToString() == SubstationCurrent.Gid)
                        {
                            GaugeClasic = meas.Value;
                        }
                        else if (meas.PowerSystemResource.ToString() == SubstationCurrent.TapChanger.GID)
                        {
                            TransformerTapChanger = meas.Value.ToString();
                        }

                        Messenger.Default.Send<Measurement>(meas);
                    }

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

                SetGaugeValues();
                FillSubstationItems();

                //Event e = new Event() {  EventReported = DateTime.Now, EventReportedBy = AlarmEventType.UI, GiD = 0,
                //    Message = "Acquisition arrived from SCADA.", PointName = "" };
                //ProxyServices.AlarmEventServiceProxy.AddEvent(e);
            }
            else if(topic == "alarm")
            {
                AlarmDescription alarmDesc = (AlarmDescription)resources;
                alarmHandler.ProcessAlarm(alarmDesc);
            }
			else if(topic == "connectionState")
			{
                ConnectionState cn = (ConnectionState)resources;
                if (cn == ConnectionState.CONNECTED)
                {
                    IsConnectedScada = true;
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.WorkerReportsProgress = true;
                    worker.DoWork += worker_DoWork;
                    worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                    worker.RunWorkerAsync(7000);
                } 

				ConnectedStatusBar = ((ConnectionState)resources).ToString();
                
			}
            else if (topic == "event")
            {
                customEventHandler.ProcessEvent((Event)resources);
            }

            MeshVisible = false;

            /*threadAlarms = new Thread(CheckForAlarms);
            threadAlarms.Start();*/
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int max = (int)e.Argument;
            int result = 0;
            for (int i = 0; i < max; i++)
            {
                System.Threading.Thread.Sleep(1);

            }
            e.Result = result;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsConnectedScada = false;
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
                        br.Time = newValue.Time.ToString();
                        br.Flag = newValue.Flag;
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
                        dis.Time = newValue.Time.ToString();
                        dis.Flag = newValue.Flag;
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
                        am.Time = newValue.Time.ToString();
                        //CommandToAM(newValue.Value);
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
                RadioButton rb = new RadioButton() {
                    Content = (i + 1) + ". " + sub.Value.Name + "(" + sub.Key + ")",
                    IsChecked = i == 0
                };

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
            int numberOfSubstations = resources.Where(x => (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.SUBSTATION)).Count(); //moze da se brise

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
            foreach (ResourceDescription resource in resources.Where(x => (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.DISCONNECTOR) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.BREAKER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.RATIOTAPCHANGER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.POWERTRANSFORMER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.TRANSFORMERWINDING) ||
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
                    case (short)DMSType.TRANSFORMERWINDING:
                        TransformerWinding transformerWinding = new TransformerWinding();
                        populateEquipment(transformerWinding, resource.Properties);
                        getMySubstation(resource.Properties).Transformator.TransformerWindings.Add(transformerWinding);
                        break;
                    case (short)DMSType.DISCRETE:
                        Measurement meas = new Measurement(resource.Id)
                        {
                            Value = resource.GetProperty(ModelCode.DISCRETE_NORMALVALUE).AsInt(),
                            Mrid = resource.GetProperty(ModelCode.IDOBJ_MRID).AsString(),
                            Name = resource.GetProperty(ModelCode.IDOBJ_NAME).AsString(),
                            Description = resource.GetProperty(ModelCode.IDOBJ_DESC).AsString(),
                            PowerSystemResource = resource.GetProperty(ModelCode.MEASUREMENT_PSR).AsReference(),
                            Min = resource.GetProperty(ModelCode.DISCRETE_MINVALUE).AsInt(),
                            Max = resource.GetProperty(ModelCode.DISCRETE_MAXVALUE).AsInt(),
                            Type = (MeasurementType)resource.GetProperty(ModelCode.MEASUREMENT_MEASTYPE).AsEnum(),
                            Time = DateTime.UtcNow
                        };

                        if(!Measurements.ContainsKey(meas.Gid))
                        {
                            Measurements.Add(meas.Gid, meas);
                            Messenger.Default.Send<Measurement>(meas);
                        }

                        foreach (var v in resource.Properties.Where(x => x.Id == ModelCode.MEASUREMENT_PSR))
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
                        Measurement meas_analog = new Measurement(resource.Id)
                        {
                            Value = resource.GetProperty(ModelCode.ANALOG_NORMALVALUE).AsFloat(),
                            Mrid = resource.GetProperty(ModelCode.IDOBJ_MRID).AsString(),
                            Name = resource.GetProperty(ModelCode.IDOBJ_NAME).AsString(),
                            Description = resource.GetProperty(ModelCode.IDOBJ_DESC).AsString(),
                            PowerSystemResource = resource.GetProperty(ModelCode.MEASUREMENT_PSR).AsReference(),
                            Min = resource.GetProperty(ModelCode.ANALOG_MINVALUE).AsFloat(),
                            Max = resource.GetProperty(ModelCode.ANALOG_MAXVALUE).AsFloat(),
                            Type = (MeasurementType)resource.GetProperty(ModelCode.MEASUREMENT_MEASTYPE).AsEnum(),
                            Time = DateTime.UtcNow
                        };

                        if (!Measurements.ContainsKey(meas_analog.Gid))
                        {
                            Measurements.Add(meas_analog.Gid, meas_analog);
                            Messenger.Default.Send<Measurement>(meas_analog);
                        }

                        foreach (var analog in resource.Properties.Where(x=>x.Id == ModelCode.MEASUREMENT_PSR))
                        {
                            float value = (resource.Properties.Where(x => x.Id == ModelCode.ANALOG_NORMALVALUE).First()).PropertyValue.FloatValue;
                            double maxValue = (resource.Properties.Where(x => x.Id == ModelCode.ANALOG_MAXVALUE).First()).PropertyValue.FloatValue;
                            double minValue = (resource.Properties.Where(x => x.Id == ModelCode.ANALOG_MINVALUE).First()).PropertyValue.FloatValue;

                            long gid = analog.PropertyValue.LongValue;

                            if (gid != 0)
                            {
                                ResourceDescription resource1 = resources.First(x => x.Id == gid);

                                if (ModelCodeHelper.ExtractTypeFromGlobalId(resource1.Id) == (short)DMSType.ASYNCHRONOUSMACHINE)
                                {
                                    foreach (Substation s in Substations.Values)
                                    {
                                        if (s.AsynchronousMachines.Count > 0)
                                        {
                                            foreach (var am in s.AsynchronousMachines)
                                            {
                                                if (am.GID == gid.ToString())
                                                {
                                                    am.SignalGid = resource.Id;
                                                    am.State = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (ModelCodeHelper.ExtractTypeFromGlobalId(resource1.Id) == (short)DMSType.TRANSFORMERWINDING)
                                {
                                    string type = resource.Properties.Where(x => x.Id == ModelCode.IDOBJ_MRID).First().PropertyValue.StringValue;

                                    foreach (Substation s in Substations.Values)
                                    {
                                        if (s.Transformator != null)
                                        {
                                            TransformerWinding TW = s.Transformator.TransformerWindings.FirstOrDefault(x => x.GID == gid.ToString());
                                            if (TW != null)
                                            {
                                                if (type.Contains("Voltage"))
                                                {
                                                    s.Transformator.TransformerWindings.FirstOrDefault(x => x.Equals(TW)).AnalogVoltageGID = resource.Id;
                                                    s.Transformator.TransformerWindings.FirstOrDefault(x => x.Equals(TW)).Voltage = value;
                                                    s.Transformator.TransformerWindings.FirstOrDefault(x => x.Equals(TW)).VoltageMaxValue = maxValue;
                                                    s.Transformator.TransformerWindings.FirstOrDefault(x => x.Equals(TW)).VoltageMinValue = minValue;
                                                }
                                                else if (type.Contains("Current"))
                                                {
                                                    s.Transformator.TransformerWindings.FirstOrDefault(x => x.Equals(TW)).AnalogCurrentGID = resource.Id;
                                                    s.Transformator.TransformerWindings.FirstOrDefault(x => x.Equals(TW)).Current = value;
                                                    s.Transformator.TransformerWindings.FirstOrDefault(x => x.Equals(TW)).CurrentMaxValue = maxValue;
                                                    s.Transformator.TransformerWindings.FirstOrDefault(x => x.Equals(TW)).CurrentMinValue = minValue;
                                                }
                                             
                                                //else if (type.Contains("TapChanger"))
                                                //{
                                                //    s.Transformator.AnalogTapChangerGID = resource.Id;
                                                //    s.Transformator.TapChangerValue = (long)value;
                                                //    s.Transformator.MaxValueTapChanger = maxValue;
                                                //    s.Transformator.MinValueTapChanger = minValue;
                                                //}
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
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.POWERTRANSFORMER ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.TRANSFORMERWINDING))))
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
                        case Common.ModelCode.TRANSFORMERWINDING_POWERTR:
                            break;
                        default:
                            break;
                    }
                }
                model.Time = DateTime.Now.ToString();
                model.Max = "1000";
                model.Min = "100";
                response.Add(model);
            }
            return response;
        }

    }
}
