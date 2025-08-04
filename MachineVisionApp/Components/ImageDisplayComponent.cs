using System.Windows.Controls;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace MachineVisionApp.Components
{
    public class ImageDisplayComponent
    {
        private Image _originalImage;
        private Image _edgeImage;

        public ImageDisplayComponent(Image originalImage, Image edgeImage)
        {
            _originalImage = originalImage;
            _edgeImage = edgeImage;
        }

        public void UpdateImages(Mat originalFrame, Mat edges)
        {
            _originalImage.Dispatcher.Invoke(() => _originalImage.Source = BitmapSourceConverter.ToBitmapSource(originalFrame));
            _edgeImage.Dispatcher.Invoke(() => _edgeImage.Source = BitmapSourceConverter.ToBitmapSource(edges));
        }
    }
}
