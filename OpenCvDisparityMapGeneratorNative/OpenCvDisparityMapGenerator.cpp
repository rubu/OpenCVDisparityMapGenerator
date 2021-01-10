#include "OpenCvDisparityMapGenerator.h"

#include <msclr\marshal_cppstd.h>
#include <opencv2/calib3d.hpp>
#include <opencv2/opencv.hpp>
#include <memory>

namespace Native
{

class OpenCvDisparityMapGeneratorImpl
{
public:
	OpenCvDisparityMapGeneratorImpl(cv::Ptr<cv::StereoMatcher> stereo_matcher) : stereo_matcher_(std::move(stereo_matcher))
	{
	}

	void ComputeDisparityMap()
	{
		cv::Mat result;
		stereo_matcher_->compute(left_image_data_, right_image_data_, result);
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
			image_data = cv::imread(new_image);
			existing_image = new_image;
		}
	}

private:
	std::string left_image_;
	cv::Mat left_image_data_;
	std::string right_image_;
	cv::Mat right_image_data_;
	cv::Ptr<cv::StereoMatcher> stereo_matcher_;
};

OpenCvDisparityMapGenerator::OpenCvDisparityMapGenerator(OpenCvDisparityMapGeneratorType type)
{
	switch (type)
	{
	case OpenCvDisparityMapGeneratorType::StereoBM:
		impl_ = new OpenCvDisparityMapGeneratorImpl(cv::StereoBM::create());
		break;
	case OpenCvDisparityMapGeneratorType::StereoSGBM:
		impl_ = new OpenCvDisparityMapGeneratorImpl(cv::StereoSGBM::create());
		break;
	}
}

OpenCvDisparityMapGenerator::!OpenCvDisparityMapGenerator()
{
	this->~OpenCvDisparityMapGenerator();
}

OpenCvDisparityMapGenerator::~OpenCvDisparityMapGenerator()
{
	delete impl_;
}

void OpenCvDisparityMapGenerator::ComputeDisparityMap()
{
	impl_->ComputeDisparityMap();
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