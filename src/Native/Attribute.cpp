#include "Attribute.h"


Native::Attribute::Attribute(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::Attribute*)native;

	// Use lazy initialization - only initialize when accessed
	m_argumentTypes = nullptr;
}

Native::Attribute::~Attribute()
{
	// Clean up the argument types array
	if (m_argumentTypes)
	{
		for (uint32_t index = 0; index < m_native->getArgumentCount(); index++)
		{
			delete m_argumentTypes[index];
		}
		delete[] m_argumentTypes;
		m_argumentTypes = nullptr;
	}

	// No need to delete m_native here, as it is managed by Slang
	//if (m_native)
	//{
	//	delete m_native;
	//	m_native = nullptr;
	//}
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

Native::TypeReflection* Native::Attribute::getArgumentType(uint32_t index)
{
	if (!m_argumentTypes)
	{
		uint32_t argumentCount = m_native->getArgumentCount();
		m_argumentTypes = new TypeReflection*[argumentCount];
		for (uint32_t i = 0; i < argumentCount; i++)
		{
			slang::TypeReflection* nativeArgumentType = m_native->getArgumentType(i);
			if (nativeArgumentType)
				m_argumentTypes[i] = new TypeReflection(nativeArgumentType);
			else
				m_argumentTypes[i] = nullptr;
		}
	}
	return m_argumentTypes[index];
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