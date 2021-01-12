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
        private String leftImage_;
        private String rightImage_;
        private Native.OpenCvDisparityMapGenerator disparityMapGenerator_;
        private Native.StereoMatcherConfiguration stereoMatcherConfiguration_;
        private Native.OpenCvDisparityMapGeneratorType[] disparityMapGeneratorTypes_;

        public MainWindow()
        {
            InitializeComponent();
            disparityMapGeneratorTypes_ = (Native.OpenCvDisparityMapGeneratorType[])Enum.GetValues(typeof(Native.OpenCvDisparityMapGeneratorType));
            var disparityMapGeneratorTypeName = ApplicationSettings.Default.DisparityMapGeneratorType;
            DisparityMapGeneratorType.ItemsSource = disparityMapGeneratorTypes_;
            if (String.IsNullOrEmpty(disparityMapGeneratorTypeName) == false)
            {
                var selectedIndex = Array.FindIndex(disparityMapGeneratorTypes_, (type) => type.ToString() == disparityMapGeneratorTypeName);
                if (selectedIndex != -1)
                {
                    DisparityMapGeneratorType.SelectedIndex = selectedIndex;
                }
                else
                {
                    DisparityMapGeneratorType.SelectedIndex = 0;
                    ApplicationSettings.Default.DisparityMapGeneratorType = disparityMapGeneratorTypes_[0].ToString();
                    ApplicationSettings.Default.Save();
                }
            }
            disparityMapGenerator_ = (new Native.OpenCvDisparityMapGeneratorBuilder()).SetType(disparityMapGeneratorTypes_[DisparityMapGeneratorType.SelectedIndex]).Build();
            LoadNativeConfiguration();
            if (String.IsNullOrEmpty(ApplicationSettings.Default.LeftImage) == false)
            {
                try
                {
                    LoadImage(ref leftImage_, ApplicationSettings.Default.LeftImage, LeftImage, LeftImagePreview);
                    disparityMapGenerator_.SetLeftImage(leftImage_);
                }
                catch (Exception exception)
                {
                }
            }
            if (String.IsNullOrEmpty(ApplicationSettings.Default.RightImage) == false)
            {
                try
                {
                    LoadImage(ref rightImage_, ApplicationSettings.Default.RightImage, RightImage, RightImagePreview);
                    disparityMapGenerator_.SetRightImage(rightImage_);
                }
                catch (Exception exception)
                {
                }
            }
        }

        private void OnSelectLeftImageClicked(object sender, RoutedEventArgs e)
        {
            if (LoadImage(ref leftImage_, LeftImage, LeftImagePreview))
            {
                disparityMapGenerator_.SetLeftImage(leftImage_);
                ApplicationSettings.Default.LeftImage = leftImage_;
                ApplicationSettings.Default.Save();
            }
        }

        private void OnSelectRightImageClicked(object sender, RoutedEventArgs e)
        {
            if (LoadImage(ref rightImage_, RightImage, RightImagePreview))
            {
                disparityMapGenerator_.SetRightImage(rightImage_);
                ApplicationSettings.Default.RightImage = rightImage_;
                ApplicationSettings.Default.Save();
            }
        }

        private void LoadImage(ref string existingImage, string newImage, TextBox image, Image imageContainer)
        {
            var imageSource = new BitmapImage(new Uri(newImage));
            imageContainer.Source = imageSource;
            image.Text = newImage;
            existingImage = newImage;
        }

        private bool LoadImage(ref string existingImage, TextBox image, Image imageContainer)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var newImage = dialog.FileName;
                if (existingImage != newImage)
                {
                    try
                    {
                        LoadImage(ref existingImage, newImage, image, imageContainer);
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
            if (disparityMapGenerator_ != null)
            {
                try
                {
                    if (StereoMatcherConfiguration.IsDirty())
                    {
                        if (MessageBox.Show("Configuration has been changed since the last time it was loaded, do you want to apply the new settings?", "Configuration Changed", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            SetNativeConfiguration();
                        }
                    }
                    // Yes yes, we block the gui here, boo hoo
                    disparityMapGenerator_.ComputeDisparityMap();
                    var source = new BitmapImage();
                    MemoryStream ms = new MemoryStream();
                    byte[] byteArray = File.ReadAllBytes(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "result.png"));
                    ms.Write(byteArray, 0, byteArray.Length);
                    ms.Position = 0;
                    source.BeginInit();
                    source.StreamSource = ms;
                    source.EndInit();
                    DisparityMap.Source = source;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void SelectedAlgorithmChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StereoMatcherConfiguration.GuiConfiguration == null)
            {
                return;
            }
            if (disparityMapGeneratorTypes_[DisparityMapGeneratorType.SelectedIndex] != StereoMatcherConfiguration.GuiConfiguration.Type)
            {
                disparityMapGenerator_ = (new Native.OpenCvDisparityMapGeneratorBuilder()).SetType(disparityMapGeneratorTypes_[DisparityMapGeneratorType.SelectedIndex]).Build();
                LoadNativeConfiguration();
                ApplicationSettings.Default.DisparityMapGeneratorType = disparityMapGeneratorTypes_[DisparityMapGeneratorType.SelectedIndex].ToString();
                ApplicationSettings.Default.Save();
            }
        }

        private void LoadNativeConfiguration()
        {
            stereoMatcherConfiguration_ = disparityMapGenerator_.GetConfiguration();
            StereoMatcherConfiguration.NativeConfiguration = stereoMatcherConfiguration_;
            StereoMatcherConfiguration.GuiConfiguration = stereoMatcherConfiguration_.Clone();
            StereoMatcherConfiguration.DataContext = StereoMatcherConfiguration.GuiConfiguration;
        }

        private void SetNativeConfiguration()
        {
            try
            {
                disparityMapGenerator_.SetConfiguration(StereoMatcherConfiguration.GuiConfiguration);
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
