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
        private Cursor _magnifyCanvasCursor = null;

        public ImageViewer()
        {
            InitializeComponent();
            _magnifyCanvasCursor = magnifyCanvas.Cursor;
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

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            if ((e.Key == Key.C || e.Key == Key.X) && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                Clipboard.SetImage(_image.BitmapSource);
                Clipboard.Flush();
            }
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(_image.BitmapSource);
            Clipboard.Flush();
        }

        private void positionMagnifier(Point position)
        {
            double startX = position.X - magnify.ActualWidth / 2,
                   startY = position.Y - magnify.ActualHeight / 2;

            Canvas.SetLeft(magnify, startX);
            Canvas.SetTop(magnify, startY);

            GeneralTransform toCanvas = magnifyCanvas.TransformToAncestor(this);
            Rect transformedViewport = toCanvas.TransformBounds(new Rect(startX, startY, magnify.ActualWidth, magnify.ActualHeight));
            magnifyBrush.Viewbox = transformedViewport;
        }

        private void magnifyCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            magnify.Visibility = Visibility.Visible;
            magnifyCanvas.CaptureMouse();
            positionMagnifier(e.GetPosition(magnifyCanvas));

            // Hide cursor
            magnifyCanvas.Cursor = Cursors.None;
        }

        private void magnifyCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            magnify.Visibility = Visibility.Hidden;
            magnifyCanvas.ReleaseMouseCapture();
            magnifyCanvas.Cursor = Cursors.Arrow;

            // Show cursor
            magnifyCanvas.Cursor = _magnifyCanvasCursor;
        }

        private void magnifyCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (magnify.Visibility == Visibility.Visible) positionMagnifier(e.GetPosition(magnifyCanvas));
        }
    }
}