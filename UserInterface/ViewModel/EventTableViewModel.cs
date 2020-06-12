using System.Collections.ObjectModel;
using UserInterface.BaseError;
using Common.AlarmEvent;
using System;
using UserInterface.Command;
using System.Linq;

namespace UserInterface.ViewModel
{
    public class EventTableViewModel : BindableBase
    {
        private CustomEventHandler customEventHandler;
        public MyICommand<string> FilterCommand { get; private set; }

        #region Variables
        private ObservableCollection<Event> eventItems = new ObservableCollection<Event>();

        private DateTime dateFrom;
        private DateTime dateTo;
        #endregion

        #region Props
        public ObservableCollection<Event> EventItems
        {
            get { return eventItems; }
            set { eventItems = value; OnPropertyChanged("EventItems"); }
        }

        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; OnPropertyChanged("DateFrom"); }
        }
        public DateTime DateTo
        {
            get { return dateTo; }
            set { dateTo = value; OnPropertyChanged("DateTo"); }
        }
        #endregion

        public EventTableViewModel(CustomEventHandler customEventHandler)
        {
            this.customEventHandler = customEventHandler;
            EventItems = new ObservableCollection<Event>(customEventHandler.Events);

            this.customEventHandler.UpdateEventsCollection += CustomEventHandler_UpdateEventsCollection;

            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;

            FilterCommand = new MyICommand<string>(ApplyFilter);
        }

        private void CustomEventHandler_UpdateEventsCollection(object sender, EventArgs e)
        {
            EventItems = new ObservableCollection<Event>(customEventHandler.Events);
        }

        public void ApplyFilter(string option)
        {
            if(string.Compare(option, "Apply") == 0)
            {
                if (DateTime.Compare(DateFrom, DateTo) > 0)
                    DateFrom = DateTime.Now;
                if (DateTime.Compare(DateTo, DateFrom) < 0)
                    DateTo = DateTime.Now;

                EventItems = new ObservableCollection<Event>(customEventHandler.Events.Where(x => DateTime.Compare(DateFrom.Date, x.EventReported.Date) <= 0 &&
                                                                                   DateTime.Compare(x.EventReported.Date, DateTo.Date) <= 0));
            }
            else if (string.Compare(option, "Disable") == 0)
            {
                EventItems = new ObservableCollection<Event>(customEventHandler.Events);
            }
        }
    }
}
