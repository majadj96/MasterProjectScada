using System.Collections.ObjectModel;
using UserInterface.BaseError;
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    public class TableViewModel : BindableBase
    {
        public ObservableCollection<UIModel> substationItems = new ObservableCollection<UIModel>();

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

        public TableViewModel(ObservableCollection<UIModel> model)
        {
            SubstationItems = model;
        }
    }
}
