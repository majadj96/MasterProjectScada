using Common;
using Common.GDA;
using GalaSoft.MvvmLight.Messaging;
using PubSubCommon;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
