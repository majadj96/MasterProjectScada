using GalaSoft.MvvmLight.Messaging;
using System;
using UserInterface.BaseError;
using UserInterface.Command;
using UserInterface.Subscription;
using UserInterface.ViewModel;

namespace UserInterface
{
    public class CommandingWindowViewModel : BindableBase
    {
        #region Variables
        private CommandDisconnectorViewModel commandDisconnectorViewModel = new CommandDisconnectorViewModel();
        private CommandBreakerViewModel commandBreakerViewModel = new CommandBreakerViewModel();

        private BindableBase currentCommandViewModel;
        #endregion

        #region Props
        public BindableBase CurrentCommandViewModel
        {
            get { return currentCommandViewModel; }
            set { SetProperty(ref currentCommandViewModel, value); }
        }
        #endregion

        public CommandingWindowViewModel()
        {
        }

        public void SetView(string window)
        {
            if (string.Compare(window, "Disconnector1") == 0)
            {
                CurrentCommandViewModel = commandDisconnectorViewModel;
                commandDisconnectorViewModel.SetName("Disconnector1");
            }
            else if (string.Compare(window, "Disconnector2") == 0)
            {
                CurrentCommandViewModel = commandDisconnectorViewModel;
                commandDisconnectorViewModel.SetName("Disconnector2");
            }
            else if (string.Compare(window, "Breaker") == 0)
            {
                CurrentCommandViewModel = commandBreakerViewModel;
                commandBreakerViewModel.SetName("Breaker");
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
