using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using UserInterface.BaseError;
using UserInterface.Model;
using UserInterface.ViewModel;
using LiveCharts.Configurations;
using System.ComponentModel;
using System.Windows;
using UserInterface.Command;
using LiveCharts.Definitions.Series;
using System.Threading.Tasks;
using Common;
using UserInterface.Classes;

namespace UserInterface
{
    class AnalyticsWindowViewModel : BindableBase
    {
        
        private bool isDiscreteSelected;
        private bool _isBussy;
        private static volatile bool _isFirstDbCall = true;
        private string selectedType;
        private int _dummyStatus;

        private List<SignalListItemViewModel> signalList;
        public List<Substation> SubstationList { get; set; }
        public List<string> MeasureType { get; set; }

        public Dictionary<long, Substation> substations;
        private Dictionary<long, Measurement> measurements = new Dictionary<long, Measurement>();
        public Dictionary<long, StepLineSeries> signalsOn;
        public SeriesCollection SeriesCollection { get; set; }
        private ObservableCollection<RadioButton> radioButtons = new ObservableCollection<RadioButton>();
        public Func<double, string> Formatter { get; set; }

        private DateTime initialDateTime;
		public DateTime InitialDateTime { get { return initialDateTime; } set { initialDateTime = value; OnPropertyChanged(nameof(InitialDateTime)); } }
		private DateTime maxDateTime;
		public DateTime MaxDateTime { get { return maxDateTime; } set { maxDateTime = value; OnPropertyChanged(nameof(MaxDateTime)); } }
        private DateTime? startDate = null;
        private DateTime? endDate = null;
        private Substation selectedSubstation;

