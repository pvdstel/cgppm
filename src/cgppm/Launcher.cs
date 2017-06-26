using cgppm.Netpbm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cgppm
{
    public class Launcher
    {
        private static List<string> _switches;
        private static List<string> _files;
        private static List<Image> _convertedImages = new List<Image>();

        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting cgppm...");

            // Gather input data
            Console.Write("Parsing arguments... ");
            _switches = args.Where(s => s[0] == '-' || s[0] == '/').Select(s => s.Substring(1).ToLower()).ToList();
            _files = args.Where(s => File.Exists(s)).ToList();
            Console.WriteLine("done.");

            if ((_files.Count == 0 && _switches.Count == 0) || _switches.Contains("?") || _switches.Contains("h") || _switches.Contains("help"))
            {
                Console.WriteLine();
                Console.WriteLine(Properties.Resources.Help);
                return;
            }

            // Files count check
            if (_files.Count == 0)
            {
                Console.WriteLine("No files were found. Specify some files and try again.");
                return;
            }
            Console.WriteLine(string.Format("Found {0} file(s).", _files.Count));

            Dictionary<string, RawImage> rawImages = ParseFiles();
            ConvertImages(rawImages);

            string targetDir = GetTargetDirectory();
            SaveImages(targetDir);
            DeleteSourceFiles();
            ShowUI();

            Console.WriteLine("Exiting cgppm...");
        }

        #region Options and handling

        private static Dictionary<string, RawImage> ParseFiles()
        {
            // Parse files
            Console.Write("Parsing Netpbm files... ");
            Parser parser = new Parser();
            Dictionary<string, RawImage> rawImages = new Dictionary<string, RawImage>();
            foreach (string file in _files)
            {
                rawImages.Add(Path.GetFullPath(file), parser.Read(file));
            }
            Console.WriteLine("done.");
            return rawImages;
        }

        private static void ConvertImages(Dictionary<string, RawImage> rawImages)
        {
            // The option for generating 8 bit images
            if (_switches.Contains("8") || _switches.Contains("8bit") || _switches.Contains("8-bit"))
            {
                Console.Write("Generating 8-bit images... ");
                _convertedImages.AddRange(Convert8Bit(rawImages));
                Console.WriteLine("done.");
            }

            // The option for generating 16 bit images
            if (_switches.Contains("16") || _switches.Contains("16bit") || _switches.Contains("16-bit"))
            {
                Console.Write("Generating 16-bit images... ");
                _convertedImages.AddRange(Convert16Bit(rawImages));
                Console.WriteLine("done.");
            }
        }

        private static string GetTargetDirectory()
        {
            // Get target dir
            string targetDir = _switches.FirstOrDefault(s => s.StartsWith("target:") || s.StartsWith("target-dir:") || s.StartsWith("dir:"));
            if (targetDir != null)
            {
                targetDir = targetDir.Split(new char[] { ':' }, 2)[1];
            }
            return targetDir;
        }

        private static void SaveImages(string targetDir)
        {
            // The option for saving as PNG
            if (_switches.Contains("save:png") || _switches.Contains("save-png") || _switches.Contains("savepng"))
            {
                Console.Write("Saving as PNG... ");
                SavePng(_convertedImages, targetDir);
                Console.WriteLine("done.");
            }

            // The option for saving as jpg
            if (_switches.Contains("save:jpg") || _switches.Contains("save-jpg") || _switches.Contains("savejpg") ||
                _switches.Contains("save:jpeg") || _switches.Contains("save-jpeg") || _switches.Contains("savejpeg"))
            {
                Console.Write("Saving as JPG... ");
                SaveJpg(_convertedImages, targetDir);
                Console.WriteLine("done.");
            }

            // The option for saving as bmp
            if (_switches.Contains("save:bmp") || _switches.Contains("save-bmp") || _switches.Contains("savebmp"))
            {
                Console.Write("Saving as BMP... ");
                SaveBmp(_convertedImages, targetDir);
                Console.WriteLine("done.");
            }
        }

        private static void DeleteSourceFiles()
        {
            if (_switches.Contains("delete-source") || _switches.Contains("delete-source-files") || _switches.Contains("deletesource"))
            {
                Console.Write("Deleting source files... ");
                foreach (string sourceFile in _files)
                {
                    File.Delete(sourceFile);
                }
                Console.WriteLine("done.");
            }
        }

        private static void ShowUI()
        {
            // The option for showing a ui
            if (_switches.Contains("ui") || _switches.Contains("show") || _switches.Contains("showui") || _switches.Contains("show-ui"))
            {
                Console.WriteLine("Starting UI...");
                Console.Write("Waiting for all UI windows to close... ");
                UI.App.Main();
                Console.WriteLine("UI closed.");
            }
        }
        #endregion

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

        #region Converting

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
        #endregion

        #region Saving images

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
        #endregion
    }
}
