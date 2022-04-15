using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RenderImage.Avalonia.UCMediaStream
{
    public class MediaStreamView : UserControl
    {
        public MediaStreamView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
