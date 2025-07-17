#include "GenericReflection.h"
#include "VariableReflection.h"
#include "TypeReflection.h"
#include <stdexcept>

Native::GenericReflection::GenericReflection(void* native)
{
	m_native = (slang::GenericReflection*)native;

    // Initialize the argument types array
    uint32_t typeParameterCount = m_native->getTypeParameterCount();
    m_typeParameters = new VariableReflection*[typeParameterCount];
    for (uint32_t index = 0; index < typeParameterCount; index++)
    {
        m_typeParameters[index] = new Native::VariableReflection(m_native->getTypeParameter(index));
    }

	// Initialize the value parameters array
    uint32_t valueParameterCount = m_native->getValueParameterCount();
    m_valueParameters = new VariableReflection*[valueParameterCount];
    for (uint32_t index = 0; index < valueParameterCount; index++)
    {
        m_valueParameters[index] = new Native::VariableReflection(m_native->getValueParameter(index));
	}

	// Initialize outer generic container
    GenericReflection* outerContainer = new Native::GenericReflection(m_native->getOuterGenericContainer());
}

Native::GenericReflection::~GenericReflection()
{
    // Clean up type parameters
    for (uint32_t index = 0; index < m_native->getTypeParameterCount(); index++)
    {
        delete m_typeParameters[index];
    }
    delete[] m_typeParameters;
    m_typeParameters = nullptr;

    // Clean up value parameters
    for (uint32_t index = 0; index < m_native->getValueParameterCount(); index++)
    {
        delete m_valueParameters[index];
    }
    delete[] m_valueParameters;
    m_valueParameters = nullptr;

    // Clean up type parameter constraint results
    for (auto& result : m_typeParameterConstraintsResultsToDelete) {
        delete result;
    }
    m_typeParameterConstraintsResultsToDelete.clear();

    // Clean up outer generic container
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
    return m_typeParameters[index];
}

unsigned int Native::GenericReflection::getValueParameterCount()
{
    return m_native->getValueParameterCount();
}

Native::VariableReflection* Native::GenericReflection::getValueParameter(unsigned index)
{
	return m_valueParameters[index];
}

unsigned int Native::GenericReflection::getTypeParameterConstraintCount(VariableReflection* typeParam)
{
    return m_native->getTypeParameterConstraintCount((slang::VariableReflection*)typeParam->getNative());
}

Native::TypeReflection* Native::GenericReflection::getTypeParameterConstraintType(VariableReflection* typeParam, unsigned index)
{
    Native::TypeReflection* result = new TypeReflection(m_native->getTypeParameterConstraintType((slang::VariableReflection*)typeParam->getNative(), index));
	m_typeParameterConstraintsResultsToDelete.push_back(result);
	return result;
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
    return m_outerGenericContainer;
}

Native::TypeReflection* Native::GenericReflection::getConcreteType(VariableReflection* typeParam)
{
    Native::TypeReflection* result = new TypeReflection(m_native->getConcreteType((slang::VariableReflection*)typeParam->getNative()));
	m_concreteTypeResultsToDelete.push_back(result);
	return result;
}

int64_t Native::GenericReflection::getConcreteIntVal(VariableReflection* valueParam)
{
	return m_native->getConcreteIntVal((slang::VariableReflection*)valueParam->getNative());
}

Native::GenericReflection* Native::GenericReflection::applySpecializations(GenericReflection* genRef)
{
    Native::GenericReflection* result = new Native::GenericReflection(m_native->applySpecializations((slang::GenericReflection*)genRef->getNative()));
    m_applySpecializationsResultsToDelete.push_back(result);
    return result;
}