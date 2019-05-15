using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
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
        private List<SignalListItemViewModel> _allSignalList;

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

        private Messenger messenger = new Messenger();
        private Substation selectedSubstation;
        

        public AnalyticsWindowViewModel(Dictionary<long, Substation> substations)
        {
            this.substations = substations;
            SeriesCollection = new SeriesCollection();
            signalsOn = new Dictionary<long, StepLineSeries>();
            Setup();
            Set();
        }

        public void Setup()
        {
            SubstationList = substations.Values.ToList();
        }

        public void Set()
        {
            Messenger.Default.Register<SignalListItemViewModel>(this, (message) => { HandleSignalChecked(message); });
        }
        int rand = 0;

        private void HandleSignalChecked(SignalListItemViewModel signal)
        {
            Console.WriteLine($"Selected signal {signal.Name}/{signal.Gid}/{signal.IsChecked}");
            StepLineSeries test;
            if (rand == 0)
            {
                test = Mock(signal.Gid.ToString());

            }
            else if (rand == 1)
            {
                test = Mock1(signal.Gid.ToString());

            }
            else
            {
                test = Mock2(signal.Gid.ToString());


            }
            rand++;

            if (signal.IsChecked)
            {
                SeriesCollection.Add(test);
                signalsOn.Add(signal.Gid, test);
            }
            else
            {
                SeriesCollection.Remove(signalsOn[signal.Gid]);
                signalsOn.Remove(signal.Gid);
            }
        }

        public StepLineSeries Mock(string title)
        {
            StepLineSeries test = new StepLineSeries();
            test.Title = title;
            test.Stroke = Brushes.Red;
            test.AlternativeStroke = Brushes.LightPink;
            test.Values = new ChartValues<int> { 1, 1, 1, 0, 0, 1 };
            return test;
        }
        public StepLineSeries Mock1(string title)
        {
            StepLineSeries test = new StepLineSeries();
            test.Title = title;
            test.Stroke = Brushes.Yellow;
            test.AlternativeStroke = Brushes.LightGoldenrodYellow;
            test.Values = new ChartValues<int> { 0, 0, 1, 1, 1, 0 };
            return test;
        }
        public StepLineSeries Mock2(string title)
        {
            StepLineSeries test = new StepLineSeries();
            test.Title = title;
            test.Stroke = Brushes.Blue;
            test.AlternativeStroke = Brushes.LightSkyBlue;
            test.Values = new ChartValues<int> { 0, 0, 0, 1, 1, 0 };
            return test;
        }


        private void PopulateSignals(string substationGid)
        {
            List<SignalListItemViewModel> tempList = new List<SignalListItemViewModel>();
            Substation selectedSub = substations.Where(x => x.Value.Gid == substationGid).FirstOrDefault().Value;

            foreach(var dis in selectedSub.Disconectors)
            {
                tempList.Add(new SignalListItemViewModel()
                {
                    Name = dis.Name,
                    Gid = dis.DiscreteGID
                });
            }

            foreach (var breaker in selectedSub.Breakers)
            {
                tempList.Add(new SignalListItemViewModel()
                {
                    Name = breaker.Name,
                    Gid = breaker.DiscreteGID
                });
            }

            foreach (var asyncMach in selectedSub.AsynchronousMachines)
            {
                tempList.Add(new SignalListItemViewModel()
                {
                    Name = asyncMach.Name,
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
