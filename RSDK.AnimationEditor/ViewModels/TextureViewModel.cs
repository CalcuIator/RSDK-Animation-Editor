using AnimationEditor.Services;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;

namespace AnimationEditor.ViewModels
{
    public class TextureViewModel : Xe.Tools.Wpf.BaseNotifyPropertyChanged
    {
        private bool _isLoaded = false;
        private BitmapSource _image;
        private string _strSize;
        private string _strFormat;

        public string Texture { get; private set; }

        public string FileName { get; private set; }

        public BitmapSource Image
        {
            get
            {
                RequireImage();
                return _image;
            }
            set
            {
                _image = value;
                if (_image != null)
                {
                    _strSize = $"{_image.PixelWidth}x{_image.PixelHeight}";
                    //if (_image Palette?.Colors.Count > 0)
                    //{
                    //    _strFormat = $"{_image.Format.BitsPerPixel}bpp, palette {_image.Palette.Colors} colors";
                    //}
                    //else
                    //{
                    //    _strFormat = $"{_image.Format.BitsPerPixel}bpp";
                    //}
                }
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(StrSize));
                //OnPropertyChanged(nameof(StrFormat));
            }
        }

        public string StrSize => _strSize;

        public string StrFormat => _strFormat;

        public TextureViewModel(string texture, string basePath)
        {
            Texture = texture;
            FileName = Path.Combine(basePath, texture);
        }

        private void RequireImage()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                Image = ImageService.Open(FileName);
            }
        }

        public override string ToString()
        {
            return Texture ?? base.ToString();
        }
    }
}
