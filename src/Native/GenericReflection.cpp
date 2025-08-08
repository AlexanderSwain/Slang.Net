#include "GenericReflection.h"
#include "VariableReflection.h"
#include "TypeReflection.h"
#include <stdexcept>

Native::GenericReflection::GenericReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::GenericReflection*)native;
}

Native::GenericReflection::~GenericReflection()
{
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
    slang::VariableReflection* nativeTypeParameter = m_native->getTypeParameter(index);
    return nativeTypeParameter ? new Native::VariableReflection(nativeTypeParameter) : nullptr;
}

unsigned int Native::GenericReflection::getValueParameterCount()
{
    return m_native->getValueParameterCount();
}

Native::VariableReflection* Native::GenericReflection::getValueParameter(unsigned index)
{
    slang::VariableReflection* nativeValueParameter = m_native->getValueParameter(index);
    return nativeValueParameter ? new Native::VariableReflection(nativeValueParameter) : nullptr;
}

unsigned int Native::GenericReflection::getTypeParameterConstraintCount(VariableReflection* typeParam)
{
    return m_native->getTypeParameterConstraintCount((slang::VariableReflection*)typeParam->getNative());
}

Native::TypeReflection* Native::GenericReflection::getTypeParameterConstraintType(VariableReflection* typeParam, unsigned index)
{
    slang::TypeReflection* nativeResult = m_native->getTypeParameterConstraintType((slang::VariableReflection*)typeParam->getNative(), index);
    return nativeResult ? new TypeReflection(nativeResult) : nullptr;
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
    slang::GenericReflection* outerContainerPtr = m_native->getOuterGenericContainer();
    return outerContainerPtr ? new GenericReflection(outerContainerPtr) : nullptr;
}

Native::TypeReflection* Native::GenericReflection::getConcreteType(VariableReflection* typeParam)
{
    slang::TypeReflection* nativeResult = m_native->getConcreteType((slang::VariableReflection*)typeParam->getNative());
    return nativeResult ? new TypeReflection(nativeResult) : nullptr;
}

int64_t Native::GenericReflection::getConcreteIntVal(VariableReflection* valueParam)
{
	return m_native->getConcreteIntVal((slang::VariableReflection*)valueParam->getNative());
}

Native::GenericReflection* Native::GenericReflection::applySpecializations(GenericReflection* genRef)
{
    slang::GenericReflection* nativeResult = m_native->applySpecializations((slang::GenericReflection*)genRef->getNative());
    return nativeResult ? new Native::GenericReflection(nativeResult) : nullptr;
}