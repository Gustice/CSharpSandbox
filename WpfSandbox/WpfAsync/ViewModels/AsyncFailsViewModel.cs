using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAsync.ViewModels
{
    public class AsyncFailsViewModel : BindableBase
    {
        public DelegateCommand DeadLockCommand { get; }
        public DelegateCommand FreezeGuiCommand { get; }
        public DelegateCommand WrongOrderCommand { get; }

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
            WrongOrderCommand = new DelegateCommand(OnProcessInWrongOrder);
        }

        private void OnDeadLockCommand()
        {
            DeadLockCommand.IsActive = false;

            // Schedules Task in UI-Thread (will be processed after this task)
            Work().GetAwaiter().GetResult(); // Would throw a convenient exception
            // Work().Wait(); // <= Would throw **Aggregate Exception**
            // Waiting for Task that is scheduled for later -> Deadlock

            // This would not cause deadlock. Task is started on different thread
            //var task = Task.Run(() => Work().GetAwaiter().GetResult());
            //task.Wait();

            DeadLockCommand.IsActive = false;
        }

        private async Task Work()
        {
            // Wow there is much to do here ...
            await Task.Delay(3_000);

            // Avoid to capture Context would prevent that this process is run on same thread
            // await Task.Delay(3_000).ConfigureAwait(false);

            // Call could also be forced in different context by 
            //var task = Task.Run(() => Work().GetAwaiter().GetResult());
            //task.Wait();
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



        private void OnProcessInWrongOrder()
        {
            OutputOrderText = "";
            OutputOrderText += ">Start Head Process\n";
            SubProcess();
            OutputOrderText += "<Head Process Finished\n";
        }

        private async void SubProcess() // Note the calling process has no handle to check if call is finished
        {
            OutputOrderText += "  >Start Sub-Process \n";
            Task sleppTask = Task.Run(() => Thread.Sleep(4000));
            OutputOrderText += "  >Start Await \n";
            await sleppTask; // First await lets this process to appear finished
            OutputOrderText += "  <Await finished\n";
            OutputOrderText += "  <Sub-Process Finished\n";
        }
    }
}


// Other Deadlocks
////// UI
///
//// My "library" method.
//public static async Task<JObject> GetJsonAsync(Uri uri)
//{
//    // (real-world code shouldn't use HttpClient in a using block; this is just example code)
//    using (var client = new HttpClient())
//    {
//        var jsonString = await client.GetStringAsync(uri);
//        return JObject.Parse(jsonString);
//    }
//}

//// My "top-level" method.
//public void Button1_Click(...)
//{
//    var jsonTask = GetJsonAsync(...);
//    textBox1.Text = jsonTask.Result;
//}
//// My "top-level" method.
//public class MyController : ApiController
//{
//    public string Get()
//    {
//        var jsonTask = GetJsonAsync(...);
//        return jsonTask.Result.ToString();
//    }
//}