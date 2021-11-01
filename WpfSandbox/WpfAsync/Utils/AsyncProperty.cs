using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAsync.Utils
{
    /// <summary>
    /// Property for implementing asynchronous update mechanisms 
    /// </summary>
    public class AsyncProperty<T> : Bindable
    {
        private readonly Func<Task<T>> _loadingFunc;
        private T _data;
        private bool _hasStartedLoadingData;

        /// <summary>
        /// Command to refresh property on demand
        /// </summary>
        public ICommand ReloadCommand { get; }

        /// <summary>
        /// Property
        /// </summary>
        public T Property
        {
            get
            {
                // Start loading on first get
                if (!_hasStartedLoadingData)
                {
                    _hasStartedLoadingData = true;
                    _ = Load();
                }
                return _data;
            }
            private set
            {
                // Notify only on actual change
                if (EqualityComparer<T>.Default.Equals(value, _data))
                    return;

                _data = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructor with delegate to Loading implementation
        /// </summary>
        /// <param name="loadingFunc">Loading implementation for requested type</param>
        public AsyncProperty(Func<Task<T>> loadingFunc)
        {
            this._loadingFunc = loadingFunc;
            ReloadCommand = new AsyncCommand(ct => Load());
        }


        private async Task Load()
        {
            var loadedData = await _loadingFunc();
            Property = loadedData;
        }
    }
}
