using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEssentials.Models
{
    public class DemoDto : BindableBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private int _number;
        public int Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }
    }
}
