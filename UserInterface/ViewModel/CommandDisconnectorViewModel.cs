using Common.AlarmEvent;
using GalaSoft.MvvmLight.Messaging;
using ScadaCommon.ComandingModel;
using System;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Model;
using UserInterface.ProxyPool;

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
            DisconectorCurrent = disconector;
            NewState = !Converter.ConvertToBool(DisconectorCurrent.State);

            this.type = type;

            Command = new MyICommand(CommandDisconnector);
        }

        public void CommandDisconnector()
        {
            DisconectorCurrent.NewState = Converter.ConvertToDiscreteState(NewState);

            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)DisconectorCurrent.NewState, SignalGid = DisconectorCurrent.DiscreteGID };
            var v = ProxyServices.CommandingServiceProxy.WriteDigitalOutput(commandObject);
            if (v == ScadaCommon.CommandResult.Success)
            {
                Messenger.Default.Send(new NotificationMessage("command", DisconectorCurrent, "Disconector" + type));

                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(DisconectorCurrent.GID), Message = "Commanding disconnector.", PointName = DisconectorCurrent.Name };
                ProxyServices.AlarmEventServiceProxy.AddEvent(e);
            }
        }
    }
}
