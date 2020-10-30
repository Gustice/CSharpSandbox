using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TasksNThreads
{
    class MainViewModel : BindableBase
    {
        public ObservableCollection<string> LogItems { get; } = new ObservableCollection<string>();

        private DelegateCommand _shortSyncProcess;
        public DelegateCommand ShortSyncProcessCommand => _shortSyncProcess ??
            (_shortSyncProcess = new DelegateCommand(OnShortSyncProcess));

        private DelegateCommand _LongSyncProcess;
        public DelegateCommand LongSyncProcessCommand => _LongSyncProcess ??
            (_LongSyncProcess = new DelegateCommand(OnLongSyncProcess));


        private DelegateCommand _shortAsyncProcess;
        public DelegateCommand ShortAsyncProcessCommand => _shortAsyncProcess ??
            (_shortAsyncProcess = new DelegateCommand(OnShortAsyncProcess));

        private DelegateCommand _longAsyncProcess;
        public DelegateCommand LongAsyncProcessCommand => _longAsyncProcess ??
            (_longAsyncProcess = new DelegateCommand(OnLongAsyncProcess));

        private DelegateCommand _nestedProcessCommand;
        public DelegateCommand NestedProcessCommand => _nestedProcessCommand ??
            (_nestedProcessCommand = new DelegateCommand(OnNestedProcess));


        private string _lastActionLog;
        private Dispatcher _dispatcher;

        public string LastActionLog
        {
            get { return _lastActionLog; }
            set { SetProperty(ref _lastActionLog, value); }
        }

        
        public MainViewModel()
        {
            LogItems.Add("Window loaded triggered");

            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        void OnShortSyncProcess()
        {
            LogItems.Add("Short SyncProcess triggered");
            Thread.Sleep(200);
            // Task.Delay just gets overrun
            LogItems.Add("Short SyncProcess finished");
        }

        void OnLongSyncProcess()
        {
            LogItems.Add("Long SyncProcess triggered");
            Thread.Sleep(5000);
            // Task.Delay just gets overrun
            LogItems.Add("Long SyncProcess finished");
        }

        async void OnShortAsyncProcess()
        {
            LogItems.Add("Short AsyncProcess triggered");
            await Task.Delay(200);
            LogItems.Add("Short AsyncProcess finished");
        }

        async void OnLongAsyncProcess()
        {
            LogItems.Add("Long AsyncProcess triggered");
            await Task.Delay(5000);
            LogItems.Add("Long AsyncProcess finished");
        }

        int ProcessNumber = 0;
        private void OnNestedProcess()
        {
            var num = ProcessNumber++; 
            LogItems.Add("Nested Process triggered");

           Task.Run(() =>
           {
               Thread.Sleep(5000);
               _dispatcher.Invoke(() => LogItems.Add("Process in NestedProcess finished"));
           });

            LogItems.Add("Nested Process finished");
        }

    }
}
