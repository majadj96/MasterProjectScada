﻿using Common;
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
using UserInterface.Model;
using UserInterface.Subscription;
using UserInterface.ViewModel;

namespace UserInterface
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> ButtonTablesCommand { get; private set; }
        public MyICommand<string> SearchSubsCommand { get; private set; }
        public MyICommand<string> DissmisSubsCommand { get; private set; }
        public MyICommand<string> LoadSubstationCommand { get; private set; }

        private MeshViewModel meshViewModel = new MeshViewModel();

        private ObservableCollection<RadioButton> radioButtons = new ObservableCollection<RadioButton>();
        private ObservableCollection<Substation> substationsList = new ObservableCollection<Substation>();
        private ObservableCollection<Substation> substationsListCopy = new ObservableCollection<Substation>();
        private ObservableCollection<string> searchType = new ObservableCollection<string> {"Name", "GID" };
        List<Substation> searchedSubs = new List<Substation>();


        #region Variables
        private BindableBase currentMeshViewModel;
        private BindableBase currentTableViewModel;

        private Substation selectedSubstation;

        public Dictionary<long, Substation> substations;
        public ObservableCollection<UIModel> substationItems = new ObservableCollection<UIModel>();
        
        public string statistics { get; set; }
        public string pubSub { get; set; }
        public string connectedStatusBar { get; set; }
        public string timeStampStatusBar { get; set; }
        public string gaugeValue { get; set; }
        public string anguarValue { get; set; }
        public string gaugeClasic { get; set; }
        public string searchTerm { get; set; }
        public string searchTypeSelected { get; set; }

        #endregion

        #region Props
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
        public string GaugeClasic
        {
            get
            {
                return gaugeClasic;
            }
            set
            {
                gaugeClasic = value;
                OnPropertyChanged("GaugeClasic");
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
            SubNMS subNMS = new SubNMS();
            subNMS.OnSubscribe();
            setUpInitState();

            substations = new Dictionary<long, Substation>();

            timer.Tick += new EventHandler(Test);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            Messenger.Default.Register<NotificationMessage>(this, (message) => { PopulateModel(message.Target); });
        }

        private void changeGrid(string a)
        {
            meshViewModel.setSubstation(SelectedSubstation);
        }
        private void searchSubstation(string a)
        {
            Console.WriteLine(SearchTerm);
            Console.WriteLine(SearchTypeSelected);

            if(SearchTypeSelected == "Name")
            {
                searchedSubs = substationsListCopy.Where(x => x.Name.Contains(SearchTerm)).ToList();
                SubstationsList = new ObservableCollection<Substation>(searchedSubs);
            }else
            {
                searchedSubs = substationsListCopy.Where(x => x.Gid.Contains(SearchTerm)).ToList();
                SubstationsList = new ObservableCollection<Substation>(searchedSubs);
            }
           
        }
        private void dissmisSubstation(string a)
        {
            SubstationsList = substationsListCopy;
            SearchTerm = "";
        }


    private void Test(object sender, EventArgs e)
        {
            GaugeValue = rand.Next(0, 100).ToString();

            AnguarValue = rand.Next(-7, 7).ToString();

            GaugeClasic = rand.Next(300, 1000).ToString();
        }

        private void OnNavigation(string destination)
        {
            TablesWindow tablesWindow = new TablesWindow();

            TablesWindowViewModel tablesWindowViewModel = new TablesWindowViewModel();

            tablesWindow.DataContext = tablesWindowViewModel;

            tablesWindowViewModel.SetView(destination);

            tablesWindow.Show();
        }

        public void setUpInitState()
        {
            connectedStatusBar = "Dissconnected"; //SCADA konekcija
            timeStampStatusBar = DateTime.Now.ToLongDateString();  //SCADA konekcija
        }
        
        public void PopulateModel(object resources)
        {
            NMSModel nMSModel = (NMSModel)resources;
            SubstationItems = toUIModelList(nMSModel.ResourceDescs);
            setModel(nMSModel.ResourceDescs);
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
            foreach(KeyValuePair<long, Substation> sub in substations)
            {
                RadioButton rb = new RadioButton() { Content = (i+1) + ". " + sub.Value.Name + "(" + sub.Key + ")", IsChecked = i == 0 };
                rb.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE8D856"));
                rb.Checked += (sender, args) =>
                {
                    foreach (RadioButton radioBut in test)
                    {
                        if(radioBut.Tag != (sender as RadioButton).Tag)
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
                substations.Add((long)gid.GetValue(), substation);
            }
            ObservableCollection<Substation> subs = new ObservableCollection<Substation>(substations.Values);
            SubstationsList = subs;
            substationsListCopy = subs;
           // setRadioButtons();
            foreach (ResourceDescription resource in resources.Where(x => (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.DISCONNECTOR) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.BREAKER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.RATIOTAPCHANGER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.POWERTRANSFORMER) ||
                                                                        (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.ASYNCHRONOUSMACHINE)))
            {

                switch (ModelCodeHelper.ExtractTypeFromGlobalId(resource.Id)) {
                    case (short)DMSType.DISCONNECTOR:
                        Disconector disconector = new Disconector();
                        populateEquipment(disconector, resource.Properties);
                        getMySubstation(resource.Properties).Disconectors.Add(disconector);
                        break;
                    case (short)DMSType.BREAKER:
                        Breaker breaker = new Breaker();
                        populateEquipment(breaker, resource.Properties);
                        getMySubstation(resource.Properties).Breaker = breaker;
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
                }

            }
            Console.Write(substations);
        }

        public ObservableCollection<UIModel> toUIModelList(List<ResourceDescription> resources)
        {
            ObservableCollection<UIModel> response = new ObservableCollection<UIModel>();
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
                                    //Disc1Id = property.GetValue().ToString();
                                    disconectors++;
                                } else if (disconectors == 2)
                                {
                                    //Disc2Id = property.GetValue().ToString();
                                    disconectors = 0;
                                }
                            } else if (ModelCodeHelper.ExtractTypeFromGlobalId(resource.Id) == (short)DMSType.BREAKER)
                            {
                                //BreakerId = property.GetValue().ToString();
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
