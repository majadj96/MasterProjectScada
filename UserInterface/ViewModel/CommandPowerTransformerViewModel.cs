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
using System.Collections.Generic;

namespace UserInterface.ViewModel
{
    public class CommandPowerTransformerViewModel : BindableBase
    {
        public MyICommand<string> CurrentVoltageCommand { get; private set; }

        #region Variables
        private Transformator transformator;
        private TapChanger TapChanger;
        private Dictionary<long, Measurement> Measurements;
        #endregion

        #region Props
        public Transformator TranformerCurrent
        {
            get { return transformator; }
            set { transformator = value; OnPropertyChanged("TranformerCurrent"); }
        }
        #endregion

        public CommandPowerTransformerViewModel(Transformator transformator, TapChanger tapChanger, Dictionary<long, Measurement> measurements)
        {
            TranformerCurrent = transformator;
            TapChanger = tapChanger;
            Measurements = measurements;

            CurrentVoltageCommand = new MyICommand<string>(CommandCurrentVoltage);
        }

        public void CommandCurrentVoltage(string type)
        {
            Measurement tapChangerPosition = GetMeasurementForTapChanger();

            switch(type)
            {
                case "TapChangerUp":
                    {
                        if (tapChangerPosition.Value + 1 <= tapChangerPosition.Max)
                        {
                            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)tapChangerPosition.Value + 1, SignalGid = tapChangerPosition.Gid };
                            var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                        }
                        //TranformerCurrent.TapChangerValue++;
                        //if (TranformerCurrent.TapChangerValue <= TranformerCurrent.MaxValueTapChanger)
                        //{
                        //    CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)TranformerCurrent.TapChangerValue, SignalGid = TranformerCurrent.AnalogTapChangerGID };
                        //    var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                        //    if (v == ScadaCommon.CommandResult.Success)
                        //    {
                        //        Messenger.Default.Send(new NotificationMessage("commandTransformer", TranformerCurrent, "TapChangerUp"));

                        //        Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(TranformerCurrent.GID), Message = "Commanding transformer TapChanger up.", PointName = TranformerCurrent.Name };
                        //        ProxyServices.AlarmEventServiceProxy.AddEvent(e);
                        //    }
                        //}
                    }
                    break;
                case "TapChangerDown":
                    {
                        if (tapChangerPosition.Value - 1 >= tapChangerPosition.Min)
                        {
                            CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)tapChangerPosition.Value - 1, SignalGid = tapChangerPosition.Gid };
                            var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                        }
                        //TranformerCurrent.TapChangerValue--;
                        //if (TranformerCurrent.TapChangerValue >= TranformerCurrent.MinValueTapChanger)
                        //{
                        //    CommandObject commandObject = new CommandObject() { CommandingTime = DateTime.Now, CommandOwner = "UI", EguValue = (float)TranformerCurrent.TapChangerValue, SignalGid = TranformerCurrent.AnalogTapChangerGID };
                        //    var v = ProxyServices.CommandingServiceProxy.WriteAnalogOutput(commandObject);
                        //    if (v == ScadaCommon.CommandResult.Success)
                        //    {
                        //        Messenger.Default.Send(new NotificationMessage("commandTransformer", TranformerCurrent, "TapChangerDown"));

                        //        Event e = new Event() { EventReported = DateTime.Now, EventReportedBy = Common.AlarmEventType.UI, GiD = long.Parse(TranformerCurrent.GID), Message = "Commanding transformer TapChanger down.", PointName = TranformerCurrent.Name };
                        //        ProxyServices.AlarmEventServiceProxy.AddEvent(e);
                        //    }
                        //}
                    }
                    break;
                default:
                    break;
            }
        }

        private Measurement GetMeasurementForTapChanger()
        {
            foreach (Measurement meas in Measurements.Values)
            {
                if(meas.PowerSystemResource.ToString() == TapChanger.GID)
                {
                    return meas;
                }
            }

            return null;
        }
    }
}
