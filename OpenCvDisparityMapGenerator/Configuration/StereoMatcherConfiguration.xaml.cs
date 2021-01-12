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
        public bool IsDirty()
        {
            return NativeConfiguration != null &&
                GuiConfiguration != null &&
                NativeConfiguration.IsEqual(GuiConfiguration) == false;
        }
        public Native.StereoMatcherConfiguration NativeConfiguration { get; set; }
        public Native.StereoMatcherConfiguration GuiConfiguration { get; set; }
        public StereoMatcherConfiguration()
        {
            InitializeComponent();
        }

    }
}
