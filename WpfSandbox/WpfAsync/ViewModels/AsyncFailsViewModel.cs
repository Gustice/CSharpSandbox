using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAsync.ViewModels
{
    public class AsyncFailsViewModel : BindableBase
    {
        public DelegateCommand DeadLockCommand { get; }
        public DelegateCommand FreezeGuiCommand { get; }
        public DelegateCommand NotAwaitedCommand { get; }
        public DelegateCommand NotAwaitableCommand { get; }
        public DelegateCommand InvokeThrowingCommand { get; }
        public DelegateCommand InvokeThrowingUncatchableCommand { get; }

        private string _outputOrderText;
        public string OutputOrderText
        {
            get { return _outputOrderText; }
            set { SetProperty(ref _outputOrderText, value); }
        }

        public AsyncFailsViewModel()
        {
            DeadLockCommand = new DelegateCommand(OnDeadLockCommand);
            FreezeGuiCommand = new DelegateCommand(OnFreezeGui);
            NotAwaitedCommand = new DelegateCommand(OnNotAwaited);
            NotAwaitableCommand = new DelegateCommand(OnNotAwaitable);
            InvokeThrowingCommand = new DelegateCommand(OnInvokeThrowing);
            InvokeThrowingUncatchableCommand = new DelegateCommand(OnInvokeThrowingUncatchable);
        }

        /// <summary>
        /// Demonstrating Deadlock due piped tasks on waiting Thread (GUI-Thread)
        /// </summary>
        private void OnDeadLockCommand()
        {
            DeadLockCommand.IsActive = false;

            // This does not lead to deadlock because all work is done synchronously in current thread.
            SyncWork().GetAwaiter().GetResult(); // No Problem here

            // Schedules Task in UI-Thread (will be processed after this task)
            WorkAsync().GetAwaiter().GetResult(); // Would throw a convenient exception
            // Work().Wait(); // <= Would throw **Aggregate Exception**
            // Waiting for Task that is scheduled for later -> Deadlock

            // This would not cause deadlock. Task is started on different thread
            //var task = Task.Run(() => Work().GetAwaiter().GetResult());
            //task.Wait();

            DeadLockCommand.IsActive = false;
        }

        private async Task WorkAsync()
        {
            // Wow there is much to do here ...
            await Task.Delay(3_000);
            // Potential problem if called from GUI-Thread.

            // Avoid to capture Context would prevent that this process is run on same thread
            // await Task.Delay(3_000).ConfigureAwait(false);

            // Call could also be forced in different context by 
            //var task = Task.Run(() => Work().GetAwaiter().GetResult());
            //task.Wait();
        }

        private Task SyncWork()
        {
            Thread.Sleep(500);
            // No potential Problems because Work is done in a synchronous manner
            return Task.CompletedTask;
        }

        /// <summary>
        /// Demonstrating Freeze of GUI due to synchronous waiting on finished tasks (at least no deadlock)
        /// </summary>
        private void OnFreezeGui()
        {
            FreezeGuiCommand.IsActive = false;

            // Start Task in Background -> No Deadlock
            var task = Task.Run(() => WorkAsync());
            task.Wait(); // Wait Synchronously -> Hence Freeze
            // Its like to start Thread.Sleep(3_000);

            FreezeGuiCommand.IsActive = false;
        }


        /// <summary>
        /// Demonstrating of unexpected output. Head-Process finishes before called method. 
        /// </summary>
        private void OnNotAwaited()
        {
            OutputOrderText = "";
            OutputOrderText += ">Start Head Process\n";
            // The subprocess leaves before the method is actually finished (i.e. on first await)
            SubProcess();
            // SubProcess return Task hence could have been awaited like:
            //    Task.Run(async () => await SubProcess()).GetAwaiter().GetResult(); // This leads to correct output
            OutputOrderText += "<Head Process Finished\n";
        }

        private async Task SubProcess()
        {
            OutputOrderText += "  >Start Sub-Process \n";
            Task sleppTask = Task.Run(() => Thread.Sleep(1000));
            OutputOrderText += "  >Start Await \n";
            await /* Method returns her */ sleppTask; // First await lets this process to appear finished
            // Rest is processed in a subsequent task
            OutputOrderText += "  <Await finished\n";
            OutputOrderText += "  <Sub-Process Finished\n";
        }


        /// <summary>
        /// Demonstrating of unexpected output. Head-Process finishes before called method.
        /// Other than above this cannot be fixed without changing the signature of the called method
        /// </summary>
        private void OnNotAwaitable()
        {
            OutputOrderText = "";
            OutputOrderText += ">Start Head Process\n";
            // The subprocess leaves before the method is actually finished (i.e. on first await)
            SubProcessVoid();
            // Workaround leads to error because called method does not return Task
            //    Task.Run(async () => await SubProcessVoid()).GetAwaiter().GetResult(); // This leads to correct output
            OutputOrderText += "<Head Process Finished\n";
        }

        private async void SubProcessVoid() // Note the calling process has no handle to check if call is finished
        {
            OutputOrderText += "  >Start Sub-Process \n";
            Task sleppTask = Task.Run(() => Thread.Sleep(1000));
            OutputOrderText += "  >Start Await \n";
            await /* Method returns her */ sleppTask; // First await lets this process to appear finished
            // Rest is processed in a subsequent task
            OutputOrderText += "  <Await finished\n";
            OutputOrderText += "  <Sub-Process Finished\n";
        }

        /// <summary>
        /// Demonstrating uncatched Exceptions that appear on awaiting an faulted process
        /// </summary>
        private void OnInvokeThrowing()
        {
            // Thrown Exception is not noticed because process is not awaited
            ThisIsGoingToBlowUpCatchMePlease();

            //try
            //{
                Task.Run(() => ThisIsGoingToBlowUpCatchMePlease()).GetAwaiter().GetResult();
            //}
            //catch (Exception)
            //{
            //    Debug.WriteLine("This catch does help, calling function is async Task. Hence Exception can be catched");
            //}
        }

        private async Task ThisIsGoingToBlowUpCatchMePlease()
        { // Exceptions can be catched because return Signature is Task
            Thread.Sleep(1_000); 
            throw new Exception("This Exception should be caught");
        }


        /// <summary>
        /// Demonstrating uncatchable Exceptions due to async void Signature
        /// </summary>
        private void OnInvokeThrowingUncatchable()
        {
            try
            {
                ThisIsGoingToBlowUpUncatchable();
            }
            catch (Exception)
            {
                Debug.WriteLine("This catch does not help, calling function is async void (would have worked if no async was involved)");
            }
        }

        private async void ThisIsGoingToBlowUpUncatchable()
        {
            try
            {
                Thread.Sleep(1_000);
                throw new Exception("This Exception should be caught");
            }
            catch (Exception)
            {
                Debug.WriteLine("This catch does actually help");
            }

            Thread.Sleep(1_000);
            // cannot be catched due to async void Signature
            throw new NotImplementedException("This Exception cannot be caught (even with catch scope on calling layer)");
            // Needs own try-catch-Block
        }
    }
}

