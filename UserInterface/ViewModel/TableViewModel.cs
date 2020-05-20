using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UserInterface.BaseError;
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    public class TableViewModel : BindableBase
    {
        public BindingList<UIModel> substationItems = new BindingList<UIModel>();

        public BindingList<UIModel> SubstationItems
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
            SubstationItems = new BindingList<UIModel>(model);

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                if (message.Notification == "UpdateTelemetry")
                    UpdateTelemetry((string)message.Sender, message.Target);
            });
        }

        private void UpdateTelemetry(string type, object target)
        {
            Breaker b = null;
            Disconector d = null;
            UIModel t = null;

            if (type.Contains("Breaker"))
            {
                b = (Breaker)target;
            }
            else if (type.Contains("Disconector"))
            {
                d = (Disconector)target;
            }
            else if (type.Contains("TapChanger"))
            {
                t = (UIModel)target;
            }

            int i = 0;
            foreach (UIModel model in SubstationItems)
            {
                if (b != null)
                {
                    if (model.GID == b.GID)
                    {
                        model.Value = b.State.ToString();
                        model.Time = DateTime.Now.ToString();
                        SubstationItems.RaiseListChangedEvents = true;
                        SubstationItems.ResetItem(i);
                        break;
                    }
                }
                if (d != null)
                {
                    if (model.GID == d.GID)
                    {
                        model.Value = d.State.ToString();
                        model.Time = DateTime.Now.ToString();
                        SubstationItems.RaiseListChangedEvents = true;
                        SubstationItems.ResetItem(i);
                        break;
                    }
                }
                if (t != null)
                {
                    if (model.GID == t.GID)
                    {
                        model.Value = t.Value.ToString();
                        model.Time = DateTime.Now.ToString();
                        SubstationItems.RaiseListChangedEvents = true;
                        SubstationItems.ResetItem(i);
                        break;
                    }
                }
                i++;
            }
        }
    }
}
