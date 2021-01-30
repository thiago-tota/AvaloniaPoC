using Avalonia.Media.Imaging;
using Common.Helper;
using System;

namespace Component.DTO
{
    public class MediaStream : BindableBase
    {
        private Guid id;
        public Guid Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private Bitmap stream;
        public Bitmap Stream
        {
            get => stream;
            set => SetProperty(ref stream, value);
        }

        private DateTime updatedAt;
        public DateTime UpdatedAt
        {
            get => updatedAt;
            set => SetProperty(ref updatedAt, value);
        }

        private uint width;
        public uint Width { get => width; set => SetProperty(ref width, value); }

        private uint height;
        public uint Height { get => height; set => SetProperty(ref height, value); }
    }
}
