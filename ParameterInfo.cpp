#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "ParameterInfo.h"


Slang::ParameterInfo::ParameterInfo(unsigned int category, unsigned int bindingIndex, unsigned int bindingSpace) :
	m_category(category),
	m_bindingIndex(bindingIndex),
	m_bindingSpace(bindingSpace)
{
}

unsigned int Slang::ParameterInfo::getCategory()
{
	return m_category;
}

unsigned int Slang::ParameterInfo::getBindingIndex()
{
	return m_bindingIndex;
}

unsigned int Slang::ParameterInfo::getBindingSpace()
{
	return m_bindingSpace;
}