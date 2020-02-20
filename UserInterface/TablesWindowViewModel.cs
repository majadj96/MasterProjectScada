using Common;
using Common.GDA;
using GalaSoft.MvvmLight.Messaging;
using PubSubCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Model;
using UserInterface.Subscription;
using UserInterface.ViewModel;

namespace UserInterface
{
    public class TablesWindowViewModel : BindableBase
    {
        private TableViewModel tableViewModel = new TableViewModel();
        private AlarmViewModel alarmViewModel = new AlarmViewModel();

        #region Variables
        private BindableBase currentTableViewModel;
        #endregion

        #region Props
        public BindableBase CurrentTableViewModel
        {
            get { return currentTableViewModel; }
            set { SetProperty(ref currentTableViewModel, value); }
        }
        #endregion

        public TablesWindowViewModel()
        {
            CurrentTableViewModel = tableViewModel;

           // Messenger.Default.Register<NotificationMessage>(tableViewModel, (message) => { tableViewModel.PopulateModel(message.Target); });

        }

        public void SetView(string window)
        {
            if (string.Compare(window, "Summary") == 0)
            {
                CurrentTableViewModel = tableViewModel;
            }
            else if (string.Compare(window, "Alarm") == 0)
            {
                CurrentTableViewModel = alarmViewModel;
            }
            else if (string.Compare(window, "Event") == 0)
            {
                //CurrentTableViewModel = eventViewModel;
            }
        }
    }
}
