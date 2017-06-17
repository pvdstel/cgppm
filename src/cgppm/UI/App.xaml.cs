using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace cgppm.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Launcher.ConvertedImages.Count == 0)
            {
                App.Current.Shutdown();
            }

            ImageConverter ic = new ImageConverter();
            foreach (Image image in Launcher.ConvertedImages)
            {
                ImageViewer iv = new ImageViewer();
                iv.SetImage(image);
                iv.Show();
            }
        }
    }
}
