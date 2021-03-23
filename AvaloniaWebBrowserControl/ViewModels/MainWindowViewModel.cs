using AvaloniaWebBrowserControl.Component;
using AvaloniaWebBrowserControl.Helper;
using CefNet;

namespace AvaloniaWebBrowserControl.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            NavigateCommand = new RelayCommand(NavigateCommandExecute);

            var viewTab = new WebViewTab();
            //viewTab.WebView.Navigated += WebView_Navigated;
            //((AvaloniaList<object>)tabs.Items).Add(viewTab);
            viewTab.Title = "about:blank";
            ChromiumWebView = viewTab.WebView;
        }

        public RelayCommand NavigateCommand { get; set; }

        private IChromiumWebView? chromiumWebView;
        public IChromiumWebView? ChromiumWebView
        {
            get => chromiumWebView; set
            {
                SetProperty(ref chromiumWebView, value);
            }
        }

        private void NavigateCommandExecute()
        {
            ChromiumWebView?.Navigate("www.google.com");
        }
    }
}
