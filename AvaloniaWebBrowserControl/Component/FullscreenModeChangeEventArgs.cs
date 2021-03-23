using Avalonia.Interactivity;

namespace AvaloniaWebBrowserControl.Component
{
    public class FullscreenModeChangeEventArgs : RoutedEventArgs
    {
        public FullscreenModeChangeEventArgs(IInteractive source, bool fullscreen)
            : base(CustomWebView.FullscreenEvent, source)
        {
            this.Fullscreen = fullscreen;
        }

        public bool Fullscreen { get; }
    }
}
