using cgppm.Netpbm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cgppm
{
    public class ImageConverter
    {
        #region Constants

        public const double DefaultDpiX = 96;
        public const double DefaultDpiY = 96;
        #endregion

        #region Fields

        private double _dpiX;
        private double _dpiY;
        #endregion

        #region Constructor

        public ImageConverter(double dpiX = DefaultDpiX, double dpiY = DefaultDpiY)
        {
            _dpiX = dpiX;
            _dpiY = dpiY;
        }
        #endregion

        public BitmapSource ConvertNetpbmTo8Bit(RawImage rawImage)
        {
            PixelFormat pixelFormat = Get8BitPixelFormat(rawImage.ImageFormat);

            byte[] imageData = rawImage.ImageData;
            if (rawImage.MaximumColorValue != byte.MaxValue)
            {
                imageData = new NormalizedImage(rawImage).GetAsByteArray(byte.MaxValue);
            }

            int bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8;
            int stride = bytesPerPixel * rawImage.Width;

            return BitmapSource.Create(rawImage.Width, rawImage.Height,
                _dpiX, _dpiY, pixelFormat, null, imageData, stride);
        }

        public static PixelFormat Get8BitPixelFormat(Formats format)
        {
            switch (format)
            {
                case Formats.PortableBitMap:
                    return PixelFormats.BlackWhite;
                case Formats.PortableGrayMap:
                    return PixelFormats.Gray8;
                case Formats.PortablePixMap:
                    return PixelFormats.Rgb24;
                default:
                    return PixelFormats.BlackWhite;
            }
        }
    }
}
