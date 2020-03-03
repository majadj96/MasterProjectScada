using System.Collections.ObjectModel;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Model;

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
            EventItems = new ObservableCollection<Event>();
            //TODO get events from scada
        }
    }
}
