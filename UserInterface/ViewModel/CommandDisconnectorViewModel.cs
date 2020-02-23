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
        private Disconector disconector;
        private string type;
        private bool newState;
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
        #endregion

        public CommandDisconnectorViewModel(Disconector disconector, string type)
        {
            DisconectorCurrent = new Disconector(disconector.MRID, disconector.GID, disconector.Name, disconector.Description, disconector.Time, disconector.State);
            NewState = !Converter.ConvertToBool(DisconectorCurrent.State);

            this.type = type;

            Command = new MyICommand(CommandDisconnector);
        }

        public void CommandDisconnector()
        {
            DisconectorCurrent.NewState = Converter.ConvertToDiscreteState(NewState);

            Messenger.Default.Send(new NotificationMessage("command", DisconectorCurrent, "Disconector" + type));
        }
    }
}