        private Messenger messenger = new Messenger();
        private IMeasurementRepository measurementProxy;
        public int DummyStatus
        {
            get => _dummyStatus;
            set
            {
                SetProperty(ref _dummyStatus, value);
            }
        }
        public bool IsBussy
        {
            get => _isBussy;
            set
            {
                SetProperty(ref _isBussy, value);
            }
        }
        public bool IsDiscreteSelected
        {
            get => isDiscreteSelected;
            set
            {
                SetProperty(ref isDiscreteSelected, value);
            }
        }
        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                PopulateSignals(selectedSubstation.Gid, selectedType);
                signalsOn.Clear();
                SeriesCollection.Clear();
            }
        }
        public Substation SelectedSubstation
        {
            get { return selectedSubstation; }
            set
            {
                selectedSubstation = value;
                signalsOn.Clear();
                SeriesCollection.Clear();
                if(SignalList != null)
                    SignalList.Clear();
                if(selectedType == "Analog" || selectedType == "Digital")
                    PopulateSignals(selectedSubstation.Gid, selectedType);
            }
        }
        public List<SignalListItemViewModel> SignalList
        {
            get => signalList;
            set => SetProperty(ref signalList, value);
        }
        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; OnPropertyChanged("StartDate"); InitialDateTime = startDate.Value; }
        }
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged("EndDate"); MaxDateTime = endDate.Value; }
        }
        
        public AnalyticsWindowViewModel(Dictionary<long, Substation> substations, IMeasurementRepository measurementProxy, Dictionary<long, Measurement> measurements)
        {
            this.measurements = measurements;
            foreach (var item in Application.Current.Windows)
			{
				if(item.GetType() == typeof(AnalyticsWindow))
				{
					((Window)item).Closing += new CancelEventHandler(AnalyticsWindow_Closing);
				}
			}
			signalsOn = new Dictionary<long, StepLineSeries>();
			this.substations = substations;
			SetChartModelValues();
            this.measurementProxy = measurementProxy;
        }

        //private void OnZoom(string destination)
        //{
        //    List<ChartPoint> values = new List<ChartPoint>();
        //    foreach(var series in SeriesCollection)
        //    {
        //        values.AddRange(series.ActualValues.GetPoints(series));
        //    }

        //    values.OrderBy(x => x.X);
        //    var maxDateTime = new DateTime((long)values.Last().X);

        //    InitialDateTime = maxDateTime.AddHours(-5);
        //}
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
			this.Formatter = value => new DateTime((long)value).ToString("yyyy-MM-dd HH:mm:ss");
		}

        public void Setup()
        {
            SubstationList = substations.Values.ToList();
            MeasureType = new List<string> { "Digital", "Analog" };
        }

        public void Set()
        {
            Messenger.Default.Register<SignalListItemViewModel>(this, async (message) => { await HandleSignalChecked(message); });
        }

        private async Task HandleSignalChecked(SignalListItemViewModel signal)
        {
            if (signal.IsChecked)
            {
                RepositoryCore.Measurement[] measurements = default;
                if (StartDate != null && EndDate != null)
                {
                    DateTime start = StartDate ?? DateTime.Now;
                    DateTime end = EndDate ?? DateTime.Now;

                    this.InitialDateTime = start;
                    this.MaxDateTime = end;
                    await ExecuteSafely(() => measurements = measurementProxy.GetAllMeasurementsByTime(start, end, signal.Gid), _isFirstDbCall);
                }
                else
                {
                    await ExecuteSafely(() => measurements = measurementProxy.GetAllMeasurementsByGid(signal.Gid), _isFirstDbCall);
                }

                StepLineSeries line = MakeSignal(signal.Name, measurements);
                if (line.Values.Count == 0)
                    return;
                SeriesCollection.Add(line);
                signalsOn.Add(signal.Gid, line);
            }
            else
            {
                if (!signalsOn.ContainsKey(signal.Gid))
                    return;
                
                if (!SeriesCollection.Contains(signalsOn[signal.Gid]))
                    return;

                SeriesCollection.Remove(signalsOn[signal.Gid]);
                signalsOn.Remove(signal.Gid);
            }
            _isFirstDbCall = false;
        }

        public StepLineSeries MakeSignal(string title, RepositoryCore.Measurement[] measurements)
        {
			StepLineSeries line = new StepLineSeries();
            line.Title = title;
            line.AlternativeStroke = Brushes.LightGray;
            if (selectedType == "Digital")
                line.PointGeometry = DefaultGeometries.Square;

            ChartValues<ChartModel> chartValues = new ChartValues<ChartModel>();
            measurements = measurements.OrderBy(m => m.ChangedTime.Value).ToArray();
			DateTime now = DateTime.Now;

			foreach (var measure in measurements)
			{
				DateTime dt = new DateTime(measure.ChangedTime.Value.Ticks);
				chartValues.Add(new ChartModel(measure.ChangedTime.Value, measure.Value));
			}

            if (chartValues.Count != 0)
            {
                this.InitialDateTime = chartValues.FirstOrDefault()?.DateTime ?? chartValues.LastOrDefault()?.DateTime.AddSeconds(-10) ?? DateTime.Now;
                this.MaxDateTime = chartValues.LastOrDefault()?.DateTime.AddSeconds(10) ?? chartValues.FirstOrDefault()?.DateTime.AddMinutes(10) ?? DateTime.Now;
            }
            line.Values = chartValues;
			return line;
        }

        private void PopulateSignals(string substationGid, string type)
        {
            List<SignalListItemViewModel> tempList = new List<SignalListItemViewModel>();
            Substation selectedSub = substations.Where(x => x.Value.Gid == substationGid).FirstOrDefault().Value;

            if (type.Equals("Digital")) {
                foreach (Measurement meas in measurements.Values)
                {
                    if ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid) == DMSType.ANALOG)
                        continue;

                    if(selectedSub.Transformator.GID == meas.PowerSystemResource.ToString()
                        || selectedSub.TapChanger.GID == meas.PowerSystemResource.ToString()
                        || selectedSub.Disconectors.Any(x => x.GID == meas.PowerSystemResource.ToString())
                        || selectedSub.Breakers.Any(x => x.GID == meas.PowerSystemResource.ToString())
                        || selectedSub.AsynchronousMachines.Any(x => x.GID == meas.PowerSystemResource.ToString())
                        || selectedSub.Transformator.TransformerWindings.Any(x => x.GID == meas.PowerSystemResource.ToString()))
                    {
                        tempList.Add(new SignalListItemViewModel()
                        {
                            Name = meas.Name,
                            Gid = meas.Gid,
                            Type = meas.Type.ToString()
                        });
                    }
                }
            } else
            {
                foreach (Measurement meas in measurements.Values)
                {
                    if ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid) == DMSType.DISCRETE)
                        continue;

                    if (selectedSub.Transformator.GID == meas.PowerSystemResource.ToString()
                        || selectedSub.TapChanger.GID == meas.PowerSystemResource.ToString()
                        || selectedSub.Disconectors.Any(x => x.GID == meas.PowerSystemResource.ToString())
                        || selectedSub.Breakers.Any(x => x.GID == meas.PowerSystemResource.ToString())
                        || selectedSub.AsynchronousMachines.Any(x => x.GID == meas.PowerSystemResource.ToString())
                        || selectedSub.Transformator.TransformerWindings.Any(x => x.GID == meas.PowerSystemResource.ToString())
                        || meas.PowerSystemResource.ToString() == selectedSub.Gid)
                    {
                        tempList.Add(new SignalListItemViewModel()
                        {
                            Name = meas.Name,
                            Gid = meas.Gid,
                            Type = meas.Type.ToString()
                        });
                    }
                }                
            }
			SignalList = tempList;
        }

		void AnalyticsWindow_Closing(object sender, CancelEventArgs e)
		{
			signalsOn = new Dictionary<long, StepLineSeries>();
			Messenger.Default.Unregister<SignalListItemViewModel>(this);
		}

        private async Task ExecuteSafely(Action action, bool isLongOperation)
        {
            try
            {
                IsBussy = true && isLongOperation;
                Task.Run(async () =>
                {
                    int counter = 0;
                    while (counter < 9)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            DummyStatus += 10;
                            
                        });
                        await Task.Delay(500);
                        counter++;
                    }
                });
                await Task.Run(action);
                App.Current.Dispatcher.Invoke(() =>
                {
                    DummyStatus = 100;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while accessing database. Reason: {ex}");
            }
            finally
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    DummyStatus = 0;
                });
                IsBussy = false;
            }
        }
    }
}

