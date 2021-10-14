using System.Windows;
using WpfAsync.ViewModels;

namespace WpfAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AsyncControlViewModel AsyncControl { get; }
        public AsyncFailsViewModel AsyncFails { get; }
        public AsyncEssentialsViewModel AsyncEssentials { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            AsyncControl = new AsyncControlViewModel();
            AsyncFails = new AsyncFailsViewModel();
            AsyncEssentials = new AsyncEssentialsViewModel();
        }
    }
}
