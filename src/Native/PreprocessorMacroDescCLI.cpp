#include "PreprocessorMacroDescCLI.h"

Native::PreprocessorMacroDescCLI::PreprocessorMacroDescCLI()
	: m_Name(nullptr)
	, m_Value(nullptr)
{
}

Native::PreprocessorMacroDescCLI::PreprocessorMacroDescCLI(const char* name, const char* value)
	: m_Name(name)
	, m_Value(value)
{
}

const char* Native::PreprocessorMacroDescCLI::getName()
{
	return m_Name;
}

const char* Native::PreprocessorMacroDescCLI::getValue()
{
	return m_Value;
}