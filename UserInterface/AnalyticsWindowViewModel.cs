using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using RepositoryCore;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using UserInterface.BaseError;
using UserInterface.Model;
using UserInterface.Networking;
using UserInterface.ViewModel;

namespace UserInterface
{
    class AnalyticsWindowViewModel : BindableBase
    {
        private bool isDiscreteSelected;
        private List<SignalListItemViewModel> signalList;

        public Dictionary<long, Substation> substations;
        public Dictionary<long, StepLineSeries> signalsOn;
        public SeriesCollection SeriesCollection { get; set; }

        private ObservableCollection<RadioButton> radioButtons = new ObservableCollection<RadioButton>();

        public List<Substation> SubstationList { get; set; }
       
        public Substation SelectedSubstation
        {
            get { return selectedSubstation; }
            set
            {
                selectedSubstation = value;
                PopulateSignals(selectedSubstation.Gid);
                signalsOn.Clear();
                SeriesCollection.Clear();
            }
        }

        public List<SignalListItemViewModel> SignalList
        {
            get => signalList;
            set => SetProperty(ref signalList, value);
        }

        private DateTime? startDate = null;
        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; OnPropertyChanged("StartDate"); }
        }

        private DateTime? endDate = null;
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged("EndDate"); }
        }


        private Messenger messenger = new Messenger();
        private Substation selectedSubstation;

        private IMeasurementRepository measurementProxy;

        public AnalyticsWindowViewModel(Dictionary<long, Substation> substations, IMeasurementRepository measurementProxy)
        {
            this.substations = substations;
            SeriesCollection = new SeriesCollection();
            signalsOn = new Dictionary<long, StepLineSeries>();
            Setup();
            Set();
            this.measurementProxy = measurementProxy;
        }

        public void Setup()
        {
            SubstationList = substations.Values.ToList();
        }

        public void Set()
        {
            Messenger.Default.Register<SignalListItemViewModel>(this, (message) => { HandleSignalChecked(message); });
        }

        private void HandleSignalChecked(SignalListItemViewModel signal)
        {

            if (signal.IsChecked)
            {
                Measurement[] measurements;
                if (StartDate != null && EndDate != null)
                {
                    DateTime start = StartDate ?? DateTime.Now;
                    DateTime end = EndDate ?? DateTime.Now;

                    measurements = measurementProxy.GetAllMeasurementsByTime(start, end, signal.Gid);

                } else
                {
                    measurements = measurementProxy.GetAllMeasurementsByGid(signal.Gid);
                }

               
                StepLineSeries line = MakeSignal(signal.Gid.ToString(), measurements);
                SeriesCollection.Add(line);
                signalsOn.Add(signal.Gid, line);
            }
            else
            {
                SeriesCollection.Remove(signalsOn[signal.Gid]);
                signalsOn.Remove(signal.Gid);
            }
        }

        public StepLineSeries MakeSignal(string title, Measurement[] measurements)
        {
            StepLineSeries line = new StepLineSeries();
            line.Title = title;
            line.AlternativeStroke = Brushes.White;
            line.Stroke = Brushes.Red;

            ChartValues<int> chartValues = new ChartValues<int>();
            
            foreach(var measure in measurements)
                chartValues.Add(measure.Value);
            

            line.Values = chartValues;
            return line;
        }


        private void PopulateSignals(string substationGid)
        {
            List<SignalListItemViewModel> tempList = new List<SignalListItemViewModel>();
            Substation selectedSub = substations.Where(x => x.Value.Gid == substationGid).FirstOrDefault().Value;

            foreach(var dis in selectedSub.Disconectors)
            {
                tempList.Add(new SignalListItemViewModel()
                {
                    Name = dis.DiscreteGID.ToString(),
                    Gid = dis.DiscreteGID
                });
            }

            foreach (var breaker in selectedSub.Breakers)
            {
                tempList.Add(new SignalListItemViewModel()
                {
                    Name = breaker.DiscreteGID.ToString(),
                    Gid = breaker.DiscreteGID
                });
            }

            foreach (var asyncMach in selectedSub.AsynchronousMachines)
            {
                tempList.Add(new SignalListItemViewModel()
                {
                    Name = asyncMach.SignalGid.ToString(),
                    Gid = asyncMach.SignalGid
                });
            }
            SignalList = tempList;
        }

        private void PopulateSignals1(string substationGid)
            => SignalList = substations.Where(x => x.Value.Gid == substationGid)
            .Select(x => new { x.Value.AsynchronousMachines, x.Value.Breakers, x.Value.Disconectors })
            .Select(x => MapSignals(x))
            .SelectMany(x => x)
            .ToList();
        

        private IEnumerable<SignalListItemViewModel> MapSignals(dynamic x)
        {
            if (isDiscreteSelected)
            {
                return new List<SignalListItemViewModel>(
                    ((List<Disconector>)x.Disconectors)
                    .Select(d =>
                                new SignalListItemViewModel()
                                {
                                    Name = d.Name,
                                    Gid = d.DiscreteGID
                                }))
                   {
                        new SignalListItemViewModel()
                        {
                            Name = x.Breaker.Name,
                            Gid = x.Breaker.DiscreteGID
                        }
                 };
            }


            return new List<SignalListItemViewModel>(
                ((List<AsynchronousMachine>)x.AsynchronousMachines)
                .Select(d =>
                            new SignalListItemViewModel()
                            {
                                Name = d.Name,
                                Gid = d.SignalGid
                            }));
        }
        
        public bool IsDiscreteSelected
        {
            get => isDiscreteSelected;
            set
            {
                SetProperty(ref isDiscreteSelected, value);
                PopulateSignals(selectedSubstation.Gid);
            }
        }
    }
}
