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
            List<string> switches = args.Where(s => s[0] == '-' || s[0] == '/').Select(s => s.Substring(1)).ToList();
            List<string> files = args.Where(s => File.Exists(s)).ToList();

            Parser parser = new Parser();
            List<RawImage> rawImages = files.Select(f => parser.Read(f)).ToList();

            if (switches.Contains("8") || switches.Contains("8bit") || switches.Contains("8-bit"))
            {
                _convertedImages.AddRange(Convert8Bit(rawImages));
            }

            if (switches.Contains("ui") || switches.Contains("show") || switches.Contains("showui") || switches.Contains("show-ui"))
            {
                UI.App.Main();
            }
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
    }
}
