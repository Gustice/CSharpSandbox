using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WpfAsync.Models;
using WpfAsync.Utils;

namespace WpfAsync.ViewModels
{
    public class AsyncEssentialsViewModel : BindableBase
    {
        public DelegateCommand WorkAroundHelperCommand { get; }
        public DelegateCommand AsyncWorkAroundCommand { get; }
        public DelegateCommand AsyncHackCommand { get; }
        public AsyncCommand AsyncProcessingCommand { get; }
        public AsyncCommand ConstructCommand { get; }

        public AsyncEssentialsViewModel()
        {
            WorkAroundHelperCommand = new DelegateCommand(OnWorkAoundHelper);
            AsyncWorkAroundCommand = new DelegateCommand(OnAsyncWorkAound);
            AsyncHackCommand = new DelegateCommand(OnAsyncHack);
            AsyncProcessingCommand = new AsyncCommand(OnAsyncProcessing, null);
            ConstructCommand = new AsyncCommand(OnConstructAsync, null);

            AsyncHackCommand.IsActive = true;
            WorkAroundHelperCommand.IsActive = true;
            AsyncWorkAroundCommand.IsActive = true;
        }

        /// <summary>
        /// Demonstration of async-Hack: Method has async-void-Signature. This works but has serious disadvantages
        /// </summary>
        private async void OnAsyncHack() // Is asynchronous but return immediately void 
        { // Calling function has no possibility to know whether task has actually finished
            AsyncHackCommand.IsActive = false;
            AsyncHackCommand.RaiseCanExecuteChanged();

            await Work();

            AsyncHackCommand.IsActive = true;
            AsyncHackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Demonstration of encapsulated hack in helper method.
        /// </summary>
        private void OnWorkAoundHelper()
        { // Mimics typical synchronous behavior by using TaskWorkAround-Class
            WorkAroundHelperCommand.IsActive = false;
            WorkAroundHelperCommand.RaiseCanExecuteChanged();

            // Synchronous processing leads to freezing GUI
            TaskWorkaround.Execute(() => Work()); // Note: Current Implementation leads to freezes
            // Variant with return type
            var result = TaskWorkaround.Execute<int>(() => Process());
            // NOTE: As additional benefit is that you can keep track of all your async "sins" in your code base (just lookup where the TaskWorkaround is used)

            WorkAroundHelperCommand.IsActive = true;
            WorkAroundHelperCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Demonstration of synchronizing of asynchronous processes.
        /// </summary>
        private void OnAsyncWorkAound() // Plain Workaround without Helper
        { // Mimics typical synchronous behavior by using TaskWorkAround-Class
            AsyncWorkAroundCommand.IsActive = false;
            AsyncWorkAroundCommand.RaiseCanExecuteChanged();

            // Create and start Task that starts given job and awaits synchronously 
            Task.Run(() => // Task is run on thread-pool hence no deadlock condition
            {
                Process().GetAwaiter().GetResult();
                AsyncWorkAroundCommand.IsActive = true;
                AsyncWorkAroundCommand.RaiseCanExecuteChanged();
            });
            // NOTE: task.Start(specificTaskSceduler); Can specify other scheduler
        }

        private async Task Work()
        {
            await Task.Delay(3_000);
        }

        private async Task<int> Process()
        {
            await Task.Delay(2_000);
            return 100;
        }

        /// <summary>
        /// Demonstration of running two concurrent tasks
        /// The Idea is that the processing should not take the sum of the required time of each task.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task OnAsyncProcessing(CancellationToken arg)
        {
            var sw = Stopwatch.StartNew();
            var t1 = WriteToDb();
            var t2 = WriteToLog();
            // Both Task will be run in the background

            // Await only evaluates their state and waits for completion if not finished
            await t1;
            await t2;
            // Alternative (if no return value needed)
            // await Task.WhenAll(WriteToDb(), WriteToLog());

            Debug.WriteLine($"## Time for processing both Tasks: {sw.ElapsedMilliseconds} ms");
        }

        private static async Task WriteToDb()
        {
            // Better to await on spot in order to locate the source of Exception messages easier
            await Task.Delay(4_000);
        }
        private static async Task WriteToLog()
        {
            // Better to await on spot in order to locate the source of Exception messages easier
            await Task.Delay(3_000);
        }


        /// <summary>
        /// Demonstration of asynchronous construction of an instance with factory-method
        /// </summary>
        private async Task OnConstructAsync(CancellationToken arg)
        {
            // By usage of factory
            var obj = await Task.Run(() => AsyncConstructable.Construct());
            Debug.WriteLine($"Construction of object finished. Value was set to {obj.SetValue}");
        }


        // Other Examples
        public async Task ManualAwaitAllTasks() {
            List<Task> pendingTasks = new List<Task>() {
                WriteToDb(),
                WriteToLog(),
            };

            while (pendingTasks.Any()) {
                var finishedTask = await Task.WhenAny(pendingTasks);
                pendingTasks.Remove(finishedTask);
            }
        }
    }
}
