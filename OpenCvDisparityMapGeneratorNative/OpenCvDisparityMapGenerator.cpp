#include "OpenCvDisparityMapGenerator.h"
#include "StereoBmConfiguration.h"
#include "StereoSgbmConfiguration.h"

#include <msclr\marshal_cppstd.h>
#include <opencv2/calib3d.hpp>
#include <opencv2/opencv.hpp>
#include <memory>
#include <sstream>

namespace Native
{

class OpenCvDisparityMapGeneratorImpl
{

public:
	OpenCvDisparityMapGeneratorImpl(OpenCvDisparityMapGeneratorType type, cv::Ptr<cv::StereoMatcher> stereo_matcher) : type_(type),
		stereo_matcher_(std::move(stereo_matcher))
	{
	}

	DisparityMapCalculationResult ^ComputeDisparityMap()
	{
		cv::Mat disparity_map, disparity_map_normalized;
		stereo_matcher_->compute(left_image_data_, right_image_data_, disparity_map);
		cv::normalize(disparity_map, disparity_map_normalized, 0, 255, cv::NORM_MINMAX, CV_8U);
		std::vector<uchar> disparity_map_png, disparity_map_normalized_png;
		cv::imencode(".png", disparity_map, disparity_map_png);
		cv::imencode(".png", disparity_map_normalized, disparity_map_normalized_png);
		DisparityMapCalculationResult ^result = gcnew DisparityMapCalculationResult;
		result->ResultPng = gcnew array<System::Byte>(disparity_map_png.size());
		cli::pin_ptr<unsigned char> result_png_managed = &result->ResultPng[result->ResultPng->GetLowerBound(0)];
		memcpy(result_png_managed, disparity_map_png.data(), disparity_map_png.size());
		result->ResultNormalizedPng = gcnew array<System::Byte>(disparity_map_normalized_png.size());
		cli::pin_ptr<unsigned char> result_normalized_png_managed = &result->ResultNormalizedPng[result->ResultNormalizedPng->GetLowerBound(0)];
		memcpy(result_normalized_png_managed, disparity_map_normalized_png.data(), disparity_map_normalized_png.size());
		return result;
	}

	StereoMatcherConfiguration ^GetConfiguration()
	{
		StereoMatcherConfiguration^ configuration = nullptr;
		switch (type_)
		{
		case OpenCvDisparityMapGeneratorType::StereoBM:
			{
				configuration = gcnew StereoBmConfiguration;
				configuration->Type = OpenCvDisparityMapGeneratorType::StereoBM;
			}
			break;
		case OpenCvDisparityMapGeneratorType::StereoSGBM:
			{
				configuration = gcnew StereoSgbmConfiguration;
				configuration->Type = OpenCvDisparityMapGeneratorType::StereoSGBM;
			}
			break;
		}
		if (configuration != nullptr)
		{
			configuration->MinDisparity = stereo_matcher_->getMinDisparity();
			configuration->NumDisparities = stereo_matcher_->getNumDisparities();
			configuration->BlockSize = stereo_matcher_->getBlockSize();
			configuration->SpeckleRange = stereo_matcher_->getSpeckleRange();
			configuration->SpeckleWindowSize = stereo_matcher_->getSpeckleWindowSize();
			configuration->Disp12MaxDiff = stereo_matcher_->getSpeckleWindowSize();
		}
		return configuration;
	}

	void SetConfiguration(StereoMatcherConfiguration ^configuration)
	{
		if (configuration->Type != type_)
		{
			std::stringstream error_stream;
			error_stream << "configuration type " << static_cast<uint32_t>(configuration->Type) << " does not match implementation type " << static_cast<uint32_t>(type_);
			throw std::exception(error_stream.str().c_str());
		}
		stereo_matcher_->setMinDisparity(configuration->MinDisparity);
		stereo_matcher_->setNumDisparities(configuration->NumDisparities);
		stereo_matcher_->setBlockSize(configuration->BlockSize);
		stereo_matcher_->setSpeckleRange(configuration->SpeckleRange);
		stereo_matcher_->setSpeckleWindowSize(configuration->SpeckleWindowSize);
		stereo_matcher_->setDisp12MaxDiff(configuration->Disp12MaxDiff);
	}

	void SetLeftImage(const std::string &left_image)
	{
		SetImage(left_image, left_image_, left_image_data_);
	}

	void SetRightImage(const std::string &right_image)
	{
		SetImage(right_image, right_image_, right_image_data_);
	}

private:
	void SetImage(const std::string &new_image, std::string &existing_image, cv::Mat &image_data)
	{
		if (new_image != existing_image)
		{
			auto image_data_original = cv::imread(new_image);
			cv::cvtColor(image_data_original, image_data, cv::COLOR_BGR2GRAY);
			existing_image = new_image;
		}
	}

private:
	std::string left_image_;
	cv::Mat left_image_data_;
	std::string right_image_;
	cv::Mat right_image_data_;
	const OpenCvDisparityMapGeneratorType type_;
	const cv::Ptr<cv::StereoMatcher> stereo_matcher_;
};

OpenCvDisparityMapGenerator::OpenCvDisparityMapGenerator(OpenCvDisparityMapGeneratorType type)
{
	CreateImpl(type);
}

OpenCvDisparityMapGenerator::!OpenCvDisparityMapGenerator()
{
	this->~OpenCvDisparityMapGenerator();
}

OpenCvDisparityMapGenerator::~OpenCvDisparityMapGenerator()
{
	delete impl_;
}

DisparityMapCalculationResult ^OpenCvDisparityMapGenerator::ComputeDisparityMap()
{
	try
	{
		return impl_->ComputeDisparityMap();
	}
	catch (const cv::Exception &exception)
	{
		throw gcnew System::Exception(gcnew System::String(exception.what()));
	}
}

void OpenCvDisparityMapGenerator::CreateImpl(OpenCvDisparityMapGeneratorType type)
{
	switch (type)
	{
	case OpenCvDisparityMapGeneratorType::StereoBM:
		impl_ = new OpenCvDisparityMapGeneratorImpl(OpenCvDisparityMapGeneratorType::StereoBM, cv::StereoBM::create());
		break;
	case OpenCvDisparityMapGeneratorType::StereoSGBM:
		impl_ = new OpenCvDisparityMapGeneratorImpl(OpenCvDisparityMapGeneratorType::StereoSGBM, cv::StereoSGBM::create());
		break;
	default:
		throw gcnew System::Exception(System::String::Format("unsupported OpenCV disparity map generator type {0}", gcnew System::UInt32(static_cast<unsigned int>(type))));
	}
	type_ = type;
}

StereoMatcherConfiguration ^OpenCvDisparityMapGenerator::GetConfiguration()
{
	return impl_->GetConfiguration();
}

void OpenCvDisparityMapGenerator::SetConfiguration(StereoMatcherConfiguration ^configuration)
{
	if (configuration->Type != type_)
	{
		CreateImpl(configuration->Type);
	}
	impl_->SetConfiguration(configuration);
}

void OpenCvDisparityMapGenerator::SetLeftImage(System::String^ left_image)
{
	impl_->SetLeftImage(msclr::interop::marshal_as<std::string>(left_image));
	left_image_ = left_image;
}

void OpenCvDisparityMapGenerator::SetRightImage(System::String^ right_image)
{
	impl_->SetRightImage(msclr::interop::marshal_as<std::string>(right_image));
	right_image_ = right_image_;
}

}