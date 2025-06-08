#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "Attribute.h"


Native::Attribute::Attribute(void* native)
{
	m_native = (slang::Attribute*)native;
}

char const* Native::Attribute::getName()
{
	return m_native->getName();
}
uint32_t Native::Attribute::getArgumentCount()
{
	return m_native->getArgumentCount();
}
Native::TypeReflection* Native::Attribute::getArgumentType(uint32_t index)
{
	if (!m_argumentType)
		m_argumentType = new TypeReflection(m_native->getArgumentType(index));
	return m_argumentType;
}
SlangResult Native::Attribute::getArgumentValueInt(uint32_t index, int* value)
{
	return m_native->getArgumentValueInt(index, value);
}
SlangResult Native::Attribute::getArgumentValueFloat(uint32_t index, float* value)
{
	return m_native->getArgumentValueFloat(index, value);
}
const char* Native::Attribute::getArgumentValueString(uint32_t index, size_t* outSize)
{
	return m_native->getArgumentValueString(index, outSize);
}