using cgppm.Netpbm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cgppm
{
    public class Launcher
    {

        [STAThread]
        public static void Main(string[] args)
        {
            List<string> switches = args.Where(s => s[0] == '-' || s[0] == '/').Select(s => s.Substring(1)).ToList();
            List<string> files = args.Where(s => File.Exists(s)).ToList();

            Parser parser = new Parser();
            List<RawImage> rawImages = files.Select(f => parser.Read(f)).ToList();

            ImageConverter ic = new ImageConverter();
            foreach (RawImage image in rawImages)
            {
                ImageViewer iv = new ImageViewer();
                iv.SetBitmapSource(ic.ConvertNetpbmTo8Bit(image));
                iv.Show();
            }

            App.Main();
        }
    }
}
