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
        public short MaximumColorValue { get; private set; } = byte.MaxValue;
    }
}
