#include "FunctionReflection.h"
#include "TypeReflection.h"
#include "VariableReflection.h"
#include "Attribute.h"
#include "GenericReflection.h"
#include "Modifier.h"
#include <stdexcept>

Native::FunctionReflection::FunctionReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::FunctionReflection*)native;

	// Use lazy initialization - only initialize when accessed
	m_returnType = nullptr;
    m_parameters = nullptr;
	m_userAttributes = nullptr;
	m_genericContainer = nullptr;
	m_overloads = nullptr;
}

Native::FunctionReflection::~FunctionReflection()
{
    // Clean up the return type
    if (m_returnType)
    {
        delete m_returnType;
        m_returnType = nullptr;
    }

    // Clean up the parameters array
    if (m_parameters)
    {
        for (uint32_t index = 0; index < m_native->getParameterCount(); index++)
        {
            delete m_parameters[index];
        }
        delete[] m_parameters;
        m_parameters = nullptr;
    }

    // Clean up user attributes
    if (m_userAttributes)
    {
        for (uint32_t index = 0; index < m_native->getUserAttributeCount(); index++)
        {
            delete m_userAttributes[index];
        }
        delete[] m_userAttributes;
        m_userAttributes = nullptr;
    }

	// Clean up modifiers
    for (auto& pair : m_modifiers)
    {
        delete pair.second; // Delete the Modifier object
	}
	m_modifiers.clear(); // Clear the map

    // Clean up generic container
    if (m_genericContainer)
    {
        delete m_genericContainer;
        m_genericContainer = nullptr;
    }

    // Clean up apply specializations results
    for (auto& item : m_applySpecializationsResultsToDelete) {
        delete item;
    }
    m_applySpecializationsResultsToDelete.clear();

    // Clean up specialize with arg types results
    for (auto& pair : m_specializeWithArgTypesResultsToDelete) {
        delete pair;
    }
    m_specializeWithArgTypesResultsToDelete.clear();

    // Clean up overloads
    if (m_overloads)
    {
        for (uint32_t index = 0; index < m_native->getOverloadCount(); index++)
        {
            delete m_overloads[index];
        }
        delete[] m_overloads;
        m_overloads = nullptr;
    }

	// No need to delete m_native here, as it is managed by Slang
    //if (m_native)
    //{
    //    delete m_native;
    //    m_native = nullptr;
    //}
}

slang::FunctionReflection* Native::FunctionReflection::getNative()
{
    return m_native;
}

char const* Native::FunctionReflection::getName()
{
    return m_native->getName();
}

Native::TypeReflection* Native::FunctionReflection::getReturnType()
{
    if (!m_returnType)
    {
        slang::TypeReflection* returnTypePtr = m_native->getReturnType();
        if (returnTypePtr) 
            m_returnType = new Native::TypeReflection(returnTypePtr);
        else
            m_returnType = nullptr;
    }
    return m_returnType;
}

unsigned int Native::FunctionReflection::getParameterCount()
{
    return m_native->getParameterCount();
}

Native::VariableReflection* Native::FunctionReflection::getParameterByIndex(unsigned int index)
{
    if (!m_parameters)
    {
        uint32_t parameterCount = m_native->getParameterCount();
        m_parameters = new VariableReflection*[parameterCount];
        for (uint32_t i = 0; i < parameterCount; i++)
        {
            slang::VariableReflection* nativeParameter = m_native->getParameterByIndex(i);
            if (nativeParameter)
                m_parameters[i] = new Native::VariableReflection(nativeParameter);
            else
                m_parameters[i] = nullptr;
        }
    }
	return m_parameters[index];
}

unsigned int Native::FunctionReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}

Native::Attribute* Native::FunctionReflection::getUserAttributeByIndex(unsigned int index)
{
    if (!m_userAttributes)
    {
        uint32_t userAttributeCount = m_native->getUserAttributeCount();
        m_userAttributes = new Native::Attribute*[userAttributeCount];
        for (uint32_t i = 0; i < userAttributeCount; i++)
        {
            slang::Attribute* nativeUserAttribute = m_native->getUserAttributeByIndex(i);
            if (nativeUserAttribute)
                m_userAttributes[i] = new Native::Attribute(nativeUserAttribute);
            else
                m_userAttributes[i] = nullptr;
        }
    }
	return m_userAttributes[index];
}

