using System.Collections.ObjectModel;
using UserInterface.BaseError;
using UserInterface.Command;
using Common.AlarmEvent;
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    public class AlarmViewModel : BindableBase
    {
        public MyICommand<int> AcknowledgeAlarmCommand { get; private set; }

        private ObservableCollection<Alarm> alarmItems = new ObservableCollection<Alarm>();
        private ObservableCollection<string> visible = new ObservableCollection<string>();

        public ObservableCollection<Alarm> AlarmItems
        {
            get { return alarmItems; }
            set { alarmItems = value; OnPropertyChanged("AlarmItems"); }
        }
        public ObservableCollection<string> Visible
        {
            get { return visible; }
            set { visible = value; OnPropertyChanged("Visible"); }
        }

        public AlarmViewModel()
        {
            AlarmItems = new ObservableCollection<Alarm>(ProxyPool.ProxyServices.AlarmEventServiceProxy.GetAllAlarms());

            AcknowledgeAlarmCommand = new MyICommand<int>(AcknowledgeAlarm);
        }

        public void AcknowledgeAlarm(int id)
        {
            //TODo send to SCADA ack alarm
        }
    }
}
