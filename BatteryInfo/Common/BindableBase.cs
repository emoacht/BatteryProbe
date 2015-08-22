using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BatteryInfo.Common
{
	public abstract class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}