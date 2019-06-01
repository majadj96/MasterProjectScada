using System.ComponentModel;

namespace FrontEndProcessorService.ViewModel
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		internal void OnPropertyChanged(string prop)
		{
			if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}