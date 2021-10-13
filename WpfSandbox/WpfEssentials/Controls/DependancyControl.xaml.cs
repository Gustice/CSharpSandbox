using System.Windows;
using System.Windows.Controls;

namespace WpfEssentials.Controls
{
    /// <summary>
    /// Interaction logic for DependancyControl.xaml
    /// </summary>
    public partial class DependancyControl : UserControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(DependancyControl), new PropertyMetadata(""));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(DependancyControl), new PropertyMetadata(""));


        public DependancyControl()
        {
            InitializeComponent();

            Container.DataContext = this;
        }
    }
}
