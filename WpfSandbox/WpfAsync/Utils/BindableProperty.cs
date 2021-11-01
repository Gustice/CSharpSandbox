using System.Collections.Generic;

namespace WpfAsync.Utils
{
    /// <summary>
    /// Generic bindable property class, with integrated change notification. 
    /// The Property called "Property" is supposed to be binded. 
    /// </summary>
    public class BindableProperty<T> : Bindable
    {
        private T _backingField;

        /// <summary>
        /// Property to be used as binding source
        /// </summary>
        public T Property
        {
            get { return _backingField; }
            set
            {
                // Notify only on actual change
                if (EqualityComparer<T>.Default.Equals(value, _backingField))
                    return;

                _backingField = value;
                OnPropertyChanged();
            }
        }
    }
}
