#include "TypeParameterReflection.h"

Native::TypeParameterReflection::TypeParameterReflection(void* native)
{
	m_native = (slang::TypeParameterReflection*)native;

	// Initialize the constraint types array
	unsigned int constraintCount = m_native->getConstraintCount();
	m_constraints = new TypeReflection * [constraintCount];
    for (unsigned int index = 0; index < constraintCount; index++)
    {
        m_constraints[index] = new TypeReflection(m_native->getConstraintByIndex(index));
	}
}

Native::TypeParameterReflection::~TypeParameterReflection()
{
    // Clean up the constraint types array
    for (uint32_t index = 0; index < m_native->getConstraintCount(); index++)
    {
        delete m_constraints[index];
    }
    delete[] m_constraints;
    m_constraints = nullptr;
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
	return m_constraints[index];
}