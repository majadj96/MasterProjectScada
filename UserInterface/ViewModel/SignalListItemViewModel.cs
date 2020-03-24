using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInterface.BaseError;

namespace UserInterface.ViewModel
{
    public class SignalListItemViewModel: BindableBase
    {
        private bool isChecked;

        public long Gid { get; set; }
        public string Name { get; set; }

        public bool IsChecked
        {
            get => isChecked;
            set => NotifyChecked(ref isChecked, value);
        }

        private void NotifyChecked(ref bool isChecked, bool newValue)
        {
            if (isChecked != newValue)
            {
                SetProperty(ref isChecked, newValue);
                Messenger.Default.Send(this);
            }
        }

    }
}
