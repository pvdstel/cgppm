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
        /// <param name="name">The name of the image.</param>
        /// <param name="bitmapSource">The bitmap source of the image.</param>
        /// <param name="path">The path of the image.</param>
        public Image(string name, string path, BitmapSource bitmapSource)
        {
            Name = name;
            BitmapSource = bitmapSource;
            Path = path;
        }

        /// <summary>
        /// The name of the image.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The path of the image.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// The bitmap source of the image.
        /// </summary>
        public BitmapSource BitmapSource { get; private set; }
    }
}
