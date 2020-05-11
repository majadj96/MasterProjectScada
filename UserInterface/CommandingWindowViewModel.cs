using UserInterface.BaseError;
using UserInterface.Model;
using UserInterface.ViewModel;

namespace UserInterface
{
    public class CommandingWindowViewModel : BindableBase
    {
        #region Variables
        private CommandDisconnectorViewModel commandDisconnectorViewModel;
        private CommandBreakerViewModel commandBreakerViewModel;
        private CommandPowerTransformerViewModel commandPowerTransformerViewModel;

        private BindableBase currentCommandViewModel;
        private Substation substationCurrent;
        #endregion

        #region Props
        public BindableBase CurrentCommandViewModel
        {
            get { return currentCommandViewModel; }
            set { SetProperty(ref currentCommandViewModel, value); }
        }
        public Substation SubstationCurrent
        {
            get { return substationCurrent; }
            set { substationCurrent = value; OnPropertyChanged("SubstationCurrent"); }
        }
        #endregion

        public CommandingWindowViewModel(Substation substation)
        {
            SubstationCurrent = substation;
        }

        public void SetView(string window)
        {
            if (SubstationCurrent == null || SubstationCurrent.Gid == null)
                return;

            if (string.Compare(window, "Disconnector1") == 0)
            {
                commandDisconnectorViewModel = new CommandDisconnectorViewModel(SubstationCurrent.Disconectors[0], "1");
                CurrentCommandViewModel = commandDisconnectorViewModel;
            }
            else if (string.Compare(window, "Disconnector2") == 0)
            {
                if (SubstationCurrent.Disconectors.Count > 1)
                {
                    commandDisconnectorViewModel = new CommandDisconnectorViewModel(SubstationCurrent.Disconectors[1], "2");
                    CurrentCommandViewModel = commandDisconnectorViewModel;
                }
            }
            else if (string.Compare(window, "Breaker1") == 0)
            {
                commandBreakerViewModel = new CommandBreakerViewModel(SubstationCurrent.Breakers[0], "1");
                CurrentCommandViewModel = commandBreakerViewModel;
            }
            else if (string.Compare(window, "Breaker2") == 0)
            {
                commandBreakerViewModel = new CommandBreakerViewModel(SubstationCurrent.Breakers[1], "2");
                CurrentCommandViewModel = commandBreakerViewModel;
            }
            else if (string.Compare(window, "Breaker3") == 0)
            {
                commandBreakerViewModel = new CommandBreakerViewModel(SubstationCurrent.Breakers[0], "3");
                CurrentCommandViewModel = commandBreakerViewModel;
            }
            else if (string.Compare(window, "Breaker4") == 0)
            {
                if (SubstationCurrent.Breakers.Count > 2)
                {
                    commandBreakerViewModel = new CommandBreakerViewModel(SubstationCurrent.Breakers[1], "4");
                    CurrentCommandViewModel = commandBreakerViewModel;
                }
            }
            else if (string.Compare(window, "Breaker5") == 0)
            {
                if (SubstationCurrent.Breakers.Count > 2)
                {
                    commandBreakerViewModel = new CommandBreakerViewModel(SubstationCurrent.Breakers[2], "5");
                    CurrentCommandViewModel = commandBreakerViewModel;
                }
            }
            else if (string.Compare(window, "PowerTransformer") == 0)
            {
                commandPowerTransformerViewModel = new CommandPowerTransformerViewModel(SubstationCurrent.Transformator);
                CurrentCommandViewModel = commandPowerTransformerViewModel;
            }
            else if (string.Compare(window, "AsynchronousMachine1") == 0)
            {

            }
            else if (string.Compare(window, "AsynchronousMachine2") == 0)
            {

            }
        }
    }
}
