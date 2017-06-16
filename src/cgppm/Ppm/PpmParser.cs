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
        /// Reads the PPM image data from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A <see cref="RawPpmImage"/> containing the image data.</returns>
        public RawPpmImage Read(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            string magicNumber;
            int width, height;
            short maxColorValue = byte.MaxValue;
            using (StreamReader sr = new StreamReader(stream))
            {
                magicNumber = sr.ReadLine();

                string[] sizes = sr.ReadLine().Split(' ');
                width = int.Parse(sizes[0]);
                height = int.Parse(sizes[1]);

                maxColorValue = short.Parse(sr.ReadLine());
            }

            return new RawPpmImage(magicNumber, width, height, maxColorValue);
        }

        public RawPpmImage Read(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

            // IO exceptions will be thrown for us if it can't be found.

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return Read(fs);
            }
        }
    }
}
