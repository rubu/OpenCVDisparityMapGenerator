#pragma once

#include "StereoMatcherConfiguration.h"

namespace Native
{

public ref class StereoBmConfiguration : public StereoMatcherConfiguration
{
public:
	StereoBmConfiguration()
	{
		Type = OpenCvDisparityMapGeneratorType::StereoBM;
	}
};

}