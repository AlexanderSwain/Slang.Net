#include "TypeParameterReflection.h"

Native::TypeParameterReflection::TypeParameterReflection(void* native)
{
	m_native = (slang::TypeParameterReflection*)native;
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
    return new TypeReflection(m_native->getConstraintByIndex(index));
}