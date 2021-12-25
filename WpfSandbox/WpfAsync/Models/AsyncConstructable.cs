using System.Threading.Tasks;
using WpfAsync.Utils;

namespace WpfAsync.Models
{
    class AsyncConstructable : Bindable
    {
        public BindableProperty<string> SomeText { get; } = new BindableProperty<string>();
        public BindableProperty<int> SomeNumber { get; } = new BindableProperty<int>();
        public int SetValue { get; private set; }

        private AsyncConstructable()
        {
            Initialize().GetAwaiter().GetResult();
        }

        private async Task Initialize()
        {
            await Task.Delay(1000);
            SetValue = 10;
        }

        public static async Task<AsyncConstructable> Construct() {
            return new AsyncConstructable();
        }
    }
}
