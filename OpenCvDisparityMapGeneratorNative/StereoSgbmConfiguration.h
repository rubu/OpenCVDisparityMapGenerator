#pragma once

#include "StereoMatcherConfiguration.h"

namespace Native
{

public ref class StereoSgbmConfiguration : public StereoMatcherConfiguration
{
public:
	StereoSgbmConfiguration()
	{
		Type = OpenCvDisparityMapGeneratorType::StereoSGBM;
	}
};

}