using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cgppm.UI
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        private Image _image;

        public ImageViewer()
        {
            InitializeComponent();
        }

        public void SetImage(Image image)
        {
            _image = image;
            DataContext = _image;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void savePngButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG image|.png";
            sfd.InitialDirectory = _image.Path;
            if (sfd.ShowDialog() == true)
            {
                _image.BitmapSource.SaveBitmapSourceAsPng(sfd.FileName);
            }
        }

        private void saveJpgButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPG image|.jpg";
            sfd.InitialDirectory = _image.Path;
            if (sfd.ShowDialog() == true)
            {
                _image.BitmapSource.SaveBitmapSourceAsJpg(sfd.FileName);
            }
        }

        private void saveBmpButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP image|.bmp";
            sfd.InitialDirectory = _image.Path;
            if (sfd.ShowDialog() == true)
            {
                _image.BitmapSource.SaveBitmapSourceAsBmp(sfd.FileName);
            }

        }
    }
}
