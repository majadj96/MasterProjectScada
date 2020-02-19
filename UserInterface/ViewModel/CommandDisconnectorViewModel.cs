using GalaSoft.MvvmLight.Messaging;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    public class CommandDisconnectorViewModel : BindableBase
    {
        public MyICommand Command { get; private set; }

        #region Variables
        private string disconnectorName;
        private string disconnectorDescription;
        private int currentValue;
        private int newValue;
        #endregion

        #region Props
        public string DisconnectorName
        {
            get { return disconnectorName; }
            set { SetProperty(ref disconnectorName, value); }
        }
        public string DisconnectorDescription
        {
            get { return disconnectorDescription; }
            set { SetProperty(ref disconnectorDescription, value); }
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

        public CommandDisconnectorViewModel(Disconector disconector)
        {
            Command = new MyICommand(CommandDisconnector);
        }

        public void SetName(string name)
        {
            DisconnectorName = name;
            DisconnectorDescription = "Description";
        }

        public void CommandDisconnector()
        {
            Messenger.Default.Send(NewValue);
        }
    }
}
