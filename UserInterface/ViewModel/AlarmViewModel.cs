using System.Collections.ObjectModel;
using UserInterface.BaseError;
using UserInterface.Command;
using Common.AlarmEvent;
using UserInterface.Model;
using System.Linq;
using UserInterface.ProxyPool;
using System;
using Common;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

namespace UserInterface.ViewModel
{
    public class AlarmViewModel : BindableBase
    {
        private AlarmHandler alarmHandler;
        public MyICommand<int> AcknowledgeAlarmCommand { get; private set; }

        private ObservableCollection<Alarm> alarmItems = new ObservableCollection<Alarm>();
        private ObservableCollection<string> visible = new ObservableCollection<string>();
        private List<Substation> substations = new List<Substation>();

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

        public AlarmViewModel(AlarmHandler alarmHandler, List<Substation> substations)
        {
            this.alarmHandler = alarmHandler;
            this.substations = substations;

            AlarmItems = new ObservableCollection<Alarm>(this.alarmHandler.Alarms);
            this.alarmHandler.UpdateAlarmsCollection += AlarmHandler_UpdateAlarmsCollection;

            AcknowledgeAlarmCommand = new MyICommand<int>(AcknowledgeAlarm);

            //FillList();
        }

        private void FillList ()
        {
            foreach(Substation s in substations)
            {
                foreach (Disconector vv in s.Disconectors)
                {
                    foreach (Alarm a in AlarmItems)
                    {
                        if(a.GiD == vv.DiscreteGID)
                        {
                            a.PointName = vv.Name;
                        }
                    }
                }
                foreach (Breaker vv in s.Breakers)
                {
                    foreach (Alarm a in AlarmItems)
                    {
                        if (a.GiD == vv.DiscreteGID)
                        {
                            a.PointName = vv.Name;
                        }
                    }
                }
                /*foreach (Alarm a in AlarmItems)
                {
                    if (a.GiD == s.TapChanger.Gid)
                    {
                        a.PointName = s.TapChanger.Name;
                    }
                }*/
                /*foreach (AsynchronousMachine vv in s.AsynchronousMachines)
                {
                    foreach (Alarm a in AlarmItems)
                    {
                        if (a.GiD == vv.gid)
                        {
                            a.PointName = vv.Name;
                        }
                    }
                }*/
                foreach (Alarm a in AlarmItems)
                {
                    if (a.GiD == s.Transformator.AnalogTapChangerGID)
                    {
                        a.PointName = s.Transformator.Name;
                    }
                }
            }
        }

        private void AlarmHandler_UpdateAlarmsCollection(object sender, EventArgs e)
        {
            AlarmItems = new ObservableCollection<Alarm>(this.alarmHandler.Alarms);
            //FillList();
        }

        public void AcknowledgeAlarm(int id)
        {
            Alarm a = AlarmItems.Where(x => x.ID == id).FirstOrDefault();

            if (a.AlarmAck)
                return;

            a.Username = "Kris";
            a.AlarmAcknowledged = DateTime.Now;
            if (ProxyServices.AlarmEventServiceProxy.AcknowledgeAlarm(a))
            {
                //AlarmItems = new ObservableCollection<Alarm>(ProxyServices.AlarmEventServiceProxy.GetAllAlarms());

                Event e = new Event()
                {
                    EventReported = DateTime.Now,
                    EventReportedBy = AlarmEventType.UI,
                    GiD = a.GiD,
                    Message = "Alarm acknowledged",
                    PointName = a.PointName
                };
                ProxyServices.AlarmEventServiceProxy.AddEvent(e);
            }
        }
    }
}
