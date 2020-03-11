using Common.AlarmEvent;
using GalaSoft.MvvmLight.Messaging;
using ScadaCommon.ComandingModel;
using System;
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

        #region Variables
        private Breaker breaker;
        private bool newState;
        private string type;
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
        #endregion

        public CommandBreakerViewModel(Breaker breaker, string type)
        {
            BreakerCurrent = breaker;
            NewState = !ConverterState.ConvertToBool(BreakerCurrent.State);

            this.type = type;

            Command = new MyICommand(CommandBreaker);
        }

        public void CommandBreaker()
        {
            BreakerCurrent.NewState = ConverterState.ConvertToDiscreteState(NewState);

            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)BreakerCurrent.NewState, SignalGid = BreakerCurrent.DiscreteGID };
            var v = ProxyServices.CommandingServiceProxy.WriteDigitalOutput(commandObject);
            if (v == ScadaCommon.CommandResult.Success)
            {
                Messenger.Default.Send(new NotificationMessage("command", BreakerCurrent, "Breaker" + type));

                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(BreakerCurrent.GID), Message = "Commanding breaker.", PointName = BreakerCurrent.Name  };
                ProxyServices.AlarmEventServiceProxy.AddEvent(e);
            }
        }
    }
}
