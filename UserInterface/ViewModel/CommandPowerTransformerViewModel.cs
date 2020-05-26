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
    public class CommandPowerTransformerViewModel : BindableBase
    {
        public MyICommand<string> CurrentVoltageCommand { get; private set; }

        #region Variables
        private Transformator transformator;
        #endregion

        #region Props
        public Transformator TranformerCurrent
        {
            get { return transformator; }
            set { transformator = value; OnPropertyChanged("TranformerCurrent"); }
        }
        #endregion

        public CommandPowerTransformerViewModel(Transformator transformator)
        {
            TranformerCurrent = transformator;

            CurrentVoltageCommand = new MyICommand<string>(CommandCurrentVoltage);
        }

        public void CommandCurrentVoltage(string type)
        {
            switch(type)
            {
                case "TapChangerUp":
                    {
                        TranformerCurrent.TapChangerValue++;
                        if (TranformerCurrent.TapChangerValue <= TranformerCurrent.MaxValueTapChanger)
                        {
                            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)TranformerCurrent.TapChangerValue, SignalGid = TranformerCurrent.AnalogTapChangerGID };
                            var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                            if (v == ScadaCommon.CommandResult.Success)
                            {
                                Messenger.Default.Send(new NotificationMessage("commandTransformer", TranformerCurrent, "TapChangerUp"));

                                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(TranformerCurrent.GID), Message = "Commanding transformer TapChanger up.", PointName = TranformerCurrent.Name };
                                ProxyServices.AlarmEventServiceProxy.AddEvent(e);
                            }
                        }
                    }
                    break;
                case "TapChangerDown":
                    {
                        TranformerCurrent.TapChangerValue--;
                        if (TranformerCurrent.TapChangerValue >= TranformerCurrent.MinValueTapChanger)
                        {
                            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)TranformerCurrent.TapChangerValue, SignalGid = TranformerCurrent.AnalogTapChangerGID };
                            var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                            if (v == ScadaCommon.CommandResult.Success)
                            {
                                Messenger.Default.Send(new NotificationMessage("commandTransformer", TranformerCurrent, "TapChangerDown"));

                                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(TranformerCurrent.GID), Message = "Commanding transformer TapChanger down.", PointName = TranformerCurrent.Name };
                                ProxyServices.AlarmEventServiceProxy.AddEvent(e);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
