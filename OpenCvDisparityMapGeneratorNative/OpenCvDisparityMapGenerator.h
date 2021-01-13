#pragma once

#include "OpenCvDisparityMapGeneratorType.h"
#include "StereoMatcherConfiguration.h"
#include "DisparityMapCalculationResult.h"

namespace Native
{

class OpenCvDisparityMapGeneratorImpl;

public ref class OpenCvDisparityMapGenerator
{
public:
	OpenCvDisparityMapGenerator(OpenCvDisparityMapGeneratorType type);
	~OpenCvDisparityMapGenerator();
	!OpenCvDisparityMapGenerator();

	DisparityMapCalculationResult ^ComputeDisparityMap();
	StereoMatcherConfiguration ^GetConfiguration();
	void SetConfiguration(StereoMatcherConfiguration ^configuration);
	void SetLeftImage(System::String ^left_image);
	void SetRightImage(System::String ^right_image);

private:
	void CreateImpl(OpenCvDisparityMapGeneratorType type);

private:
	System::String^ left_image_;
	System::String^ right_image_;
	OpenCvDisparityMapGeneratorType type_;
	OpenCvDisparityMapGeneratorImpl *impl_;
};

}