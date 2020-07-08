﻿using GalaSoft.MvvmLight.Messaging;
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
        public List<string> MeasureType { get; set; }
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
                signalsOn.Clear();
                SeriesCollection.Clear();
                if(SignalList != null)
                    SignalList.Clear();
                if(selectedType == "Analog" || selectedType == "Digital")
                    PopulateSignals(selectedSubstation.Gid, selectedType);
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
        private string selectedType;

        private IMeasurementRepository measurementProxy;

        public AnalyticsWindowViewModel(Dictionary<long, Substation> substations, IMeasurementRepository measurementProxy)
        {
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
                foreach (var dis in selectedSub.Disconectors)
                {
                    tempList.Add(new SignalListItemViewModel()
                    {
                        Name = dis.Name,
                        Gid = dis.DiscreteGID,
                        Type = "State"
                    });
                }

                foreach (var breaker in selectedSub.Breakers)
                {
                    tempList.Add(new SignalListItemViewModel()
                    {
                        Name = breaker.Name,
                        Gid = breaker.DiscreteGID,
                        Type = "State"
                    });
                }
            } else
            {
                tempList
                    .AddRange(selectedSub.AsynchronousMachines
                        .Select(x => new SignalListItemViewModel
                            {
                                Name = x.Name,
                                Gid = x.SignalGid,
                                Type = "Power"
                            }));

                tempList.Add(new SignalListItemViewModel()
                {
                    Name = "Tap Changer",
                    Gid = long.Parse(selectedSub.TapChanger.GID),
                    Type = "Position"
                });

                tempList.Add(new SignalListItemViewModel()
                {
                    Name = "Primary winding",
                    Gid = selectedSub.Transformator.TransformerWindings[0],
                    Type = "Voltage"
                });

                tempList.Add(new SignalListItemViewModel()
                {
                    Name = "Primary winding",
                    Gid = selectedSub.Transformator.TransformerWindings[1],
                    Type = "Current"
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
                //PopulateSignals(selectedSubstation.Gid);
            }
        }

		void AnalyticsWindow_Closing(object sender, CancelEventArgs e)
		{
			signalsOn = new Dictionary<long, StepLineSeries>();
			Messenger.Default.Unregister<SignalListItemViewModel>(this);
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

