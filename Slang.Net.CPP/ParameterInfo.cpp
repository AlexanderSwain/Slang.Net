#include "ParameterInfo.h"

Slang::ParameterInfo::ParameterInfo(System::String^ name, unsigned int category, unsigned int bindingIndex, unsigned int bindingSpace)
	: m_name(name),
	m_category(category),
	m_bindingIndex(bindingIndex),
	m_bindingSpace(bindingSpace)
{
}