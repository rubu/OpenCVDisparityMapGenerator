#pragma once

#include "OpenCvDisparityMapGeneratorType.h"

namespace Native
{

class OpenCvDisparityMapGeneratorImpl;

public ref class OpenCvDisparityMapGenerator
{
public:
	OpenCvDisparityMapGenerator(OpenCvDisparityMapGeneratorType type);
	~OpenCvDisparityMapGenerator();
	!OpenCvDisparityMapGenerator();

	void ComputeDisparityMap();
	void SetLeftImage(System::String ^left_image);
	void SetRightImage(System::String ^right_image);

private:
	System::String^ left_image_;
	System::String^ right_image_;
	OpenCvDisparityMapGeneratorImpl* impl_;
};

}