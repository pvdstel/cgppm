using System;
using System.Collections.Generic;
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

namespace cgppm
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        private BitmapSource _bitmapSource;

        public ImageViewer()
        {
            InitializeComponent();
        }

        public void SetBitmapSource(BitmapSource bitmapSource)
        {
            _bitmapSource = bitmapSource;
            DataContext = _bitmapSource;
        }
    }
}
