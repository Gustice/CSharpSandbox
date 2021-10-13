using Prism.Mvvm;
using System.Collections.ObjectModel;
using WpfEssentials.Models;

namespace WpfEssentials.ViewModels
{
    public class SubControlsViewModel : BindableBase
    {
        public ObservableCollection<DemoDto> Items { get; } = new ObservableCollection<DemoDto>();

        private DemoDto _selected;
        public DemoDto Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public SubControlsViewModel()
        {
            Items.Add(new DemoDto() { Text = "Item 1", Number = 1 });
            Items.Add(new DemoDto() { Text = "Item 2", Number = 2 });
            Items.Add(new DemoDto() { Text = "Item 3", Number = 3 });
            Items.Add(new DemoDto() { Text = "Item 4", Number = 4 });
        }


    }
}
