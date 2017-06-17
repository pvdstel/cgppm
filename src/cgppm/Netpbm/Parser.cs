﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgppm.Netpbm
{
    public class Parser
    {
        /// <summary>
        /// Reads the PBM image data from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="detectMaximumColorValue">Whether the maximum color value should be detected automatically.</param>
        /// <returns>A <see cref="RawImage"/> containing the image data.</returns>
        public RawImage Read(Stream stream, bool detectMaximumColorValue = true)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            string magicNumber = null;
            int? width = null, height = null;
            ushort? maxColorValue = null;
            byte[] imageData;

            // Detect the magic number
            while (magicNumber == null)
            {
                string nextLine = stream.ReadSingleLine();
                if (!nextLine.IsComment())
                {
                    magicNumber = nextLine;
                    // Detect the maxium color value if specified
                    if (detectMaximumColorValue)
                    {
                        maxColorValue = RawImage.GetDefaultMaximumColorValue(RawImage.GetImageFormat(magicNumber));
                    }
                }
            }
            // Detect the image size
            while (!width.HasValue || !height.HasValue)
            {
                string nextLine = stream.ReadSingleLine();
                if (!nextLine.IsComment())
                {
                    string[] sizes = nextLine.Split(' ');
                    width = int.Parse(sizes[0]);
                    height = int.Parse(sizes[1]);
                }
            }
            // Detect the maximum color value
            while (!maxColorValue.HasValue)
            {
                string nextLine = stream.ReadSingleLine();
                if (!nextLine.IsComment())
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

            return new RawImage(magicNumber, width.Value, height.Value, maxColorValue.Value, imageData);
        }

        /// <summary>
        /// Reads the PPM image data from the stream.
        /// </summary>
        /// <param name="path">The path to read the file from.</param>
        /// <param name="detectMaximumColorValue">Whether the maximum color value should be detected automatically.</param>
        /// <returns>A <see cref="RawImage"/> containing the image data.</returns>
        public RawImage Read(string path, bool detectMaximumColorValue = false)
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
