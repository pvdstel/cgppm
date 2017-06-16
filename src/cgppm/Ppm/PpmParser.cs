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
        private const byte LineFeed = 10;
        private const byte CarriageReturn = 13;

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
            byte[] imageData;

            // Detect the magic number
            while (magicNumber == null)
            {
                string nextLine = GetStreamLine(stream);
                if (!IsComment(nextLine))
                {
                    magicNumber = nextLine;
                    // Detect the maxium color value if specified
                    if (detectMaximumColorValue)
                    {
                        maxColorValue = RawPpmImage.GetDefaultMaximumColorValue(RawPpmImage.GetImageFormat(magicNumber));
                    }
                }
            }
            // Detect the image size
            while (!width.HasValue || !height.HasValue)
            {
                string nextLine = GetStreamLine(stream);
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
                string nextLine = GetStreamLine(stream);
                if (!IsComment(nextLine))
                {
                    maxColorValue = ushort.Parse(nextLine);
                }

            }

            // Get the image data
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            return new RawPpmImage(magicNumber, width.Value, height.Value, maxColorValue.Value, imageData);
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

        /// <summary>
        /// Reads a single line of text from stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A <see cref="string"/> with the line that was read.</returns>
        private static string GetStreamLine(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new InvalidOperationException("The stream cannot be read.");
            if (stream.Length == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            int nextByte = stream.ReadByte();
            while (nextByte > 0 && nextByte != LineFeed && nextByte != CarriageReturn)
            {
                sb.Append((char)nextByte);
                nextByte = stream.ReadByte();
            }
            return sb.ToString();
        }
    }
}
