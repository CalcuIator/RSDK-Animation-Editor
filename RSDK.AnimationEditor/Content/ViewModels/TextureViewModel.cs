using Microsoft.UI.Xaml.Media.Imaging;
using RSDK.AnimationEditor.Content.Services;
using System.IO;

namespace RSDK.AnimationEditor.Content.ViewModels
{
    public class TextureViewModel : Xe.Tools.Wpf.BaseNotifyPropertyChanged
    {
        private bool _isLoaded = false;
        private BitmapSource _image;
        private string _strSize;

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
                }
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(StrSize));
            }
        }

        public string StrSize => _strSize;

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
