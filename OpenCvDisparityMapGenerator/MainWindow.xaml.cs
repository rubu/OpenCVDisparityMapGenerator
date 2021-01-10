using Microsoft.Win32;
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

namespace OpenCvDisparityMapGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String left_image_;
        private String right_image_;
        private Native.OpenCvDisparityMapGenerator disparity_map_generator_;

        public MainWindow()
        {
            InitializeComponent();
            var disparity_map_generator_types = Enum.GetValues(typeof(Native.OpenCvDisparityMapGeneratorType));
            DisparityMapGeneratorType.ItemsSource = disparity_map_generator_types;
            DisparityMapGeneratorType.SelectedIndex = 0;
            if (String.IsNullOrEmpty(ApplicationSettings.Default.LeftImage) == false)
            {
                LoadImage(ApplicationSettings.Default.LeftImage, LeftImage, LeftImagePreview);
            }
            if (String.IsNullOrEmpty(ApplicationSettings.Default.RightImage) == false)
            {
                LoadImage(ApplicationSettings.Default.RightImage, RightImage, RightImagePreview);
            }
        }

        private void OnSelectLeftImageClicked(object sender, RoutedEventArgs e)
        {
            if (LoadImage(ref left_image_, LeftImage, LeftImagePreview))
            {
                disparity_map_generator_.SetLeftImage(left_image_);
                ApplicationSettings.Default.LeftImage = left_image_;
            }
        }

        private void OnSelectRightImageClicked(object sender, RoutedEventArgs e)
        {
            if (LoadImage(ref right_image_, RightImage, RightImagePreview))
            {
                disparity_map_generator_.SetRightImage(right_image_);
                ApplicationSettings.Default.RightImage = right_image_;
            }
        }

        private void LoadImage(string new_image, TextBox image, Image image_container)
        {
            var image_source = new BitmapImage(new Uri(new_image));
            image_container.Source = image_source;
            image.Text = new_image;
        }

        private bool LoadImage(ref string existing_image, TextBox image, Image image_container)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var new_image = dialog.FileName;
                if (existing_image != new_image)
                {
                    try
                    {
                        LoadImage(new_image, image, image_container);
                        existing_image = new_image;
                        return true;
                    }
                    catch (Exception exception)
                    {

                    }
                }
            }
            return false;
        }

        private void OnComputeDisparityMapClicked(object sender, RoutedEventArgs e)
        {
            if (disparity_map_generator_ != null)
            {
                disparity_map_generator_.ComputeDisparityMap();
            }
        }
    }
}
