using Common.AlarmEvent;
using GalaSoft.MvvmLight.Messaging;
using ScadaCommon.ComandingModel;
using System;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Converters;
using UserInterface.Model;
using UserInterface.ProxyPool;
using System.Windows;

namespace UserInterface.ViewModel
{
    public class CommandDisconnectorViewModel : BindableBase
    {
        public MyICommand Command { get; private set; }

        #region Variables
        private Disconector disconector;
        private string type;
        private bool newState;
        private string inAlarmSource;
        private string autoCommandedSource;
        private string operatorCommandedSource;
        #endregion

        #region Props
        public Disconector DisconectorCurrent
        {
            get { return disconector; }
            set { disconector = value; OnPropertyChanged("DisconectorCurrent"); }
        }
        public bool NewState
        {
            get { return newState; }
            set { newState = value; OnPropertyChanged("NewState"); }
        }
        public string InAlarmSource
        {
            get => inAlarmSource;
            set { inAlarmSource = value; OnPropertyChanged("InAlarmSource"); }
        }
        public string AutoCommandedSource
        {
            get => autoCommandedSource;
            set { autoCommandedSource = value; OnPropertyChanged("AutoCommandedSource"); }
        }
        public string OperatorCommandedSource
        {
            get => operatorCommandedSource;
            set { operatorCommandedSource = value; OnPropertyChanged("OperatorCommandedSource"); }
        }
        #endregion

        public CommandDisconnectorViewModel(Disconector disconector, string type)
        {
            DisconectorCurrent = disconector;
            NewState = !ConverterState.ConvertToBool(DisconectorCurrent.State);

            this.type = type;

            SetPictures();

            Command = new MyICommand(CommandDisconnector);
        }

        private void SetPictures()
        {
            if (DisconectorCurrent.InAlarm)
                InAlarmSource = "../Assets/fire-alarm.png";
            else
                InAlarmSource = String.Empty;

            if (DisconectorCurrent.OperaterCommanded)
                OperatorCommandedSource = "../Assets/robot.png";
            else
                OperatorCommandedSource = String.Empty;

            if (DisconectorCurrent.AutoCommanded)
                AutoCommandedSource = "../Assets/automation.png";
            else
                AutoCommandedSource = String.Empty;
        }

        public void CommandDisconnector()
        {
            DisconectorCurrent.NewState = ConverterState.ConvertToDiscreteState(NewState);

            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)DisconectorCurrent.NewState, SignalGid = DisconectorCurrent.DiscreteGID };
            var v = ProxyServices.CommandingServiceProxy.WriteDigitalOutput(commandObject);
            if (v == ScadaCommon.CommandResult.Success)
            {
                DisconectorCurrent.State = DisconectorCurrent.NewState;

                Messenger.Default.Send(new NotificationMessage("command", DisconectorCurrent, "Disconector" + type));

                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(DisconectorCurrent.GID), Message = "Commanding disconnector.", PointName = DisconectorCurrent.Name };
                ProxyServices.AlarmEventServiceProxy.AddEvent(e);

                foreach (Window item in Application.Current.Windows)
                {
                    if (item.DataContext != null)
                    {
                        if (item.DataContext.ToString() == "UserInterface.CommandingWindowViewModel")
                        {
                            item.Close();
                            break;
                        }
                    }
                }
            }
        }
    }
}
