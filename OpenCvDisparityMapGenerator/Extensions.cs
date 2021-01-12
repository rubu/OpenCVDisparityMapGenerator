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

        public static bool IsEqual(this Native.StereoMatcherConfiguration configuration, Native.StereoMatcherConfiguration other)
        {
            if (configuration.Type != other.Type)
            {
                return false;
            }
            if (configuration.BlockSize != other.BlockSize ||
                configuration.Disp12MaxDiff != other.Disp12MaxDiff ||
                configuration.MinDisparity != other.MinDisparity ||
                configuration.NumDisparities != other.NumDisparities ||
                configuration.SpeckleRange != other.SpeckleRange ||
                configuration.SpeckleWindowSize != other.SpeckleWindowSize)
            {
                return false;
            }
            if (configuration is Native.StereoBmConfiguration)
            {
                var otherStereoBmConfiguration = other as Native.StereoBmConfiguration;
                 
            }
            else if (configuration is Native.StereoSgbmConfiguration)
            {
                var otherStereoSgbmConfiguration = other as Native.StereoSgbmConfiguration;
            }
            return true;
        }
    }
}
