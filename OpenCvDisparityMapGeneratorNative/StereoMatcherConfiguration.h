#pragma once

#include "OpenCvDisparityMapGeneratorType.h"

namespace Native
{

public ref class StereoMatcherConfiguration
{
public:
    property OpenCvDisparityMapGeneratorType Type;
    property int MinDisparity;
    property int NumDisparities;
    property int BlockSize;
    property int SpeckleWindowSize;
    property int SpeckleRange;
    property int Disp12MaxDiff;
};
}