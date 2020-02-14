using GalaSoft.MvvmLight.Messaging;
using System;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Subscription;
using UserInterface.ViewModel;

namespace UserInterface
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavigationCommand { get; private set; }
        
        private MeshViewModel meshViewModel = new MeshViewModel();
        private TableViewModel tableViewModel = new TableViewModel();
        private AlarmViewModel alarmViewModel = new AlarmViewModel();
        
        #region Variables
        private BindableBase currentMeshViewModel;
        private BindableBase currentTableViewModel;

        public string statistics { get; set; }
        public string pubSub { get; set; }
        public string connectedStatusBar { get; set; }
        public string timeStampStatusBar { get; set; }
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
        #endregion

        public MainWindowViewModel()
        {
            CurrentMeshViewModel = meshViewModel;
            CurrentTableViewModel = tableViewModel;
            NavigationCommand = new MyICommand<string>(OnNavigation);

            SubNMS subNMS = new SubNMS();
            subNMS.OnSubscribe();
            setUpInitState();
            Messenger.Default.Register<NotificationMessage>(tableViewModel, (message) => { tableViewModel.PopulateModel(message.Target); });
        }

        private void OnNavigation(string destination)
        {
            switch (destination)
            {
                case "Points":
                    CurrentTableViewModel = tableViewModel;
                    break;
                case "Alarm":
                    CurrentTableViewModel = alarmViewModel;
                    break;
            }
        }

        public void setUpInitState()
        {
            connectedStatusBar = "Dissconnected"; //SCADA konekcija
            timeStampStatusBar = DateTime.Now.ToLongDateString();  //SCADA konekcija
        }
        
        //comboSubstations lista u comboBoxu
        //SelectedSubstation izabrani u comboBoxu
        //substationItems lista
        //substationItem oznacen u listi 
        
        /*public void PopulateModel(object resources)
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
            return substations[long.Parse(subGid.PropertyValue.LongValue.ToString())];
        }

        public Substation getSubstationForTapChaner(List<Property> properties, List<ResourceDescription> resources)
        {
            Property twGid = properties.Where(x => x.Id == ModelCode.RATIOTAPCHANGER_TRWINDING).FirstOrDefault();
            ResourceDescription tw = resources.Where(x => x.Properties.Where(y => y.Id == ModelCode.IDOBJ_GID).FirstOrDefault().PropertyValue == twGid.PropertyValue).FirstOrDefault();
            Property ptGid = tw.Properties.Where(x => x.Id == ModelCode.TRANSFORMERWINDING_POWERTR).FirstOrDefault();
            ResourceDescription pt = resources.Where(x => x.Properties.Where(y => y.Id == ModelCode.IDOBJ_GID).FirstOrDefault().PropertyValue == ptGid.PropertyValue).FirstOrDefault();
            return getMySubstation(pt.Properties);
        }

        public void setModel(List<ResourceDescription> resources)
        {
            int numberOfSubstations = resources.Where(x => (ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.SUBSTATION)).Count();

            foreach (ResourceDescription sub in resources.Where(x => ModelCodeHelper.ExtractTypeFromGlobalId(x.Id) == (short)DMSType.SUBSTATION))
            {
                Substation substation = new Substation();
                Property gid = sub.Properties.Where(x => x.Id == ModelCode.IDOBJ_GID).FirstOrDefault();
                substations.Add((long)gid.GetValue(), substation);
            }

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
        }*/
    } 
}
