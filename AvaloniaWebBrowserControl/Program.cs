using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using AvaloniaWebBrowserControl.Component;
using CefNet;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaWebBrowserControl
{
    class Program
    {
        private static Timer? messagePump;
        private const int messagePumpDelay = 10;

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            App.FrameworkInitialized += App_FrameworkInitialized;
            App.FrameworkShutdown += App_FrameworkShutdown;

            CefBrowserComponent.App?.Initialize(
                        CefBrowserComponent.CefPath,
                        CefBrowserComponent.CefSettings);

            BuildAvaloniaApp()
            //workaround for https://github.com/AvaloniaUI/Avalonia/issues/3533
            .With(new AvaloniaNativePlatformOptions { UseGpu = false })
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        private static void App_FrameworkInitialized(object sender, EventArgs e)
        {
            if (PlatformInfo.IsMacOS || Environment.GetCommandLineArgs().Contains("--external-message-pump"))
            {
                messagePump = new Timer(_ => Dispatcher.UIThread.Post(CefApi.DoMessageLoopWork), null, messagePumpDelay, messagePumpDelay);
            }
        }

        private static void App_FrameworkShutdown(object sender, EventArgs e)
        {
            messagePump?.Dispose();
            CefBrowserComponent.App?.Shutdown();
        }
    }
}
