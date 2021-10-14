using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfAsync.Utils
{
    /// <summary>
    /// ICommand implementation targeting asynchronous applications
    /// </summary>
    public class AsyncCommand : Bindable, ICommand
    {
        private readonly Func<CancellationToken, Task> _taskToInvoke;
        private readonly Func<string, Task> _onNotifyCallback;
        private bool _isBusy;
        private Exception _lastError;
        private CancellationTokenSource _cts;

        public ICommand CancelCommand { get; }

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                if (value == _isBusy) return;
                _isBusy = value;
                OnPropertyChanged();

                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Exception LastError
        {
            get => _lastError;
            private set
            {
                if (Equals(value, _lastError)) return;
                _lastError = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructor with command target and notification callback
        /// </summary>
        /// <param name="taskToInvoke">Task to be invoked on command</param>
        /// <param name="onNotifyCallback">Callback to propagate status changes or error messages</param>
        public AsyncCommand(Func<CancellationToken, Task> taskToInvoke, Func<string, Task> onNotifyCallback = null)
        {
            _taskToInvoke = taskToInvoke;
            _onNotifyCallback = onNotifyCallback;
            CancelCommand = new AsyncCommand(() => Cancel());
        }

        /// <summary>
        /// Private constructor to generate nested commands
        /// </summary>
        private AsyncCommand(Func<Task> taskToInvoke)
        {
            _taskToInvoke = ct => taskToInvoke();
        }

        private Task Cancel()
        {
            _cts?.Cancel();
            return Task.CompletedTask;
        }


        #region ICommand
        public bool CanExecute(object parameter)
        {
            return !IsBusy;
        }

        /// <summary>
        /// Execution in asynchronous context. Not intended to synchronize after finishing.
        /// </summary>
        public async void Execute(object parameter)
        {
            IsBusy = true;
            try
            {
                _cts = new CancellationTokenSource();
                LastError = null;
                Task task = _taskToInvoke(_cts.Token);
                await task;
            }
            catch (OperationCanceledException)
            {
                _onNotifyCallback?.Invoke("Aborted");
            }
            catch (Exception ex)
            {
                LastError = ex;
                _onNotifyCallback?.Invoke(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event EventHandler CanExecuteChanged;
        #endregion
    }
}
