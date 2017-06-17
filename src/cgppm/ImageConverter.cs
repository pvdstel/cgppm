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

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageConverter"/> class.
        /// </summary>
        /// <param name="dpiX">The X dpi value.</param>
        /// <param name="dpiY">The Y dpi value.</param>
        public ImageConverter(double dpiX = DefaultDpiX, double dpiY = DefaultDpiY)
        {
            _dpiX = dpiX;
            _dpiY = dpiY;
        }
        #endregion

        /// <summary>
        /// Converts a Netbpm image to an 8-bit <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="rawImage">The Netbpm image.</param>
        /// <returns>A <see cref="BitmapSource"/> representing the given image.</returns>
        public BitmapSource ConvertNetpbmTo8Bit(RawImage rawImage)
        {
            PixelFormat pixelFormat = Get8BitPixelFormat(rawImage.ImageFormat);

            byte pixelFormatMaximumValue = Get8BitMaxColorValue(rawImage.ImageFormat);
            byte[] imageData;
            if (rawImage.MaximumColorValue != pixelFormatMaximumValue)
            {
                imageData = new NormalizedImage(rawImage).GetAsByteArray(pixelFormatMaximumValue);
            }
            else
            {
                imageData = rawImage.ImageData.Select(us => (byte)us).ToArray();
            }

            int bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8;
            int stride = bytesPerPixel * rawImage.Width;

            return BitmapSource.Create(rawImage.Width, rawImage.Height,
                _dpiX, _dpiY, pixelFormat, null, imageData, stride);
        }

        /// <summary>
        /// Converts a Netbpm image to an 16-bit <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="rawImage">The Netbpm image.</param>
        /// <returns>A <see cref="BitmapSource"/> representing the given image.</returns>
        public BitmapSource ConvertNetpbmTo16Bit(RawImage rawImage)
        {
            PixelFormat pixelFormat = Get16BitPixelFormat(rawImage.ImageFormat);

            ushort pixelFormatMaximumValue = Get16BitMaxColorValue(rawImage.ImageFormat);
            ushort[] imageData;
            if (rawImage.MaximumColorValue != pixelFormatMaximumValue)
            {
                imageData = new NormalizedImage(rawImage).GetAsUInt16Array(pixelFormatMaximumValue);
            }
            else
            {
                imageData = rawImage.ImageData;
            }

            int bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8;
            int stride = bytesPerPixel * rawImage.Width;

            return BitmapSource.Create(rawImage.Width, rawImage.Height,
                _dpiX, _dpiY, pixelFormat, null, imageData, stride);
        }

        /// <summary>
        /// Gets the pixel format for an 8 bit image.
        /// </summary>
        /// <param name="format">The format of the Netbpm image.</param>
        /// <returns>A <see cref="PixelFormat"/> representing the correct pixelformat.</returns>
        public static PixelFormat Get8BitPixelFormat(Formats format)
        {
            switch (format)
            {
                case Formats.PortableBitMap:
                case Formats.PortableGrayMap:
                    return PixelFormats.Gray8;
                case Formats.PortablePixMap:
                    return PixelFormats.Rgb24;
                default:
                    return PixelFormats.BlackWhite;
            }
        }

        /// <summary>
        /// Get the maximum color value for an 8 bit image format.
        /// </summary>
        /// <param name="format">The format of the Netbpm image.</param>
        /// <returns>A <see cref="byte"/> with the maximum value for the specified format.</returns>
        public static byte Get8BitMaxColorValue(Formats format)
        {
            return byte.MaxValue;
        }

        /// <summary>
        /// Gets the pixel format for a 16 bit image.
        /// </summary>
        /// <param name="format">The format of the Netbpm image.</param>
        /// <returns>A <see cref="PixelFormat"/> representing the correct pixelformat.</returns>
        public static PixelFormat Get16BitPixelFormat(Formats format)
        {
            switch (format)
            {
                case Formats.PortableBitMap:
                case Formats.PortableGrayMap:
                    return PixelFormats.Gray16;
                case Formats.PortablePixMap:
                    return PixelFormats.Rgb48;
                default:
                    return PixelFormats.BlackWhite;
            }
        }

        /// <summary>
        /// Get the maximum color value for a 16 bit image format.
        /// </summary>
        /// <param name="format">The format of the Netbpm image.</param>
        /// <returns>A <see cref="ushort"/> with the maximum value for the specified format.</returns>
        public static ushort Get16BitMaxColorValue(Formats format)
        {
            return ushort.MaxValue;
        }
    }
}
