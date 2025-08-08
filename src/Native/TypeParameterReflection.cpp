#include "TypeParameterReflection.h"

Native::TypeParameterReflection::TypeParameterReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::TypeParameterReflection*)native;
}

Native::TypeParameterReflection::~TypeParameterReflection()
{
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
    slang::TypeReflection* nativeConstraint = m_native->getConstraintByIndex(index);
    return nativeConstraint ? new TypeReflection(nativeConstraint) : nullptr;
}