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
using System.IO;

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
        private Native.StereoMatcherConfiguration stereo_matcher_configuration_;

        public MainWindow()
        {
            InitializeComponent();
            var disparity_map_generator_types = Enum.GetValues(typeof(Native.OpenCvDisparityMapGeneratorType));
            DisparityMapGeneratorType.ItemsSource = disparity_map_generator_types;
            DisparityMapGeneratorType.SelectedIndex = 0;
            disparity_map_generator_ = (new Native.OpenCvDisparityMapGeneratorBuilder()).Build();
            LoadNativeConfiguration();
            if (String.IsNullOrEmpty(ApplicationSettings.Default.LeftImage) == false)
            {
                LoadImage(ref left_image_, ApplicationSettings.Default.LeftImage, LeftImage, LeftImagePreview);
                disparity_map_generator_.SetLeftImage(left_image_);
            }
            if (String.IsNullOrEmpty(ApplicationSettings.Default.RightImage) == false)
            {
                LoadImage(ref right_image_, ApplicationSettings.Default.RightImage, RightImage, RightImagePreview);
                disparity_map_generator_.SetRightImage(right_image_);
            }
        }

        private void OnSelectLeftImageClicked(object sender, RoutedEventArgs e)
        {
            if (LoadImage(ref left_image_, LeftImage, LeftImagePreview))
            {
                disparity_map_generator_.SetLeftImage(left_image_);
                ApplicationSettings.Default.LeftImage = left_image_;
                ApplicationSettings.Default.Save();
            }
        }

        private void OnSelectRightImageClicked(object sender, RoutedEventArgs e)
        {
            if (LoadImage(ref right_image_, RightImage, RightImagePreview))
            {
                disparity_map_generator_.SetRightImage(right_image_);
                ApplicationSettings.Default.RightImage = right_image_;
                ApplicationSettings.Default.Save();
            }
        }

        private void LoadImage(ref string existing_image, string new_image, TextBox image, Image image_container)
        {
            var image_source = new BitmapImage(new Uri(new_image));
            image_container.Source = image_source;
            image.Text = new_image;
            existing_image = new_image;
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
                        LoadImage(ref existing_image, new_image, image, image_container);
                        return true;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
            return false;
        }

        private void OnComputeDisparityMapClicked(object sender, RoutedEventArgs e)
        {
            if (disparity_map_generator_ != null)
            {
                try
                {
                    if (StereoMatcherConfiguration.Dirty)
                    {
                        if (MessageBox.Show("Configuration has been changed since the last time it was loaded, do you want to apply the new settings?", "Configuration Changed", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            SetNativeConfiguration();
                        }
                    }
                    DisparityMap.Source = null;
                    disparity_map_generator_.ComputeDisparityMap();
                    var source = new BitmapImage();
                    source.BeginInit();
                    source.CacheOption = BitmapCacheOption.None;
                    source.UriSource = new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "result.png"));
                    source.EndInit();
                    DisparityMap.Source = source;
                }
                catch (Exception exception)
                {
                }
            }
        }

        private void SelectedAlgorithmChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LoadNativeConfiguration()
        {
            stereo_matcher_configuration_ = disparity_map_generator_.GetConfiguration();
            StereoMatcherConfiguration.NativeConfiguration = stereo_matcher_configuration_;
            StereoMatcherConfiguration.GuiConfiguration = stereo_matcher_configuration_.Clone();
            StereoMatcherConfiguration.DataContext = StereoMatcherConfiguration.GuiConfiguration;
            StereoMatcherConfiguration.Dirty = false;
        }

        private void SetNativeConfiguration()
        {
            try
            {
                disparity_map_generator_.SetConfiguration(StereoMatcherConfiguration.GuiConfiguration);
                LoadNativeConfiguration();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void OnApplySettingsClicked(object sender, RoutedEventArgs e)
        {
            SetNativeConfiguration();
        }
    }
}
