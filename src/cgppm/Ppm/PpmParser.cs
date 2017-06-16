using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgppm.Ppm
{
    public class PpmParser
    {
        /// <summary>
        /// Reads the PBM image data from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="detectMaximumColorValue">Whether the maximum color value should be detected automatically.</param>
        /// <returns>A <see cref="RawPpmImage"/> containing the image data.</returns>
        public RawPpmImage Read(Stream stream, bool detectMaximumColorValue = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            string magicNumber = null;
            int? width = null, height = null;
            ushort? maxColorValue = null;

            // Find image properties
            using (StreamReader sr = new StreamReader(stream))
            {
                // Detect the magic number
                while (magicNumber == null)
                {
                    string nextLine = sr.ReadLine().Trim();
                    if (!IsComment(nextLine))
                    {
                        magicNumber = nextLine;
                        // Detect the maxium color value if specified
                        if (detectMaximumColorValue)
                        {
                            maxColorValue = RawPpmImage.GetDefaultMaximumColorValue(RawPpmImage.GetFileType(magicNumber));
                        }
                    }
                }
                // Detect the image size
                while (!width.HasValue || !height.HasValue)
                {
                    string nextLine = sr.ReadLine().Trim();
                    if (!IsComment(nextLine))
                    {
                        string[] sizes = nextLine.Split(' ');
                        width = int.Parse(sizes[0]);
                        height = int.Parse(sizes[1]);
                    }
                }
                // Detect the maximum color value
                while (!maxColorValue.HasValue)
                {
                    string nextLine = sr.ReadLine().Trim();
                    if (!IsComment(nextLine))
                    {
                        ushort.Parse(nextLine);
                    }

                }
            }

            return new RawPpmImage(magicNumber, width.Value, height.Value, maxColorValue.Value);
        }

        /// <summary>
        /// Reads the PPM image data from the stream.
        /// </summary>
        /// <param name="path">The path to read the file from.</param>
        /// <param name="detectMaximumColorValue">Whether the maximum color value should be detected automatically.</param>
        /// <returns>A <see cref="RawPpmImage"/> containing the image data.</returns>
        public RawPpmImage Read(string path, bool detectMaximumColorValue = false)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

            // IO exceptions will be thrown for us if it can't be found.

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return Read(fs, detectMaximumColorValue);
            }
        }

        /// <summary>
        /// Checks whether the given string is a comment.
        /// </summary>
        /// <param name="line">The string to check.</param>
        /// <returns>A <see cref="bool"/> value indicating whether the given string is a comment.</returns>
        private static bool IsComment(string line)
        {
            if (string.IsNullOrEmpty(line)) throw new ArgumentNullException(nameof(line));
            return line[0] == '#';
        }
    }
}
