using Common;
using Common.AlarmEvent;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UserInterface.BaseError;
using UserInterface.Model;

namespace UserInterface.ViewModel
{
    public class TableViewModel : BindableBase
    {
        public BindingList<UIModel> substationItems = new BindingList<UIModel>();
        public BindingList<Measurement> analogs = new BindingList<Measurement>();
        public BindingList<Measurement> discretes = new BindingList<Measurement>();
        private List<Alarm> alarms;

        public BindingList<UIModel> SubstationItems
        {
            get
            {
                return substationItems;
            }
            set
            {
                substationItems = value;
                OnPropertyChanged("SubstationItems");
            }
        }

        public BindingList<Measurement> Analogs
        {
            get
            {
                return analogs;
            }
            set
            {
                analogs = value;
                OnPropertyChanged("Analogs");
            }
        }

        public BindingList<Measurement> Discretes
        {
            get
            {
                return discretes;
            }
            set
            {
                discretes = value;
                OnPropertyChanged("Discretes");
            }
        }

        public TableViewModel(ObservableCollection<UIModel> model, Dictionary<long, Measurement> measurements, List<Alarm> alarms)
        {
            //SubstationItems = new BindingList<UIModel>(model);

            this.alarms = alarms;

            Analogs = new BindingList<Measurement>();
            Discretes = new BindingList<Measurement>();

            foreach (var meas in measurements)
            {
                if ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(meas.Key) == DMSType.ANALOG)
                {
                    if(IsInAlarmState(meas.Key))
                    {
                        meas.Value.AlarmVisibility = "Visible";
                    }
                    else
                    {
                        meas.Value.AlarmVisibility = "Hidden";
                    }
                    Analogs.Add(meas.Value);
                }
                else if ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(meas.Key) == DMSType.DISCRETE)
                {
                    if (IsInAlarmState(meas.Key))
                    {
                        meas.Value.AlarmVisibility = "Visible";
                    }
                    else
                    {
                        meas.Value.AlarmVisibility = "Hidden";
                    }
                    Discretes.Add(meas.Value);
                }
            }

            Messenger.Default.Register<Measurement>(this, (meas) =>
            {
                //if (message.Notification == "UpdateTelemetry")
                //    UpdateTelemetry((string)message.Sender, message.Target);
                UpdateMeas(meas);
            });
        }

        private void UpdateMeas(Measurement meas)
        {
            if (IsInAlarmState(meas.Gid))
            {
                meas.AlarmVisibility = "Visible";
            }
            else
            {
                meas.AlarmVisibility = "Hidden";
            }

            if ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid) == DMSType.ANALOG)
            {
                for (int i = 0; i < Analogs.Count; i++)
                {
                    if(Analogs[i].Gid == meas.Gid)
                    {
                        Analogs.ResetItem(i);
                        return;
                    }
                }

                Analogs.Add(meas);
            }
            else if ((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(meas.Gid) == DMSType.DISCRETE)
            {
                for (int i = 0; i < Discretes.Count; i++)
                {
                    if (Discretes[i].Gid == meas.Gid)
                    {
                        Discretes.ResetItem(i);
                        return;
                    }
                }

                Discretes.Add(meas);
            }
        }

        private void UpdateTelemetry(string type, object target)
        {
            Breaker b = null;
            Disconector d = null;
            UIModel t = null;

            if (type.Contains("Breaker"))
            {
                b = (Breaker)target;
            }
            else if (type.Contains("Disconector"))
            {
                d = (Disconector)target;
            }
            else if (type.Contains("TapChanger"))
            {
                t = (UIModel)target;
            }

            int i = 0;
            foreach (UIModel model in SubstationItems)
            {
                if (b != null)
                {
                    if (model.GID == b.GID)
                    {
                        model.Value = b.State.ToString();
                        model.Time = DateTime.Now.ToString();
                        SubstationItems.RaiseListChangedEvents = true;
                        SubstationItems.ResetItem(i);
                        break;
                    }
                }
                if (d != null)
                {
                    if (model.GID == d.GID)
                    {
                        model.Value = d.State.ToString();
                        model.Time = DateTime.Now.ToString();
                        SubstationItems.RaiseListChangedEvents = true;
                        SubstationItems.ResetItem(i);
                        break;
                    }
                }
                if (t != null)
                {
                    if (model.GID == t.GID)
                    {
                        model.Value = t.Value.ToString();
                        model.Time = DateTime.Now.ToString();
                        SubstationItems.RaiseListChangedEvents = true;
                        SubstationItems.ResetItem(i);
                        break;
                    }
                }
                i++;
            }
        }

        private bool IsInAlarmState(long measGid)
        {
            for (int i = 0; i < alarms.Count; i++)
            {
                if (alarms[i].GiD == measGid)
                    return true;
            }

            return false;
        }
    }
}
