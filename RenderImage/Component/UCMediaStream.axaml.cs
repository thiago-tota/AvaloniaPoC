using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Component
{
    public class UCMediaStream : UserControl
    {
        public UCMediaStream()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
