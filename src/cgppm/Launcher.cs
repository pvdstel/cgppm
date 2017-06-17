using cgppm.Netpbm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cgppm
{
    public class Launcher
    {
        private static List<Image> _convertedImages = new List<Image>();

        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            // Gather input data
            Console.Write("Parsing arguments... ");
            List<string> switches = args.Where(s => s[0] == '-' || s[0] == '/').Select(s => s.Substring(1).ToLower()).ToList();
            List<string> files = args.Where(s => File.Exists(s)).ToList();
            Console.WriteLine("done.");

            // Files count check
            if (files.Count == 0)
            {
                Console.WriteLine("No files were found. Specify some files and try again.");
                return;
            }
            Console.WriteLine(string.Format("Found {0} file(s).", files.Count));

            // Parse files
            Console.Write("Parsing Netpbm files... ");
            Parser parser = new Parser();
            Dictionary<string, RawImage> rawImages = new Dictionary<string, RawImage>();
            foreach (string file in files)
            {
                rawImages.Add(Path.GetFullPath(file), parser.Read(file));
            }
            Console.WriteLine("done.");

            // The option for generating 8 bit images
            if (switches.Contains("8") || switches.Contains("8bit") || switches.Contains("8-bit"))
            {
                Console.Write("Generating 8-bit images... ");
                _convertedImages.AddRange(Convert8Bit(rawImages));
                Console.WriteLine("done.");
            }

            // The option for generating 16 bit images
            if (switches.Contains("16") || switches.Contains("16bit") || switches.Contains("16-bit"))
            {
                Console.Write("Generating 16-bit images... ");
                _convertedImages.AddRange(Convert16Bit(rawImages));
                Console.WriteLine("done.");
            }

            // Get target dir
            string targetDir = switches.FirstOrDefault(s => s.StartsWith("target:") || s.StartsWith("target-dir:") || s.StartsWith("dir:"));
            if (targetDir != null)
            {
                targetDir = targetDir.Split(new char[] { ':' }, 2)[1];
            }

            // The option for saving as PNG
            if (switches.Contains("save:png") || switches.Contains("save-png") || switches.Contains("savepng"))
            {
                SavePng(_convertedImages, targetDir);
            }

            // The option for saving as jpg
            if (switches.Contains("save:jpg") || switches.Contains("save-jpg") || switches.Contains("savejpg"))
            {
                SaveJpg(_convertedImages, targetDir);
            }

            // The option for saving as bmp
            if (switches.Contains("save:bmp") || switches.Contains("save-bmp") || switches.Contains("savebmp"))
            {
                SaveBmp(_convertedImages, targetDir);
            }

            // The option for showing a ui
            if (switches.Contains("ui") || switches.Contains("show") || switches.Contains("showui") || switches.Contains("show-ui"))
            {
                Console.WriteLine("Starting UI...");
                Console.Write("Waiting for all UI windows to close... ");
                UI.App.Main();
                Console.WriteLine("UI closed.");
            }

            Console.WriteLine("Exiting...");
        }

        /// <summary>
        /// Gets the result of image conversion.
        /// </summary>
        public static List<Image> ConvertedImages
        {
            get
            {
                return _convertedImages;
            }
        }

        private static List<Image> Convert8Bit(Dictionary<string, RawImage> rawImages)
        {
            List<Image> images = new List<Image>();
            ImageConverter ic = new ImageConverter();
            foreach (KeyValuePair<string, RawImage> rawImage in rawImages)
            {
                string name = string.Format("{0}-8bit", Path.GetFileNameWithoutExtension(rawImage.Key));
                images.Add(new Image(name, Path.GetDirectoryName(rawImage.Key), ic.ConvertNetpbmTo8Bit(rawImage.Value)));
            }
            return images;
        }

        private static List<Image> Convert16Bit(Dictionary<string, RawImage> rawImages)
        {
            List<Image> images = new List<Image>();
            ImageConverter ic = new ImageConverter();
            foreach (KeyValuePair<string, RawImage> rawImage in rawImages)
            {
                string name = string.Format("{0}-16bit", Path.GetFileNameWithoutExtension(rawImage.Key));
                images.Add(new Image(name, Path.GetDirectoryName(rawImage.Key), ic.ConvertNetpbmTo8Bit(rawImage.Value)));
            }
            return images;
        }

        private static void SavePng(IEnumerable<Image> images, string directory)
        {
            if (directory != null) Directory.CreateDirectory(directory);
            foreach (Image image in images)
            {
                string dir = directory ?? image.Path;
                Utilities.SaveBitmapSourceAsPng(image.BitmapSource, Path.Combine(dir, image.Name + ".png"));
            }
        }

        private static void SaveJpg(IEnumerable<Image> images, string directory)
        {
            if (directory != null) Directory.CreateDirectory(directory);
            foreach (Image image in images)
            {
                string dir = directory ?? image.Path;
                Utilities.SaveBitmapSourceAsJpg(image.BitmapSource, Path.Combine(dir, image.Name + ".jpg"));
            }
        }

        private static void SaveBmp(IEnumerable<Image> images, string directory)
        {
            if (directory != null) Directory.CreateDirectory(directory);
            foreach (Image image in images)
            {
                string dir = directory ?? image.Path;
                Utilities.SaveBitmapSourceAsBmp(image.BitmapSource, Path.Combine(dir, image.Name + ".bmp"));
            }
        }
    }
}
