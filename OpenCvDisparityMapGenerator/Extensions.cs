using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCvDisparityMapGenerator
{
    public static class Extensions
    {
        public static Native.StereoMatcherConfiguration Clone(this Native.StereoMatcherConfiguration configuration)
        {
            Native.StereoMatcherConfiguration clone = null;
            if (configuration is Native.StereoBmConfiguration)
            {
                var stereoBmClone = new Native.StereoBmConfiguration();
                clone = stereoBmClone;
            }
            else if (configuration is Native.StereoSgbmConfiguration)
            {
                var stereoSgbmClone = new Native.StereoSgbmConfiguration();
                clone = stereoSgbmClone;
            }
            else if (configuration is Native.StereoMatcherConfiguration)
            {
                clone = new Native.StereoMatcherConfiguration();
            }
            if (clone != null)
            {
                clone.BlockSize = configuration.BlockSize;
                clone.Disp12MaxDiff = configuration.Disp12MaxDiff;
                clone.MinDisparity = configuration.MinDisparity;
                clone.NumDisparities = configuration.NumDisparities;
                clone.SpeckleRange = configuration.SpeckleRange;
                clone.SpeckleWindowSize = configuration.SpeckleWindowSize;
            }
            return clone;
        }
    }
}
