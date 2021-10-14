using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfAsync.Utils;

namespace WpfAsync.Models
{
    class AsyncConstructable : Bindable
    {
        public BindableProperty<string> SomeText { get; } = new BindableProperty<string>();
        public BindableProperty<int> SomeNumber { get; } = new BindableProperty<int>();

        private AsyncConstructable()
        {
            Initialize().GetAwaiter().GetResult();
        }

        private async Task Initialize()
        {
            Thread.Sleep(1_000); // Simulate some serous work
        }

        public static async Task<AsyncConstructable> Construct() {
            return new AsyncConstructable();
        }
    }
}
