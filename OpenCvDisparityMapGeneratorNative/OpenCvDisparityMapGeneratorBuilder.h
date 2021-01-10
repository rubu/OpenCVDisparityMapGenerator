#pragma once

#include "OpenCvDisparityMapGeneratorType.h"
#include "OpenCvDisparityMapGenerator.h"

namespace Native
{

public ref class OpenCvDisparityMapGeneratorBuilder
{
public:
	OpenCvDisparityMapGeneratorBuilder()
	{
	}

	OpenCvDisparityMapGenerator ^Build()
	{
		switch (type_)
		{
		case OpenCvDisparityMapGeneratorType::StereoBM:
		case OpenCvDisparityMapGeneratorType::StereoSGBM:
			return gcnew OpenCvDisparityMapGenerator(type_);
		default:
			throw gcnew System::Exception("invalid OpenCV disparity generator type");
		}
	}

	void SetUsePreProcesssing(bool use_pre_processing)
	{
		use_pre_processing_ = use_pre_processing;
	}
	void SetType(OpenCvDisparityMapGeneratorType type)
	{
		type_ = type;
	}
	bool UsePreProcessing()
	{
		return use_pre_processing_;
	}
	OpenCvDisparityMapGeneratorType Type()
	{
		return type_;
	}

private:
	bool use_pre_processing_ = false;
	OpenCvDisparityMapGeneratorType type_ = OpenCvDisparityMapGeneratorType::StereoBM;
};

}