using System;

namespace cgppm.Netpbm
{
    public class NormalizedImage
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedImage"/> class.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="imageData">The normalized image data.</param>
        public NormalizedImage(int width, int height, double[] imageData)
        {
            if (imageData == null) throw new ArgumentNullException(nameof(imageData));
            if (!ValidateNormalizedData(imageData)) throw new FormatException("The image data is not normalized.");

            Width = width;
            Height = height;
            ImageData = imageData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedImage"/> class.
        /// </summary>
        /// <param name="rawImage">The <see cref="RawImage"/> to create a normalized image from.</param>
        public NormalizedImage(RawImage rawImage) : this(rawImage.Width, rawImage.Height, NormalizeImageData(rawImage.ImageData, rawImage.MaximumColorValue))
        {

        }
        #endregion

        #region Properties

        /// <summary>
        /// The width of the image.
        /// </summary>
        public int Width { get; private set; } = 0;

        /// <summary>
        /// The height of the image.
        /// </summary>
        public int Height { get; private set; } = 0;

        /// <summary>
        /// The normalized image data of the image.
        /// </summary>
        public double[] ImageData { get; private set; }
        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format(nameof(NormalizedImage) + "({0}, {1}, {2})", Width, Height, ImageData.Length);
        }

        /// <summary>
        /// Normalizes image data to values between 0 and 1.
        /// </summary>
        /// <param name="rawData">The raw image data.</param>
        /// <param name="maximumColorValue">The maximum color value.</param>
        /// <returns></returns>
        public static double[] NormalizeImageData(byte[] rawData, ushort maximumColorValue)
        {
            bool useDoubleBytes = maximumColorValue > byte.MaxValue;
            double tempMaximumColorValue = maximumColorValue;

            double[] normalizedData = new double[useDoubleBytes ? rawData.Length >> 1 : rawData.Length];
            for (int i = 0, j = 0; i < rawData.Length; i++, j++)
            {
                double normalized;
                if (useDoubleBytes)
                {
                    normalized = ((rawData[i] << 8) + rawData[++i]) / tempMaximumColorValue;
                }
                else
                {
                    normalized = rawData[i] / tempMaximumColorValue;
                }
                normalizedData[j] = Math.Min(normalized, 1);
            }

            return normalizedData;
        }

        /// <summary>
        /// Checks if the provided data is normalized.
        /// </summary>
        /// <param name="normalizedData">The data to check.</param>
        /// <returns>A <see cref="bool"/> value indicating whether the data is normalized.</returns>
        private static bool ValidateNormalizedData(double[] normalizedData)
        {
            if (normalizedData == null) throw new ArgumentNullException(nameof(normalizedData));

            for (int i = 0; i < normalizedData.Length; i++)
            {
                if (normalizedData[i] < 0 || normalizedData[i] > 1)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
