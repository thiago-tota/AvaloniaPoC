using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvaloniaWebBrowserControl.DTO;
using AvaloniaWebBrowserControl.Helper;
using CefNet;

namespace AvaloniaWebBrowserControl.Component
{
    public static class CefBrowserComponent
    {
        private static CefAppImpl? app;
        public static CefAppImpl? App { get => app; }

        public static string? CefPath { get; private set; }
        public static CefSettings? CefSettings { get; private set; }

        static CefBrowserComponent()
        {
            try
            {

                bool externalMessagePump = false;

                if (App == default)
                {
                    if (PlatformInfo.IsMacOS)
                    {
                        externalMessagePump = true;
                        CefPath = Path.Combine(GetProjectPath(), "Contents", "Frameworks", "Chromium Embedded Framework.framework");
                    }
                    else
                    {
                        CefPath = Path.Combine(Path.GetDirectoryName(GetProjectPath()), "cef");
                    }

                    //Cef.EnableHighDPISupport();
                    CefSettings = new CefSettings()
                    {
                        //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                        //CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefNet", "Cache"),

                        MultiThreadedMessageLoop = !externalMessagePump,
                        ExternalMessagePump = externalMessagePump,
                        NoSandbox = true,
                        WindowlessRenderingEnabled = true,
                        LocalesDirPath = CefPath,// Path.Combine(CefPath, "Resources", "locales"),
                        ResourcesDirPath = CefPath,// Path.Combine(CefPath, "Resources"),
                        LogSeverity = CefLogSeverity.Warning,
                        IgnoreCertificateErrors = true,
                        UncaughtExceptionStackSize = 8
                    };

                    app = new CefAppImpl
                    {
                        ScheduleMessagePumpWorkCallback = OnScheduleMessagePumpWork
                    };
                }
            }
            catch (Exception ex)
            {
                //Logger.Error(ex);
                throw;
            }
        }

        private static string GetProjectPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;

            string projectPath = Path.GetDirectoryName(typeof(App).Assembly.Location);
            string projectName = typeof(App).Assembly.GetName().Name;
            if (PlatformInfo.IsMacOS) projectName += ".app";
            string rootPath = Path.GetPathRoot(projectPath);
            while (Path.GetFileName(projectPath) != projectName)
            {
                if (projectPath == rootPath)
                    throw new DirectoryNotFoundException("Could not find the project directory.");
                projectPath = Path.GetDirectoryName(projectPath);
            }
            return projectPath;
        }

        private static async void OnScheduleMessagePumpWork(long delayMs)
        {
            await Task.Delay((int)delayMs);
            Dispatcher.UIThread.Post(CefApi.DoMessageLoopWork);
        }

    }
}
