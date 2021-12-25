using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAsync.Views
{
    /// <summary>
    /// Interaction logic for AsyncFailsView.xaml
    /// </summary>
    public partial class AsyncFailsView : UserControl
    {
        public AsyncFailsView()
        {
            InitializeComponent();
        }

     // NOTE: See View model for further notes
     private void DeadLock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            // This does not lead to deadlock because all work is done synchronously in current thread.
            SyncWork().GetAwaiter().GetResult();

            // Schedules Task in UI-Thread (will be processed after this task)
            WorkAsync().GetAwaiter().GetResult();
            // Waiting for Task that is scheduled for later -> Deadlock

            btn.IsEnabled = true;
        }

        // NOTE: See View model for further notes
        private void FreezeGui_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            // Start Task in Background -> No Deadlock
            var task = Task.Run(() => WorkAsync());
            task.Wait(); // Wait Synchronously -> Hence Freeze
            // Its like to start Thread.Sleep(3_000);

            btn.IsEnabled = true;
        }

        private async Task WorkAsync()
        {
            await /* <-> */ Task.Delay(3_000);
            // Potential problem if called from GUI-Thread.
        }

        private Task SyncWork()
        {
            Thread.Sleep(500);
            // No potential Problems because Work is done in a synchronous manner
            return Task.CompletedTask;
        }
    }
}
