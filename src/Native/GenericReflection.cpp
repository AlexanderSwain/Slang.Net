#include "GenericReflection.h"
#include "VariableReflection.h"
#include "TypeReflection.h"
#include <stdexcept>

Native::GenericReflection::GenericReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::GenericReflection*)native;

    // Use lazy initialization - only initialize when accessed
    m_typeParameters = nullptr;
	m_valueParameters = nullptr;
	m_outerGenericContainer = nullptr;
}

Native::GenericReflection::~GenericReflection()
{
    // Clean up type parameters
    if (m_typeParameters)
    {
        for (uint32_t index = 0; index < m_native->getTypeParameterCount(); index++)
        {
            delete m_typeParameters[index];
        }
        delete[] m_typeParameters;
        m_typeParameters = nullptr;
    }

    // Clean up value parameters
    if (m_valueParameters)
    {
        for (uint32_t index = 0; index < m_native->getValueParameterCount(); index++)
        {
            delete m_valueParameters[index];
        }
        delete[] m_valueParameters;
        m_valueParameters = nullptr;
    }

    // Clean up type parameter constraint results
    for (auto& result : m_typeParameterConstraintsResultsToDelete) {
        delete result;
    }
    m_typeParameterConstraintsResultsToDelete.clear();

    // Clean up outer generic container
    if (m_outerGenericContainer)
        delete m_outerGenericContainer;

    // Clean up concrete type results results
    for (auto& result : m_concreteTypeResultsToDelete) {
        delete result;
    }
    m_concreteTypeResultsToDelete.clear();

    // Clean up apply specializations results results
    for (auto& result : m_applySpecializationsResultsToDelete) {
        delete result;
    }
    m_applySpecializationsResultsToDelete.clear();

    // No need to delete m_native here, as it is managed by Slang
    //if (m_native)
    //{
    //	delete m_native;
    //	m_native = nullptr;
    //}
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
    if (!m_typeParameters)
    {
        uint32_t typeParameterCount = m_native->getTypeParameterCount();
        m_typeParameters = new VariableReflection*[typeParameterCount];
        for (uint32_t i = 0; i < typeParameterCount; i++)
        {
            slang::VariableReflection* nativeTypeParameter = m_native->getTypeParameter(i);
            if (nativeTypeParameter)
                m_typeParameters[i] = new Native::VariableReflection(nativeTypeParameter);
            else
                m_typeParameters[i] = nullptr;
        }
    }
    return m_typeParameters[index];
}

unsigned int Native::GenericReflection::getValueParameterCount()
{
    return m_native->getValueParameterCount();
}

Native::VariableReflection* Native::GenericReflection::getValueParameter(unsigned index)
{
    if (!m_valueParameters)
    {
        uint32_t valueParameterCount = m_native->getValueParameterCount();
        m_valueParameters = new VariableReflection*[valueParameterCount];
        for (uint32_t i = 0; i < valueParameterCount; i++)
        {
            slang::VariableReflection* nativeValueParameter = m_native->getValueParameter(i);
            if (nativeValueParameter)
                m_valueParameters[i] = new Native::VariableReflection(nativeValueParameter);
            else
                m_valueParameters[i] = nullptr;
        }
    }
	return m_valueParameters[index];
}

unsigned int Native::GenericReflection::getTypeParameterConstraintCount(VariableReflection* typeParam)
{
    return m_native->getTypeParameterConstraintCount((slang::VariableReflection*)typeParam->getNative());
}

Native::TypeReflection* Native::GenericReflection::getTypeParameterConstraintType(VariableReflection* typeParam, unsigned index)
{
    slang::TypeReflection* nativeResult = m_native->getTypeParameterConstraintType((slang::VariableReflection*)typeParam->getNative(), index);
    if (nativeResult)
    {
        Native::TypeReflection* result = new TypeReflection(nativeResult);
        m_typeParameterConstraintsResultsToDelete.push_back(result);
        return result;
    }
    return nullptr;
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
    if (!m_outerGenericContainer)
    {
        slang::GenericReflection* outerContainerPtr = m_native->getOuterGenericContainer();
        if (outerContainerPtr) 
            m_outerGenericContainer = new GenericReflection(outerContainerPtr);
        else
            m_outerGenericContainer = nullptr;
    }
    return m_outerGenericContainer;
}

Native::TypeReflection* Native::GenericReflection::getConcreteType(VariableReflection* typeParam)
{
    slang::TypeReflection* nativeResult = m_native->getConcreteType((slang::VariableReflection*)typeParam->getNative());
    if (nativeResult)
    {
        Native::TypeReflection* result = new TypeReflection(nativeResult);
        m_concreteTypeResultsToDelete.push_back(result);
        return result;
    }
    return nullptr;
}

int64_t Native::GenericReflection::getConcreteIntVal(VariableReflection* valueParam)
{
	return m_native->getConcreteIntVal((slang::VariableReflection*)valueParam->getNative());
}

Native::GenericReflection* Native::GenericReflection::applySpecializations(GenericReflection* genRef)
{
    slang::GenericReflection* nativeResult = m_native->applySpecializations((slang::GenericReflection*)genRef->getNative());
    if (nativeResult)
    {
        Native::GenericReflection* result = new Native::GenericReflection(nativeResult);
        m_applySpecializationsResultsToDelete.push_back(result);
        return result;
    }
    return nullptr;
}