using System.Collections.ObjectModel;
using System.Windows;

namespace TasksNThreads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        ObservableCollection<string> LogItems = new ObservableCollection<string>();
        
        public MainWindow()
        {
            InitializeComponent();

            MainView.DataContext = new MainViewModel();
        }
        
    }
}
