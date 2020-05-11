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
                case "CurrentUp":
                    {
                        TranformerCurrent.Current++;
                        if (TranformerCurrent.Current <= TranformerCurrent.MaxCurrent)
                        {
                            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)TranformerCurrent.Current, SignalGid = TranformerCurrent.AnalogCurrentGID };
                            var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                            if (v == ScadaCommon.CommandResult.Success)
                            {
                                Messenger.Default.Send(new NotificationMessage("commandTransformer", TranformerCurrent, "CurrentUp"));

                                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(TranformerCurrent.GID), Message = "Commanding transformer current up.", PointName = TranformerCurrent.Name };
                                ProxyServices.AlarmEventServiceProxy.AddEvent(e);
                            }
                        }
                    }
                    break;
                case "CurrentDown":
                    {
                        TranformerCurrent.Current--;
                        if (TranformerCurrent.Current >= TranformerCurrent.MinCurrent)
                        {
                            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)TranformerCurrent.Current, SignalGid = TranformerCurrent.AnalogCurrentGID };
                            var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                            if (v == ScadaCommon.CommandResult.Success)
                            {
                                Messenger.Default.Send(new NotificationMessage("commandTransformer", TranformerCurrent, "CurrentDown"));

                                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(TranformerCurrent.GID), Message = "Commanding transformer current down.", PointName = TranformerCurrent.Name };
                                ProxyServices.AlarmEventServiceProxy.AddEvent(e);
                            }
                        }
                    }
                    break;
                case "VoltageUp":
                    {
                        TranformerCurrent.Voltage = TranformerCurrent.Voltage + 10;
                        if (TranformerCurrent.Voltage <= 10000)//TranformerCurrent.MaxVoltage
                        {
                            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)TranformerCurrent.Voltage, SignalGid = TranformerCurrent.AnalogVoltageGID};
                            var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                            if (v == ScadaCommon.CommandResult.Success)
                            {
                                Messenger.Default.Send(new NotificationMessage("commandTransformer", TranformerCurrent, "VoltageUp"));

                                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(TranformerCurrent.GID), Message = "Commanding transformer voltage up.", PointName = TranformerCurrent.Name };
                                ProxyServices.AlarmEventServiceProxy.AddEvent(e);
                            }
                        }
                    }
                    break;
                case "VoltageDown":
                    {
                        TranformerCurrent.Voltage = TranformerCurrent.Voltage - 10;
                        if (TranformerCurrent.Voltage >= TranformerCurrent.MinVoltage)
                        {
                            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)TranformerCurrent.Voltage, SignalGid = TranformerCurrent.AnalogVoltageGID };
                            var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                            if (v == ScadaCommon.CommandResult.Success)
                            {
                                Messenger.Default.Send(new NotificationMessage("commandTransformer", TranformerCurrent, "VoltageDown"));

                                Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(TranformerCurrent.GID), Message = "Commanding transformer voltage down.", PointName = TranformerCurrent.Name };
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
