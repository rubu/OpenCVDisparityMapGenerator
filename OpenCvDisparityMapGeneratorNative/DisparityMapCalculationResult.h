#pragma once

namespace Native
{

public ref class DisparityMapCalculationResult
{
public:
	property array<System::Byte> ^ResultPng;
	property array<System::Byte> ^ResultNormalizedPng;
	property array<System::Byte>^ ResultFilteredPng;
	property array<System::Byte> ^ConfidenceMapPng;
};

}
