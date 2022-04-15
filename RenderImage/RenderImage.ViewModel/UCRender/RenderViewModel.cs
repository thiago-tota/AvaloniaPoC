using System;
using RenderImage.Shared.Helper;
using RenderImage.ViewModel.UCMediaStream;

namespace RenderImage.ViewModel.UCRender
{
    public class RenderViewModel : ViewModelBase
    {
        public RenderViewModel()
        {
            StartCommand = new RelayCommand(StartCommandExecute);
            StopCommand = new RelayCommand(StopCommandExecute);

            MediaStreams = new MediaStreamViewModel();
        }

        #region Commands declaration

        public RelayCommand StartCommand { get; set; }
        public RelayCommand StopCommand { get; set; }

        #endregion

        #region Private declaration

        private bool _isDisposed;

        #endregion Private declaration       

        #region Binding Properties

        private MediaStreamViewModel? mediaStreams;
        public MediaStreamViewModel? MediaStreams { get => mediaStreams; set => SetProperty(ref mediaStreams, value); }

        private bool isRendering;
        public bool IsRendering { get => isRendering; set => SetProperty(ref isRendering, value); }

        #endregion Binding Properties

        private void StartCommandExecute()
        {
            MediaStreams?.StartRender();
            IsRendering = true;
        }

        private void StopCommandExecute()
        {
            MediaStreams?.StopRender();
            IsRendering = false;
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
                MediaStreams?.Dispose();
            }

            //Free native (unmanaged) resources
            //TODO: Free resources

            _isDisposed = true;
        }
    }
}
