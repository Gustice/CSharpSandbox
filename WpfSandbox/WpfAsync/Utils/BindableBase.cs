using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfAsync.Utils
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoking PropertyChanged event handler.
        /// Property-changed-handler can be implemented as shown below with attribute `[CallerMemberName]` to avoid naming the property explicitly.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}