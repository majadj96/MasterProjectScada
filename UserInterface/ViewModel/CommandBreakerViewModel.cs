using Common;
using Common.AlarmEvent;
using GalaSoft.MvvmLight.Messaging;
using ScadaCommon.ComandingModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Converters;
using UserInterface.Model;
using UserInterface.ProxyPool;

namespace UserInterface.ViewModel
{
    class CommandBreakerViewModel : BindableBase
    {
        public MyICommand Command { get; private set; }
        public MyICommand AutoModeCommand { get; private set; }
        public MyICommand ManualModeCommand { get; private set; }

        #region Variables
        private Breaker breaker;
        private Measurement measurement;
        private bool newState;
        private string type;
        private string inAlarmSource;
        private string autoCommandedSource;
        private string operatorCommandedSource;
        private Brush autoModeBackground;
        private Brush manualModeBackground;
        #endregion

        #region Props
        public Breaker BreakerCurrent
        {
            get { return breaker; }
            set { breaker = value; OnPropertyChanged("Breaker"); }
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
        public Brush AutoModeBackground
        {
            get => autoModeBackground;
            set { autoModeBackground = value; OnPropertyChanged("AutoModeBackground"); }
        }
        public Brush ManualModeBackground
        {
            get => manualModeBackground;
            set { manualModeBackground = value; OnPropertyChanged("ManualModeBackground"); }
        }
        #endregion

        public CommandBreakerViewModel(Breaker breaker, string type, Dictionary<long, Measurement> measurements)
        {
            BreakerCurrent = breaker;
            NewState = !ConverterState.ConvertToBool(BreakerCurrent.State);

            this.type = type;

            SetPictures();

            if (measurements.TryGetValue(BreakerCurrent.DiscreteGID, out Measurement meas))
            {
                this.measurement = meas;
                SetOpModeBindings(meas.OperationMode);
            }

            Command = new MyICommand(CommandBreaker);
            AutoModeCommand = new MyICommand(CommandAutoMode);
            ManualModeCommand = new MyICommand(CommandManualMode);
        }

        private void CommandManualMode()
        {
            if (ProxyServices.CommandingServiceProxy.SetPointOperationMode(this.measurement.Gid, OperationMode.MANUAL))
            {
                this.measurement.OperationMode = OperationMode.MANUAL;
            }

            SetOpModeBindings(OperationMode.MANUAL);
        }

        private void CommandAutoMode()
        {
            if (ProxyServices.CommandingServiceProxy.SetPointOperationMode(this.measurement.Gid, OperationMode.AUTO))
            {
                this.measurement.OperationMode = OperationMode.AUTO;
            }

            SetOpModeBindings(OperationMode.AUTO);
        }

        private void SetOpModeBindings(OperationMode opMode)
        {
            switch (opMode)
            {
                case OperationMode.AUTO:
                    AutoModeBackground = Brushes.OrangeRed;
                    ManualModeBackground = SystemColors.ControlBrush;
                    break;
                case OperationMode.MANUAL:
                    ManualModeBackground = Brushes.OrangeRed;
                    AutoModeBackground = SystemColors.ControlBrush;
                    break;
                default:
                    AutoModeBackground = SystemColors.ControlBrush;
                    ManualModeBackground = SystemColors.ControlBrush;
                    break;
            }
        }

        private void SetPictures()
        {
            if (BreakerCurrent.InAlarm)
                InAlarmSource = "../Assets/fire-alarm.png";
            else
                InAlarmSource = String.Empty;

            if (BreakerCurrent.OperaterCommanded)
                OperatorCommandedSource = "../Assets/robot.png";
            else
                OperatorCommandedSource = String.Empty;

            if (BreakerCurrent.AutoCommanded)
                AutoCommandedSource = "../Assets/automation.png";
            else
                AutoCommandedSource = String.Empty;
        }

        public void CommandBreaker()
        {
            if (this.measurement.OperationMode != OperationMode.MANUAL)
            {
                string messageText = "Operation mode will be set to Manual. Are you sure you want to execute command?";
                MessageBoxResult result = MessageBox.Show(messageText, "Alert", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    CommandManualMode();
                }
            }

            BreakerCurrent.NewState = ConverterState.ConvertToDiscreteState(NewState);

            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)BreakerCurrent.NewState, SignalGid = BreakerCurrent.DiscreteGID };
            var v = ProxyServices.CommandingServiceProxy.WriteDigitalOutput(commandObject);
            if (v == ScadaCommon.CommandResult.Success)
            {
                BreakerCurrent.State = BreakerCurrent.NewState;
                Messenger.Default.Send(new NotificationMessage("command", BreakerCurrent, "Breaker" + type));

                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(BreakerCurrent.GID), Message = "Commanding breaker.", PointName = BreakerCurrent.Name  };
                ProxyServices.AlarmEventServiceProxy.AddEvent(e);

                foreach(Window item in Application.Current.Windows)
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
