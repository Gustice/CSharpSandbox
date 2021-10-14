using WpfAsync.Utils;

namespace WpfAsync.Models
{
    public class TextProperty : Bindable
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if (value == _text) 
                    return;
                
                _text = value;
                OnPropertyChanged();
            }
        }

        public TextProperty(string init)
        {
            Text = init;
        }
    }
}
