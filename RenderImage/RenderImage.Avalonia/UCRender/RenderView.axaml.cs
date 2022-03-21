using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RenderImage.Avalonia.UCRender
{
    public class RenderView : UserControl
    {
        public RenderView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
