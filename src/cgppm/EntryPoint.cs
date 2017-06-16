using cgppm.Ppm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cgppm
{
    public class EntryPoint
    {
        private static IEnumerable<string> switches;
        private static IEnumerable<string> files;

        [STAThread]
        public static void Main(string[] args)
        {
            switches = args.Where(s => s[0] == '-' || s[0] == '/').Select(s => s.Substring(1));
            files = args.Where(s => File.Exists(s));

            PpmParser parser = new PpmParser();
            List<RawPpmImage> rawImages = files.Select(f => parser.Read(f)).ToList();

            Console.WriteLine(string.Format("Successfully parsed {0} files.", rawImages.Count));
        }
    }
}
