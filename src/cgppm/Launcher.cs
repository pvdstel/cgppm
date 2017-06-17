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

            Console.Write("Parsing arguments... ");
            List<string> switches = args.Where(s => s[0] == '-' || s[0] == '/').Select(s => s.Substring(1)).ToList();
            List<string> files = args.Where(s => File.Exists(s)).ToList();
            Console.WriteLine("done.");

            if (files.Count == 0)
            {
                Console.WriteLine("No files were found. Specify some files and try again.");
                return;
            }
            Console.WriteLine(string.Format("Found {0} file(s).", files.Count));

            Console.Write("Parsing Netpbm files... ");
            Parser parser = new Parser();
            List<RawImage> rawImages = files.Select(f => parser.Read(f)).ToList();
            Console.WriteLine("done.");

            if (switches.Contains("8") || switches.Contains("8bit") || switches.Contains("8-bit"))
            {
                Console.Write("Generating 8-bit images... ");
                _convertedImages.AddRange(Convert8Bit(rawImages));
                Console.WriteLine("done.");
            }
            if (switches.Contains("16") || switches.Contains("16bit") || switches.Contains("16-bit"))
            {
                Console.Write("Generating 16-bit images... ");
                _convertedImages.AddRange(Convert16Bit(rawImages));
                Console.WriteLine("done.");
            }

            if (switches.Contains("ui") || switches.Contains("show") || switches.Contains("showui") || switches.Contains("show-ui"))
            {
                Console.WriteLine("Starting UI...");
                Console.Write("Waiting for all UI windows to close... ");
                UI.App.Main();
                Console.WriteLine("UI closed.");
            }

            Console.WriteLine("Exiting...");
        }

        public static List<Image> ConvertedImages
        {
            get
            {
                return _convertedImages;
            }
        }

        private static List<Image> Convert8Bit(List<RawImage> rawImages)
        {
            List<Image> images = new List<Image>();
            ImageConverter ic = new ImageConverter();
            foreach (RawImage image in rawImages)
            {
                images.Add(new Image(ic.ConvertNetpbmTo8Bit(image), "8 bit image"));
            }
            return images;
        }

        private static List<Image> Convert16Bit(List<RawImage> rawImages)
        {
            List<Image> images = new List<Image>();
            ImageConverter ic = new ImageConverter();
            foreach (RawImage image in rawImages)
            {
                images.Add(new Image(ic.ConvertNetpbmTo16Bit(image), "16 bit image"));
            }
            return images;
        }
    }
}
