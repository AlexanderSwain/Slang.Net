#include "GenericReflection.h"
#include "VariableReflection.h"
#include "TypeReflection.h"
#include <stdexcept>

Native::GenericReflection::GenericReflection(void* native)
{
	m_native = (slang::GenericReflection*)native;
}

slang::GenericReflection* Native::GenericReflection::getNative()
{
    return m_native;
}

//Native::DeclReflection* Native::GenericReflection::asDecl()
//{
//    return (DeclReflection*)spReflectionGeneric_asDecl((SlangReflectionGeneric*)this);
//}

char const* Native::GenericReflection::getName() 
{
    return m_native->getName();
}

unsigned int Native::GenericReflection::getTypeParameterCount()
{
    return m_native->getTypeParameterCount();
}

Native::VariableReflection* Native::GenericReflection::getTypeParameter(unsigned index)
{
    return new Native::VariableReflection(m_native->getTypeParameter(index));
}

unsigned int Native::GenericReflection::getValueParameterCount()
{
    return m_native->getValueParameterCount();
}

Native::VariableReflection* Native::GenericReflection::getValueParameter(unsigned index)
{
    return new Native::VariableReflection(m_native->getValueParameter(index));
}

unsigned int Native::GenericReflection::getTypeParameterConstraintCount(VariableReflection* typeParam)
{
    return m_native->getTypeParameterConstraintCount((slang::VariableReflection*)typeParam->getNative());
}

Native::TypeReflection* Native::GenericReflection::getTypeParameterConstraintType(VariableReflection* typeParam, unsigned index)
{
	return new TypeReflection(m_native->getTypeParameterConstraintType((slang::VariableReflection*)typeParam->getNative(), index));
}

//Native::DeclReflection* Native::GenericReflection::getInnerDecl()
//{
//    return (DeclReflection*)spReflectionGeneric_GetInnerDecl((SlangReflectionGeneric*)this);
//}

Native::DeclKind Native::GenericReflection::getInnerKind()
{
    return (Native::DeclKind)m_native->getInnerKind();
}

Native::GenericReflection* Native::GenericReflection::getOuterGenericContainer()
{
    return new Native::GenericReflection(m_native->getOuterGenericContainer());
}

Native::TypeReflection* Native::GenericReflection::getConcreteType(VariableReflection* typeParam)
{
	return new TypeReflection(m_native->getConcreteType((slang::VariableReflection*)typeParam->getNative()));
}

int64_t Native::GenericReflection::getConcreteIntVal(VariableReflection* valueParam)
{
	return m_native->getConcreteIntVal((slang::VariableReflection*)valueParam->getNative());
}

Native::GenericReflection* Native::GenericReflection::applySpecializations(GenericReflection* genRef)
{
	return new Native::GenericReflection(m_native->applySpecializations((slang::GenericReflection*)genRef->getNative()));
}