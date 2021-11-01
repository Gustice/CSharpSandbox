using System.Windows;
using WpfEssentials.ViewModels;

namespace WpfEssentials
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SubControlsViewModel SubControls { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            SubControls = new SubControlsViewModel();
        }
    }
}
