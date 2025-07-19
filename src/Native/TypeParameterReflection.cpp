#include "TypeParameterReflection.h"

Native::TypeParameterReflection::TypeParameterReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::TypeParameterReflection*)native;

	// Use lazy initialization - only initialize when accessed
	m_constraints = nullptr;
}

Native::TypeParameterReflection::~TypeParameterReflection()
{
    // Clean up the constraint types array
    if (m_constraints)
    {
        for (uint32_t index = 0; index < m_native->getConstraintCount(); index++)
        {
            delete m_constraints[index];
        }
        delete[] m_constraints;
        m_constraints = nullptr;
    }
}

slang::TypeParameterReflection* Native::TypeParameterReflection::getNative()
{
    return m_native;
}

char const* Native::TypeParameterReflection::getName()
{
    return m_native->getName();
}

unsigned Native::TypeParameterReflection::getIndex()
{
    return m_native->getIndex();
}

unsigned Native::TypeParameterReflection::getConstraintCount()
{
    return m_native->getConstraintCount();
}

Native::TypeReflection* Native::TypeParameterReflection::getConstraintByIndex(int index)
{
    if (!m_constraints)
    {
        unsigned int constraintCount = m_native->getConstraintCount();
        m_constraints = new TypeReflection * [constraintCount];
        for (unsigned int i = 0; i < constraintCount; i++)
        {
            slang::TypeReflection* nativeConstraint = m_native->getConstraintByIndex(i);
            if (nativeConstraint)
                m_constraints[i] = new TypeReflection(nativeConstraint);
            else
                m_constraints[i] = nullptr;
        }
    }
	return m_constraints[index];
}