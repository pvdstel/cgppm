using cgppm.Netpbm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cgppm
{
    public class Launcher
    {
        private static IEnumerable<string> switches;
        private static IEnumerable<string> files;

        [STAThread]
        public static void Main(string[] args)
        {
            switches = args.Where(s => s[0] == '-' || s[0] == '/').Select(s => s.Substring(1));
            files = args.Where(s => File.Exists(s));

            Parser parser = new Parser();
            IEnumerable<RawImage> rawImages = files.Select(f => parser.Read(f));
            IEnumerable<NormalizedImage> normalizedImages = rawImages.Select(ri => new NormalizedImage(ri));
            
            Console.WriteLine(string.Format("Successfully parsed {0} files.", normalizedImages.Count()));
        }
    }
}
