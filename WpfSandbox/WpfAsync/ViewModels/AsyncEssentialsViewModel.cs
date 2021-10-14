using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using WpfAsync.Utils;

namespace WpfAsync.ViewModels
{
    public class AsyncEssentialsViewModel : BindableBase
    {
        public DelegateCommand AsyncWorkAroundCommand { get; }
        public DelegateCommand AsyncHackCommand { get; }

        public AsyncEssentialsViewModel()
        {
            AsyncWorkAroundCommand = new DelegateCommand(OnAsyncWorkAound);
            AsyncHackCommand = new DelegateCommand(OnAsyncHack);

            AsyncHackCommand.IsActive = true;
            AsyncWorkAroundCommand.IsActive = true;
        }

        private void OnAsyncWorkAound()
        { // Mimics typical synchronous behavior by using TaskWorkAround-Class
            AsyncWorkAroundCommand.IsActive = false;
            AsyncWorkAroundCommand.RaiseCanExecuteChanged();

            // Synchronous processing leads to freezing GUI
            TaskWorkaround.Execute(() => Work());

            var result = TaskWorkaround.Execute<int>(() => Process());

            AsyncWorkAroundCommand.IsActive = true;
            AsyncWorkAroundCommand.RaiseCanExecuteChanged();
        }
        private async void OnAsyncHack() // Is asynchronous but return immediately void 
        { // Calling function has no possibility to know whether task has actually finished
            AsyncHackCommand.IsActive = false;
            AsyncHackCommand.RaiseCanExecuteChanged();

            await Work();

            AsyncHackCommand.IsActive = true;
            AsyncHackCommand.RaiseCanExecuteChanged();
        }

        private async Task Work()
        {
            await Task.Delay(3_000);
        }

        private async Task<int> Process()
        {
            await Task.Delay(1_000);
            return 100;
        }
    }
}
