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

        private void DeadLock_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            // Schedules Task in UI-Thread (will be processed after this task)
            Work().GetAwaiter().GetResult();
            // Waiting for Task that is scheduled for later -> Deadlock

            btn.IsEnabled = true;
        }

        private void FreezeGui_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            // Start Task in Background -> No Deadlock
            var task = Task.Run(() => Work().GetAwaiter().GetResult());
            task.Wait(); // Wait Synchronously -> Hence Freeze
            // Its like to start Thread.Sleep(3_000);

            btn.IsEnabled = true;
        }

        private async Task Work()
        {
            await Task.Delay(3_000);
        }
    }
}
