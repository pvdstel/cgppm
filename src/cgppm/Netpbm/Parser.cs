using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgppm.Netpbm
{
    public class Parser
    {
        public const bool DefaultDetectMaximumColorValue = false;

        /// <summary>
        /// Reads the PBM image data from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="detectMaximumColorValue">Whether the maximum color value should be detected automatically.</param>
        /// <returns>A <see cref="RawImage"/> containing the image data.</returns>
        public RawImage Read(Stream stream, bool detectMaximumColorValue = DefaultDetectMaximumColorValue)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            string magicNumber = null;
            int? width = null, height = null;
            ushort? maxColorValue = null;
            ushort[] imageData;

            // Get header information
            while (width == null || height == null || maxColorValue == null || magicNumber == null)
            {
                if (magicNumber == null)
                {
                    magicNumber = stream.ReadSingleWord();
                    Formats imageFormat = RawImage.GetImageFormat(magicNumber);
                    if (imageFormat == Formats.PortableBitMap)
                    {
                        maxColorValue = 1;
                    }
                    else if (detectMaximumColorValue)
                    {
                        maxColorValue = RawImage.GetDefaultMaximumColorValue(imageFormat);
                    }
                }
                else if (width == null)
                {
                    width = int.Parse(stream.ReadSingleWord());
                }
                else if (height == null)
                {
                    height = int.Parse(stream.ReadSingleWord());
                }
                else if (maxColorValue == null)
                {
                    maxColorValue = ushort.Parse(stream.ReadSingleWord());
                }
            }

            // Get the image data
            if (RawImage.GetIsBinary(magicNumber))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    imageData = ms.ToArray().Select(b => (ushort)b).ToArray();
                }
            }
            else
            {
                List<ushort> pixelValues = new List<ushort>();
                using (StreamReader sr = new StreamReader(stream))
                {
                    string nextLine = sr.ReadLine();
                    while (nextLine != null)
                    {
                        string[] rawValues = nextLine.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string rawValue in rawValues)
                        {
                            pixelValues.Add(ushort.Parse(rawValue));
                        }
                        nextLine = sr.ReadLine();
                    }
                }
                imageData = pixelValues.ToArray();
            }

            return new RawImage(magicNumber, width.Value, height.Value, maxColorValue.Value, imageData);
        }

        /// <summary>
        /// Reads the PPM image data from the stream.
        /// </summary>
        /// <param name="path">The path to read the file from.</param>
        /// <param name="detectMaximumColorValue">Whether the maximum color value should be detected automatically.</param>
        /// <returns>A <see cref="RawImage"/> containing the image data.</returns>
        public RawImage Read(string path, bool detectMaximumColorValue = DefaultDetectMaximumColorValue)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

            // IO exceptions will be thrown for us if it can't be found.

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return Read(fs, detectMaximumColorValue);
            }
        }
    }
}
