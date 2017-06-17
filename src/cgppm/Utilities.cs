using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace cgppm
{
    public static class Utilities
    {
        private const byte LineFeed = 10;
        private const byte CarriageReturn = 13;

        /// <summary>
        /// Checks whether the given string is a comment.
        /// </summary>
        /// <param name="line">The string to check.</param>
        /// <returns>A <see cref="bool"/> value indicating whether the given string is a comment.</returns>
        public static bool IsComment(this string line)
        {
            if (string.IsNullOrEmpty(line)) throw new ArgumentNullException(nameof(line));
            return line[0] == '#';
        }

        /// <summary>
        /// Reads a single line of text from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A <see cref="string"/> with the line that was read.</returns>
        public static string ReadSingleLine(this Stream stream)
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

        /// <summary>
        /// Reads a single word from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A <see cref="string"/> with the word that was read.</returns>
        public static string ReadSingleWord(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new InvalidOperationException("The stream cannot be read.");
            if (stream.Length == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            int nextByte = stream.ReadByte();
            bool foundNonWhitespace = false;
            while (nextByte > 0)
            {
                char next = (char)nextByte;
                if (next == '#') // start of a comment, go to next line
                {
                    stream.ReadSingleLine();
                    sb.Clear();
                    foundNonWhitespace = false;
                    nextByte = stream.ReadByte();
                    continue;
                }

                bool isSpecialChar = char.IsWhiteSpace(next) || char.IsControl(next) || char.IsSeparator(next);
                if (isSpecialChar && foundNonWhitespace)
                {
                    // End of the word, break
                    break;
                }

                foundNonWhitespace |= !isSpecialChar;
                sb.Append(next);
                nextByte = stream.ReadByte();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Saves a <see cref="BitmapSource"/> as a PNG file to the specified location.
        /// </summary>
        /// <param name="bitmapSource">The <see cref="BitmapSource"/></param>
        /// <param name="path">The path to save to.</param>
        public static void SaveBitmapSourceAsPng(this BitmapSource bitmapSource, string path)
        {
            if (bitmapSource == null) throw new ArgumentNullException(nameof(bitmapSource));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fs);
            }
        }

        /// <summary>
        /// Saves a <see cref="BitmapSource"/> as a JPG file to the specified location.
        /// </summary>
        /// <param name="bitmapSource">The <see cref="BitmapSource"/></param>
        /// <param name="path">The path to save to.</param>
        public static void SaveBitmapSourceAsJpg(this BitmapSource bitmapSource, string path)
        {
            if (bitmapSource == null) throw new ArgumentNullException(nameof(bitmapSource));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fs);
            }
        }

        /// <summary>
        /// Saves a <see cref="BitmapSource"/> as a BMP file to the specified location.
        /// </summary>
        /// <param name="bitmapSource">The <see cref="BitmapSource"/></param>
        /// <param name="path">The path to save to.</param>
        public static void SaveBitmapSourceAsBmp(this BitmapSource bitmapSource, string path)
        {
            if (bitmapSource == null) throw new ArgumentNullException(nameof(bitmapSource));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fs);
            }
        }
    }
}
