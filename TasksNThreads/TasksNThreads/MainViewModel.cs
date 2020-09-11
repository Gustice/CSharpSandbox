using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

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


        private string _lastActionLog;
        public string LastActionLog
        {
            get { return _lastActionLog; }
            set { SetProperty(ref _lastActionLog, value); }
        }

        public MainViewModel()
        {
            LogItems.Add("Window loaded triggered");
        }

        void OnShortSyncProcess()
        {
            LogItems.Add("OnShortSyncProcess triggered");
        }

        void OnLongSyncProcess()
        {
            LogItems.Add("OnLongSyncProcess triggered");
        }

        void OnShortAsyncProcess()
        {
            LogItems.Add("OnShortAsyncProcess triggered");
        }

        void OnLongAsyncProcess()
        {
            LogItems.Add("OnLongAsyncProcess triggered");
        }


    }
}
