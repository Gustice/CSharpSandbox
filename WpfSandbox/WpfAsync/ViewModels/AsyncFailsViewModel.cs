using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAsync.ViewModels
{
    public class AsyncFailsViewModel : BindableBase
    {
        public DelegateCommand DeadLockCommand { get; }
        public DelegateCommand FreezeGuiCommand { get; }

        public AsyncFailsViewModel()
        {
            DeadLockCommand = new DelegateCommand(Command);
            FreezeGuiCommand = new DelegateCommand(OnFreezeGui);
        }

        private void Command()
        {
            DeadLockCommand.IsActive = false;

            // Schedules Task in UI-Thread (will be processed after this task)
            Work().GetAwaiter().GetResult();
            // Waiting for Task that is scheduled for later -> Deadlock

            DeadLockCommand.IsActive = false;
        }

        private void OnFreezeGui()
        {
            FreezeGuiCommand.IsActive = false;

            // Start Task in Background -> No Deadlock
            var task = Task.Run(() => Work().GetAwaiter().GetResult());
            task.Wait(); // Wait Synchronously -> Hence Freeze
            // Its like to start Thread.Sleep(3_000);

            FreezeGuiCommand.IsActive = false;
        }

        private async Task Work()
        {
            await Task.Delay(3_000);
        }
    }
}
