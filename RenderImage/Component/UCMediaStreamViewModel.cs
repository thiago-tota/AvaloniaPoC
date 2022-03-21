using Avalonia.Media.Imaging;
using Common.Helper;
using Component.Dto;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Timers;

namespace Component
{
    public class UCMediaStreamViewModel : BindableBase, IDisposable
    {
        public UCMediaStreamViewModel()
        {
            InitialiseDefaultSettings();
        }

        #region Private Properties

        private uint _streamWidth = 160;
        private uint _streamHeight = 120;

        private Timer _renderImagesTimer;
        private List<string> _imageList;
        private List<string> _imageListCopy = new List<string>();

        #endregion

        #region Binding Properties

        public ObservableCollection<MediaStream> mediaStreams;
        public ObservableCollection<MediaStream> MediaStreams
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
            _renderImagesTimer = new Timer(500);
            _renderImagesTimer.Elapsed += RenderImagesTimer_Elapsed;

            MediaStreams = new ObservableCollection<MediaStream>();
        }

        private void LoadImages(string path)
        {
            var files = Directory.GetFiles(path);
            _imageList = new List<string>();
            _imageList.AddRange(files);
        }

        private void RenderImagesTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                int n = 0;
                _imageListCopy.AddRange(_imageList);

                foreach (var item in MediaStreams)
                {
                    n = new Random().Next(0, _imageListCopy.Count);
                    item.Stream = new Bitmap(_imageListCopy[n]);
                    item.UpdatedAt = DateTime.UtcNow;
                    _imageListCopy.RemoveAt(n);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public void StartRender()
        {
            try
            {
                LoadImages("Assets");
                MediaStreams?.Clear();
                for (int i = 0; i < _imageList.Count; i++)
                    MediaStreams.Add(new MediaStream
                    {
                        Id = Guid.NewGuid(),
                        Height = _streamHeight,
                        Width = _streamWidth
                    });

                _renderImagesTimer.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public void StopRender()
        {
            try
            {
                _renderImagesTimer.Stop();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public void SetStreamSize(uint width, uint height)
        {
            try
            {
                _streamWidth = width;
                _streamHeight = height;

                foreach (var item in MediaStreams)
                {
                    item.Width = _streamWidth;
                    item.Height = _streamHeight;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod()?.Name);
            }
        }

        public bool HasStream()
        {
            return MediaStreams?.Count > 0;
        }

        public override void Dispose()
        {
            try
            {
                _renderImagesTimer.Stop();
                MediaStreams?.Clear();
                MediaStreams = default;
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod()?.Name);
            }
        }
    }
}
