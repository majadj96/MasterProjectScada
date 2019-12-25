using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UserInterface.Command
{
    class DisconectorCommand : ICommand
    {
        private MainViewModel mainViewModel;
        public DisconectorCommand(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
            //   return mainWindowViewModel.CanLogIn;
        }

        public void Execute(object parameter)
        {
            mainViewModel.DisconectorOperation();
        }
    }
}
