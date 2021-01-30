using Anotar.Serilog;
using AvaloniaRenderImage.Helper;
using Common.Helper;
using Component;
using System;
using System.Reflection;

namespace AvaloniaRenderImage.UCRender
{
    internal class RenderViewModel : ViewModelBase
    {
        public RenderViewModel()
        {
            StartCommand = new RelayCommand(StartCommandExecute);
            StopCommand = new RelayCommand(StopCommandExecute);
            CloseCommand = new RelayCommand(CloseCommandExecute);

            MediaStreams = new UCMediaStreamViewModel();
        }

        #region Commands declaration

        public RelayCommand StartCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        #endregion

        #region Private declaration

        private bool _isDisposed;

        #endregion Private declaration       

        #region Binding Properties

        private UCMediaStreamViewModel? mediaStreams;
        public UCMediaStreamViewModel? MediaStreams { get => mediaStreams; set => SetProperty(ref mediaStreams, value); }

        private uint streamWidth;
        public uint StreamWidth { get => streamWidth; set => SetProperty(ref streamWidth, value); }

        private uint streamHeight;
        public uint StreamHeight { get => streamHeight; set => SetProperty(ref streamHeight, value); }

        private bool isRendering;
        public bool IsRendering { get => isRendering; set => SetProperty(ref isRendering, value); }

        #endregion Binding Properties

        private void StartCommandExecute()
        {
            try
            {
                MediaStreams?.StartRender();
                IsRendering = true;
            }
            catch (Exception ex)
            {
                LogTo.Error(ex, MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        private bool StartCommandCanExecute()
        {
            try
            {
                return !IsRendering;
            }
            catch (Exception ex)
            {
                LogTo.Error(ex, MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        private void StopCommandExecute()
        {
            try
            {
                MediaStreams?.StopRender();
                IsRendering = false;
            }
            catch (Exception ex)
            {
                LogTo.Error(ex, MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        private bool StopCommandCanExecute()
        {
            try
            {
                return IsRendering;
            }
            catch (Exception ex)
            {
                LogTo.Error(ex, MethodBase.GetCurrentMethod().Name);
                throw;
            }
        }

        private void CloseCommandExecute()
        {
            try
            {
                //Application.Current.Exit();
            }
            catch (Exception ex)
            {
                LogTo.Error(ex, MethodBase.GetCurrentMethod().Name);
                ExecuteNotificationMessageEvent(ex.Message);
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                //Free managed resources
                CloseCommandExecute();
                MediaStreams?.Dispose();
            }

            //Free native (unmanaged) resources
            //TODO: Free resources

            _isDisposed = true;
        }
    }
}
