using System.Collections.ObjectModel;
using UserInterface.BaseError;
using Common.AlarmEvent;

namespace UserInterface.ViewModel
{
    public class EventTableViewModel : BindableBase
    {
        private ObservableCollection<Event> eventItems = new ObservableCollection<Event>();

        public ObservableCollection<Event> EventItems
        {
            get { return eventItems; }
            set { eventItems = value; OnPropertyChanged("EventItems"); }
        }

        public EventTableViewModel()
        {
            EventItems = new ObservableCollection<Event>(ProxyPool.ProxyServices.AlarmEventServiceProxy.GetAllEvents());
        }
    }
}
