using Anotar.Serilog;
using AvaloniaRenderImage.Helper;
using Common.Helper;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AvaloniaRenderImage
{
    internal class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            InitialiseSettings();
            NavigationCommand = new RelayCommand<string>(OnNavigate);

            //Load initial user contorl
            OnNavigate("UserControlRender");
        }

        #region Commands declarations

        public RelayCommand<string> NavigationCommand { get; set; }
        #endregion

        #region Private declarations

        private readonly Dictionary<View, ViewModelBase> ViewObjects = new Dictionary<View, ViewModelBase>();

        private enum View : byte
        {
            Render = 0
        }

        #endregion

        #region Binding Properties

        private BindableBase? currentViewModel;
        public BindableBase? CurrentViewModel
        {
            get => currentViewModel; set
            {
                SetProperty(ref currentViewModel, value);
            }
        }

        private string? notificationMessage = "Welcome to meeting application.";
        public string? NotificationMessage
        {
            get => notificationMessage; set
            {
                SetProperty(ref notificationMessage, value);
            }
        }

        #endregion

        private void OnNavigate(string destination)
        {
            try
            {
                switch (destination)
                {
                    case "UserControlRender":
                        if (!ViewObjects.ContainsKey(View.Render))
                        {
                            ViewObjects.Add(View.Render, new UCRender.RenderViewModel());
                            StartStopViewModelBaseEventsListener(ViewObjects[View.Render], true);
                        }

                        CurrentViewModel = ViewObjects[View.Render];
                        break;
                }
            }
            catch (Exception ex)
            {
                LogTo.Error(ex, MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        private void InitialiseSettings()
        {
            var configuration = new ConfigurationBuilder()
                            .SetBasePath($"{Directory.GetCurrentDirectory()}")
                            .AddJsonFile("appsettings.json", false, true)
                            .Build();

            Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateLogger();

            LogTo.Information($"{AppDomain.CurrentDomain.FriendlyName} has started at {DateTime.Now}");
        }


        public void StartStopViewModelBaseEventsListener(ViewModelBase viewModel, bool start)
        {
            if (start)
            {
                viewModel.OnNotificationMessage += ViewModel_OnNotificationMessage;
                viewModel.OnNavigate += ViewModel_OnNavigate;
            }
            else
            {
                viewModel.OnNotificationMessage -= ViewModel_OnNotificationMessage;
                viewModel.OnNavigate -= ViewModel_OnNavigate;
            }
        }

        private void ViewModel_OnNotificationMessage(string obj)
        {
            NotificationMessage = obj;
        }

        private void ViewModel_OnNavigate(string destination)
        {
            OnNavigate(destination);
        }

        public override void Dispose()
        {
            foreach (var item in ViewObjects)
            {
                StartStopViewModelBaseEventsListener(item.Value, false);
            }

            ViewObjects.Clear();
            CurrentViewModel?.Dispose();
            CurrentViewModel = default;
        }
    }
}
