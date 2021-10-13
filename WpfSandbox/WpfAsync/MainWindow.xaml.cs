using System.Windows;
using WpfAsync.ViewModels;

namespace WpfAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AsyncControlViewModel AsyncView { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            AsyncView = new AsyncControlViewModel();
        }
    }
}
