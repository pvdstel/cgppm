using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Reads a single line of text from stream.
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
    }
}
