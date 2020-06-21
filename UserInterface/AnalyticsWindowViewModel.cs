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
using LiveCharts.Configurations;
using System.ComponentModel;

namespace UserInterface
{
    class AnalyticsWindowViewModel : BindableBase
    {
        private bool isDiscreteSelected;
        private List<SignalListItemViewModel> signalList;

        public Dictionary<long, Substation> substations;

        public static Dictionary<long, StepLineSeries> signalsOn;
        public SeriesCollection SeriesCollection { get; set; }

        private ObservableCollection<RadioButton> radioButtons = new ObservableCollection<RadioButton>();

        public List<Substation> SubstationList { get; set; }
		private DateTime initialDateTime;
		public DateTime InitialDateTime { get { return initialDateTime; } set { initialDateTime = value; OnPropertyChanged(nameof(InitialDateTime)); } }
		private DateTime maxDateTime;
		public DateTime MaxDateTime { get { return maxDateTime; } set { maxDateTime = value; OnPropertyChanged(nameof(MaxDateTime)); } }
		public Func<double, string> Formatter { get; set; }

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
            set { startDate = value; OnPropertyChanged("StartDate"); InitialDateTime = startDate.Value; }
        }

        private DateTime? endDate = null;
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged("EndDate"); MaxDateTime = endDate.Value; }
        }


        private Messenger messenger = new Messenger();
        private Substation selectedSubstation;

        private IMeasurementRepository measurementProxy;

        public AnalyticsWindowViewModel(Dictionary<long, Substation> substations, IMeasurementRepository measurementProxy)
        {
			signalsOn = new Dictionary<long, StepLineSeries>();

			this.substations = substations;
			
			SetChartModelValues();

            this.measurementProxy = measurementProxy;
        }

		private void SetChartModelValues()
		{
			Setup();
			Set();
			var dayConfig = Mappers.Xy<ChartModel>()
							   .X(dayModel => dayModel.DateTime.Ticks)
							   .Y(dayModel => dayModel.Value);

			this.SeriesCollection = new SeriesCollection(dayConfig);

			this.InitialDateTime = DateTime.Now;
			this.MaxDateTime = DateTime.Now.AddDays(30);

			this.Formatter = value => new DateTime((long)value).ToString("yyyy-MM:dd HH:mm:ss");
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
                RepositoryCore.Measurement[] measurements;
                if (StartDate != null && EndDate != null)
                {
                    DateTime start = StartDate ?? DateTime.Now;
                    DateTime end = EndDate ?? DateTime.Now;

					this.InitialDateTime = start;
					this.MaxDateTime = end;

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

        public StepLineSeries MakeSignal(string title, RepositoryCore.Measurement[] measurements)
        {
			StepLineSeries line = new StepLineSeries();
            line.Title = title;
            //line.AlternativeStroke = Brushes.White;
            line.Stroke = Brushes.Red;

            ChartValues<ChartModel> chartValues = new ChartValues<ChartModel>();

			measurements.OrderBy(m => m.ChangedTime.Value);

			DateTime now = DateTime.Now;

			foreach (var measure in measurements)
			{
				DateTime dt = new DateTime(measure.ChangedTime.Value.Ticks);
				chartValues.Add(new ChartModel(measure.ChangedTime.Value, measure.Value));
			}

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

			tempList.Add(new SignalListItemViewModel()
			{
				Name = selectedSub.TapChanger.GID.ToString(),
				Gid = long.Parse(selectedSub.TapChanger.GID)
			});

			tempList.Add(new SignalListItemViewModel()
			{
				Name = selectedSub.Transformator.GID.ToString(),
				Gid = long.Parse(selectedSub.Transformator.GID)
			});

			foreach (var item in selectedSub.Transformator.TransformerWindings)
			{
				tempList.Add(new SignalListItemViewModel()
				{
					Name = item.ToString(),
					Gid = item
				});
			}

			SignalList = tempList;
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

	public class ChartModel
	{
		public DateTime DateTime { get; set; }
		public double Value { get; set; }

		public ChartModel(DateTime dateTime, double value)
		{
			this.DateTime = dateTime;
			this.Value = value;
		}
	}
}

