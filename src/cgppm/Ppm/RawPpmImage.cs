using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgppm.Ppm
{
    /// <summary>
    /// Represents a raw PPM image.
    /// </summary>
    public class RawPpmImage
    {
        #region Constants

        public const ushort DefaultMaximumColorValue = ushort.MaxValue;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RawPpmImage"/> class.
        /// </summary>
        /// <param name="magicNumber">The magic number of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="maximumColorValue">The maximum color value of the image.</param>
        public RawPpmImage(string magicNumber, int width, int height, ushort maximumColorValue, byte[] imageData)
        {
            MagicNumber = magicNumber;
            Width = width;
            Height = height;
            MaximumColorValue = maximumColorValue;
            ImageData = imageData;

            DetermineProperties();
        }

        private void DetermineProperties()
        {
            // warning: dumb code ahead.
            ImageFormat = GetImageFormat(MagicNumber);
            IsBinary = GetIsBinary(MagicNumber);
        }
        #endregion

        #region Properties

        /// <summary>
        /// The magic number of the PPM image.
        /// </summary>
        public string MagicNumber { get; private set; } = string.Empty;

        /// <summary>
        /// The width of the image.
        /// </summary>
        public int Width { get; private set; } = 0;

        /// <summary>
        /// The height of the image.
        /// </summary>
        public int Height { get; private set; } = 0;

        /// <summary>
        /// The maximum color value of the image. The default value is the highest possible value for a byte.
        /// </summary>
        public ushort MaximumColorValue { get; private set; } = byte.MaxValue;

        /// <summary>
        /// The image data of this image.
        /// </summary>
        public byte[] ImageData { get; private set; }

        /// <summary>
        /// The image format.
        /// </summary>
        public ImageFormats ImageFormat { get; private set; } = ImageFormats.Unknown;

        /// <summary>
        /// Whether this image is in binary format.
        /// </summary>
        public bool IsBinary { get; private set; }
        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format(nameof(RawPpmImage) + "({0}, {1], {2}, {3}, length)", MagicNumber, Width, Height, MaximumColorValue);
        }

        /// <summary>
        /// Gets the iamge format from the given magic number.
        /// </summary>
        /// <param name="magicNumber">The magic number.</param>
        /// <returns>A <see cref="ImageFormats"/> with the detected value.</returns>
        public static ImageFormats GetImageFormat(string magicNumber)
        {
            switch (magicNumber)
            {
                case "P1":
                case "P4":
                    return ImageFormats.PortableBitMap;
                case "P2":
                case "P5":
                    return ImageFormats.PortableGrayMap;
                case "P3":
                case "P6":
                    return ImageFormats.PortablePixMap;
                default:
                    return ImageFormats.Unknown;
            }
        }

        /// <summary>
        /// Gets whether the magic number indicates a binary format.
        /// </summary>
        /// <param name="magicNumber">A magic number.</param>
        /// <returns>A <see cref="bool"/> indicating whether the magic number represents a binary format.</returns>
        public static bool GetIsBinary(string magicNumber)
        {
            return (magicNumber == "P4") || (magicNumber == "P5") || (magicNumber == "P6");
        }

        /// <summary>
        /// Gets the default maximum color value as defined in the specification.
        /// </summary>
        /// <param name="imageFormat">The format of the image.</param>
        /// <returns>The appropriate value if defined, null if otherwise.</returns>
        public static ushort? GetDefaultMaximumColorValue(ImageFormats imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormats.PortableBitMap:
                    return 1;
                case ImageFormats.PortableGrayMap:
                    return byte.MaxValue;
                case ImageFormats.PortablePixMap:
                    return byte.MaxValue;
                case ImageFormats.Unknown:
                default:
                    return null;
            }
        }
        #endregion
    }
}
