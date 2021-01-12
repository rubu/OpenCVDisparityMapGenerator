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

namespace OpenCvDisparityMapGenerator.Configuration
{
    /// <summary>
    /// Interaction logic for StereoMatcherConfiguration.xaml
    /// </summary>
    public partial class StereoMatcherConfiguration : UserControl
    {
        public bool Dirty { get; set; } = false;
        public Native.StereoMatcherConfiguration NativeConfiguration { get; set; }
        public Native.StereoMatcherConfiguration GuiConfiguration { get; set; }
        public StereoMatcherConfiguration()
        {
            InitializeComponent();
        }

        private void AnyValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var senderName = ((Control)sender).Name;
            if (String.IsNullOrEmpty(senderName))
            {
                throw new ApplicationException("value without a name binding has been changed in stereo matcher configuration");
            }
            if (GuiConfiguration != null && NativeConfiguration != null)
            {
                var nativeProperty = NativeConfiguration.GetType().GetProperty(senderName);
                var guiProperty = GuiConfiguration.GetType().GetProperty(senderName);
                if (nativeProperty == null || guiProperty == null)
                {
                    throw new ApplicationException("unkown value has been changed in stereo matcher configuration");
                }
                if (Dirty == false)
                {
                    var nativeValue = nativeProperty.GetValue(NativeConfiguration, null);
                    var guiValue = guiProperty.GetValue(GuiConfiguration, null);
                    if (nativeValue.Equals(guiValue) == false)
                    {
                        Dirty = true;
                    }
                }
            }
        }
    }
}
