using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RenderImage.Shared.Dto;
using RenderImage.Shared.Helper;
using SixLabors.ImageSharp;

namespace RenderImage.ViewModel.UCMediaStream
{
    public class MediaStreamViewModel : BindableBase, IDisposable
    {
        public MediaStreamViewModel()
        {
            InitialiseDefaultSettings();
        }

        #region Private Properties

        private const uint _streamWidth = 180;
        private const uint _streamHeight = 135;

        private CancellationTokenSource _renderImagesTimerCancellationToken;
        private List<string>? _imageList;
        private readonly List<string> _imageListCopy = new();

        #endregion

        #region Binding Properties

        public ObservableCollection<MediaStream>? mediaStreams;
        public ObservableCollection<MediaStream>? MediaStreams
        {
            get => mediaStreams;
            set
            {
                SetProperty(ref mediaStreams, value);

                if (mediaStreams != null)
                {
                    foreach (var item in mediaStreams)
                    {
                        item.PropertyChanged -= MediaStreams_PropertyChanged;
                        item.PropertyChanged += MediaStreams_PropertyChanged;
                    }
                }
            }
        }

        private void MediaStreams_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e.PropertyName);
        }

        #endregion

        private void InitialiseDefaultSettings()
        {
            MediaStreams = new ObservableCollection<MediaStream>();
        }

        private void LoadImages(string path)
        {
            var files = Directory.GetFiles(path);
            _imageList = new List<string>();
            _imageList.AddRange(files);
        }

        private void RenderImagesTimer(int interval, CancellationTokenSource cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _imageListCopy?.AddRange(_imageList ?? new());

                for (int i = 0; i < _imageList?.Count; i++)
                {
                    int randomIndex = new Random().Next(0, _imageListCopy.Count);
                    MediaStreams[i].Stream = Image.Load(_imageListCopy[randomIndex]);
                    MediaStreams[i].UpdatedAt = DateTime.UtcNow;

                    _imageListCopy.RemoveAt(randomIndex);
                }
            }
        }

        public void StartRender()
        {
            LoadImages("Assets");
            MediaStreams?.Clear();
            for (int i = 0; i < _imageList?.Count; i++)
                MediaStreams?.Add(new MediaStream
                {
                    Id = Guid.NewGuid(),
                    Height = _streamHeight,
                    Width = _streamWidth
                });
            _renderImagesTimerCancellationToken = new CancellationTokenSource();
            Task.Run(() => RenderImagesTimer(500, _renderImagesTimerCancellationToken));
        }

        public void StopRender()
        {
            _renderImagesTimerCancellationToken.Cancel();
        }

        public override void Dispose()
        {
            _renderImagesTimerCancellationToken.Cancel();
            _renderImagesTimerCancellationToken.Dispose();
            MediaStreams?.Clear();
            MediaStreams = default;
        }
    }
}