Native::Attribute* Native::FunctionReflection::findAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways.
	// Decided to keep it for consistency with slang api.
    slang::Attribute* nativeAttribute = m_native->findAttributeByName(SessionCLI::GetGlobalSession(), name);
    if (nativeAttribute)
        return new Native::Attribute(nativeAttribute);
    return nullptr;
}

Native::Attribute* Native::FunctionReflection::findUserAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways
    // Decided to keep it for consistency with slang api.
    slang::Attribute* nativeAttribute = m_native->findUserAttributeByName(SessionCLI::GetGlobalSession(), name);
    if (nativeAttribute)
        return new Native::Attribute(nativeAttribute);
    return nullptr;
}

Native::Modifier* Native::FunctionReflection::findModifier(Modifier::ID id)
{
	// Check if the modifier is already cached
    auto it = m_modifiers.find(id);

	// If the modifier is already cached, return it
    if (it != m_modifiers.end()) 
		return it->second;

	// If not cached, create a new Modifier and cache it
    slang::Modifier* nativeModifier = m_native->findModifier((slang::Modifier::ID)(int)id);
    if (nativeModifier)
    {
        Native::Modifier* result = new Native::Modifier(nativeModifier);
        m_modifiers[id] = result;
        return result;
    }
    return nullptr;
}

Native::GenericReflection* Native::FunctionReflection::getGenericContainer()
{
    if (!m_genericContainer)
    {
        slang::GenericReflection* containerPtr = m_native->getGenericContainer();
        if (containerPtr) 
            m_genericContainer = new GenericReflection(containerPtr);
        else
            m_genericContainer = nullptr;
    }
    return m_genericContainer;
}

Native::FunctionReflection* Native::FunctionReflection::applySpecializations(GenericReflection* genRef)
{
    slang::FunctionReflection* nativeResult = m_native->applySpecializations((slang::GenericReflection*)genRef->getNative());
    if (nativeResult)
    {
        Native::FunctionReflection* result = new Native::FunctionReflection(nativeResult);
        m_applySpecializationsResultsToDelete.push_back(result);
        return result;
    }
    return nullptr;
}

Native::FunctionReflection* Native::FunctionReflection::specializeWithArgTypes(unsigned int argCount, TypeReflection* const* types)
{
    slang::TypeReflection** slangTypes = new slang::TypeReflection * [argCount];
    for (unsigned int i = 0; i < argCount; ++i) {
        slangTypes[i] = (slang::TypeReflection*)types[i]->getNative();
    }

	slang::FunctionReflection* specialized = m_native->specializeWithArgTypes(argCount, slangTypes);
	delete[] slangTypes; // Clean up the allocated array
    
    if (specialized)
    {
        Native::FunctionReflection* result = new Native::FunctionReflection(specialized);
        m_specializeWithArgTypesResultsToDelete.push_back(result);
        return result;
    }
    return nullptr;
}

bool Native::FunctionReflection::isOverloaded()
{
    return m_native->isOverloaded();
}

unsigned int Native::FunctionReflection::getOverloadCount()
{
    return m_native->getOverloadCount();
}

Native::FunctionReflection* Native::FunctionReflection::getOverload(unsigned int index)
{
    if (!m_overloads)
    {
        uint32_t overloadCount = m_native->getOverloadCount();
        m_overloads = new Native::FunctionReflection*[overloadCount];
        for (uint32_t i = 0; i < overloadCount; i++)
        {
            slang::FunctionReflection* nativeOverload = m_native->getOverload(i);
            if (nativeOverload)
                m_overloads[i] = new Native::FunctionReflection(nativeOverload);
            else
                m_overloads[i] = nullptr;
        }
    }
	return m_overloads[index];
}