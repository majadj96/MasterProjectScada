using GalaSoft.MvvmLight.Messaging;
using UserInterface.BaseError;
using UserInterface.Command;

namespace UserInterface.ViewModel
{
    class CommandBreakerViewModel : BindableBase
    {
        public MyICommand Command { get; private set; }

        #region Variables
        private string breakerName;
        private string breakerDescription;
        private int currentValue;
        private int newValue;
        #endregion

        #region Props
        public string BreakerName
        {
            get { return breakerName; }
            set { SetProperty(ref breakerName, value); }
        }
        public string BreakerDescription
        {
            get { return breakerDescription; }
            set { SetProperty(ref breakerDescription, value); }
        }
        public int CurrentValue
        {
            get { return currentValue; }
            set { SetProperty(ref currentValue, value); }
        }
        public int NewValue
        {
            get { return newValue; }
            set { SetProperty(ref newValue, value); }
        }
        #endregion

        public CommandBreakerViewModel()
        {
            Command = new MyICommand(CommandBreaker);
        }

        public void SetName(string name)
        {
            BreakerName = name;
            BreakerDescription = "Description";
        }

        public void CommandBreaker()
        {
            Messenger.Default.Send(NewValue);
        }
    }
}
