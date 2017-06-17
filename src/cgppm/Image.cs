using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace cgppm
{
    /// <summary>
    /// Represents an image.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="bitmapSource">The bitmap source of the image.</param>
        /// <param name="tag">The tag of the image.</param>
        public Image(BitmapSource bitmapSource, string tag)
        {
            BitmapSource = bitmapSource;
            Tag = tag;
        }

        /// <summary>
        /// The bitmap source of the image.
        /// </summary>
        public BitmapSource BitmapSource { get; private set; }

        /// <summary>
        /// The tag of the image.
        /// </summary>
        public string Tag { get; private set; } = string.Empty;
    }
}
