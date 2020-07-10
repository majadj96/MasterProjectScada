using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace UserInterface.Dialog
{
    /// <summary>
    /// Interaction logic for DialogViewModel
    /// </summary>
    public partial class DialogViewModel : ViewModelBase
    {
        private DialogMessage _currentMessage;
        private bool _isVisible;
        private object _lock = new object();

        public DialogViewModel()
        {
            MessengerInstance.Register<DialogMessage>(this, (x) =>
            {
                lock (_lock)
                {
                    if (IsVisible)
                    {
                        return;
                    }

                    Message = x;
                    IsVisible = true;
                    RaisePropertyChanged(nameof(Header));
                }
            });
        }

        public DialogMessage Message
        {
            get => _currentMessage;
            set
            {
                Set(ref _currentMessage, value);
            }
        }

        public string Header => Message?.Type == DialogMessageType.Error ?
            "Error" :
            "Info";

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                Set(ref _isVisible, value);
            }
        }

        public RelayCommand CloseCommand => new RelayCommand(() => IsVisible = false);
    }

    public class DialogMessage
    {
        public string Message { get; }
        
        public DialogMessageType Type { get; }
        public DialogMessage(string message, DialogMessageType type)
        {
            Message = message;
            Type = type;
        }
    }

    public enum DialogMessageType
    {
        Error,
        Info
    }
}
