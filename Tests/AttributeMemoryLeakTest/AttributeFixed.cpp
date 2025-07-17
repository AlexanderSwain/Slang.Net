#include "AttributeFixed.h"

Native::Attribute::Attribute(void* native)
{
	m_native = (slang::Attribute*)native;
	m_argumentType = nullptr;
}

// Add destructor to clean up the cached TypeReflection
Native::Attribute::~Attribute()
{
	if (m_argumentType)
	{
		delete m_argumentType;
		m_argumentType = nullptr;
	}
}

slang::Attribute* Native::Attribute::getNative()
{
	return m_native;
}

char const* Native::Attribute::getName()
{
	return m_native->getName();
}

uint32_t Native::Attribute::getArgumentCount()
{
	return m_native->getArgumentCount();
}

// OPTION 1: Fix the memory leak by adding destructor (but this still has the logic bug)
Native::TypeReflection* Native::Attribute::getArgumentType(uint32_t index)
{
	if (!m_argumentType)
		m_argumentType = new TypeReflection(m_native->getArgumentType(index));
	return m_argumentType;
}

// OPTION 2: Better fix - remove caching entirely and let caller manage lifetime
/*
Native::TypeReflection* Native::Attribute::getArgumentType(uint32_t index)
{
	// Let the caller manage the lifetime, just like other similar methods in the codebase
	// This also fixes the logic bug where only the first call was cached
	return new TypeReflection(m_native->getArgumentType(index));
}
*/

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