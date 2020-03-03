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
            else if (string.Compare(window, "Breaker") == 0)
            {
                commandBreakerViewModel = new CommandBreakerViewModel(SubstationCurrent.Breaker);
                CurrentCommandViewModel = commandBreakerViewModel;
            }
            else if (string.Compare(window, "PowerTransformer") == 0)
            {

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
