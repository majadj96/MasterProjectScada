using GalaSoft.MvvmLight.Messaging;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    class CommandBreakerViewModel : BindableBase
    {
        public MyICommand Command { get; private set; }

        #region Variables
        private Breaker breaker;
        private bool newState;
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

        public CommandBreakerViewModel(Breaker breaker)
        {
            BreakerCurrent = new Breaker(breaker.MRID, breaker.GID, breaker.Name, breaker.Description, breaker.Time, breaker.State);
            NewState = Converter.ConvertToBool(BreakerCurrent.State);

            Command = new MyICommand(CommandBreaker);
        }

        public void CommandBreaker()
        {
            BreakerCurrent.NewState = Converter.ConvertToDiscreteState(NewState);

            Messenger.Default.Send(new NotificationMessage("command", BreakerCurrent, "Breaker"));
        }
    }
}
