using System.Collections.Generic;
using RenderImage.Shared.Helper;

namespace RenderImage.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
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

        private string? notificationMessage = "Welcome to example application.";
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
