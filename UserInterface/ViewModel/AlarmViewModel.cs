﻿using System.Collections.ObjectModel;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    public class AlarmViewModel : BindableBase
    {
        public MyICommand<int> AcknowledgeAlarmCommand { get; private set; }

        private ObservableCollection<Alarm> alarmItems = new ObservableCollection<Alarm>();

        public ObservableCollection<Alarm> AlarmItems
        {
            get { return alarmItems; }
            set { alarmItems = value; OnPropertyChanged("AlarmItems"); }
        }

        public AlarmViewModel()
        {
            AcknowledgeAlarmCommand = new MyICommand<int>(AcknowledgeAlarm);
        }

        public void AcknowledgeAlarm(int id)
        {
            //TODo send to SCADA ack alarm
        }
    }
}