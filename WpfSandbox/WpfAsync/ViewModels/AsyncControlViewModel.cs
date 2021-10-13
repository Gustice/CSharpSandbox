using System;
using System.Threading;
using System.Threading.Tasks;
using WpfAsync.Models;
using WpfAsync.Utils;

namespace WpfAsync.ViewModels
{
    public class AsyncControlViewModel : BindableBase
    {
        /// <summary>
        /// Progress as bindable Property (already implements INotifyPropertyChanged)
        /// </summary>
        public BindableProperty<string> Progress { get; } = new BindableProperty<string>();
                
        /// <summary>
        /// Status as classic Property (property-changed callback is triggered explicitly)
        /// </summary>
        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Text-Property demonstrates lazy loading with reload-ability
        /// </summary>
        public AsyncProperty<TextProperty> LazyProperty { get; }

        /// <summary>
        /// Asynchronous command demonstration with integrated Cancel command.
        /// </summary>
        public AsyncCommand StartCommand { get; }


        public AsyncControlViewModel()
        {
            StartCommand = new AsyncCommand(OnStart, OnNotifyCallback);
            LazyProperty = new AsyncProperty<TextProperty>(LoadProperty);
        }

        private async Task<TextProperty> LoadProperty()
        {
            // Simulates some work
            await Task.Delay(3000);
            // After all the work is done
            return new TextProperty($"Lazy Load Completed {DateTime.Now}");
        }

        private async Task OnStart(CancellationToken cToken)
        {
            var counter = 50;
            Status = "Started";

            while (counter-- > 0)
            {
                cToken.ThrowIfCancellationRequested();
                // Updating progress output
                Progress.Property = $"Processing ... {counter}";
                // Simulates continuous work 
                await Task.Delay(100, cToken).ConfigureAwait(false);
            }

            Status = "Finished";
        }

        /// <summary>
        /// Callback method to 
        /// </summary>
        private Task OnNotifyCallback(string status)
        {
            Status = $"Background-Notification: '{status}'";
            return Task.CompletedTask;
        }
    }
}
