#include "ParameterInfoCLI.h"

Native::ParameterInfoCLI::ParameterInfoCLI()
	: m_Name(nullptr), m_Category(0), m_BindingIndex(0), m_BindingSpace(0) {
}

Native::ParameterInfoCLI::ParameterInfoCLI(const char* name, unsigned int category, unsigned int bindingIndex, unsigned int bindingSpace)
	: m_Name(name), m_Category(category), m_BindingIndex(bindingIndex), m_BindingSpace(bindingSpace) {
}

const char* Native::ParameterInfoCLI::getName()
{
	return m_Name;
}

unsigned int Native::ParameterInfoCLI::getCategory()
{
	return m_Category;
}

unsigned int Native::ParameterInfoCLI::getBindingIndex()
{
	return m_BindingIndex;
}

unsigned int Native::ParameterInfoCLI::getBindingSpace()
{
	return m_BindingSpace;
}