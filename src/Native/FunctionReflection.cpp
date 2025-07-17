#include "FunctionReflection.h"
#include "TypeReflection.h"
#include "VariableReflection.h"
#include "Attribute.h"
#include "GenericReflection.h"
#include "Modifier.h"
#include <stdexcept>

Native::FunctionReflection::FunctionReflection(void* native)
{
	m_native = (slang::FunctionReflection*)native;

	m_returnType = new Native::TypeReflection(m_native->getReturnType());

    // Initialize the argument types array
    uint32_t parameterCount = m_native->getParameterCount();
    m_parameters = new VariableReflection*[parameterCount];
    for (uint32_t index = 0; index < parameterCount; index++)
    {
        m_parameters[index] = new Native::VariableReflection(m_native->getParameterByIndex(index));
    }

	// Initialize user attributes
    uint32_t userAttributeCount = m_native->getUserAttributeCount();
    m_userAttributes = new Native::Attribute*[userAttributeCount];
    for (uint32_t index = 0; index < userAttributeCount; index++)
    {
        m_userAttributes[index] = new Native::Attribute(m_native->getUserAttributeByIndex(index));
	}

	// Initialize generic container
	m_genericContainer = new Native::GenericReflection(m_native->getGenericContainer());

	// Initialize overloads

    // Initialize the overloads array
    uint32_t overloadCount = m_native->getOverloadCount();
    m_overloads = new Native::FunctionReflection*[overloadCount];
    for (uint32_t index = 0; index < overloadCount; index++)
    {
        m_overloads[index] = new Native::FunctionReflection(m_native->getOverload(index));
    }
}

Native::FunctionReflection::~FunctionReflection()
{
    // Clean up the return type
    delete m_returnType;
    m_returnType = nullptr;

    // Clean up the parameters array
    for (uint32_t index = 0; index < m_native->getParameterCount(); index++)
    {
        delete m_parameters[index];
    }
    delete[] m_parameters;
    m_parameters = nullptr;

    // Clean up user attributes
    for (uint32_t index = 0; index < m_native->getUserAttributeCount(); index++)
    {
        delete m_userAttributes[index];
    }
    delete[] m_userAttributes;
    m_userAttributes = nullptr;

	// Clean up modifiers
    for (auto& pair : m_modifiers)
    {
        delete pair.second; // Delete the Modifier object
	}
	m_modifiers.clear(); // Clear the map


    // Clean up generic container
    delete m_genericContainer;
    m_genericContainer = nullptr;

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
    for (uint32_t index = 0; index < m_native->getOverloadCount(); index++)
    {
        delete m_overloads[index];
    }
    delete[] m_overloads;
    m_overloads = nullptr;

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
    return m_returnType;
}

unsigned int Native::FunctionReflection::getParameterCount()
{
    return m_native->getParameterCount();
}

Native::VariableReflection* Native::FunctionReflection::getParameterByIndex(unsigned int index)
{
	return m_parameters[index];
}

unsigned int Native::FunctionReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}
Native::Attribute* Native::FunctionReflection::getUserAttributeByIndex(unsigned int index)
{
	return m_userAttributes[index];
}
Native::Attribute* Native::FunctionReflection::findAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways.
	// Decided to keep it for consistency with slang api.
    return new Native::Attribute(m_native->findAttributeByName(SessionCLI::GetGlobalSession(), name));
}
Native::Attribute* Native::FunctionReflection::findUserAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways
    // Decided to keep it for consistency with slang api.
    return new Native::Attribute(m_native->findUserAttributeByName(SessionCLI::GetGlobalSession(), name));
}

Native::Modifier* Native::FunctionReflection::findModifier(Modifier::ID id)
{
	// Check if the modifier is already cached
    auto it = m_modifiers.find(id);

	// If the modifier is already cached, return it
    if (it != m_modifiers.end()) 
		return it->second;

	// If not cached, create a new Modifier and cache it
    Native::Modifier* result = new Native::Modifier(m_native->findModifier((slang::Modifier::ID)(int)id));
	m_modifiers[id] = result;
	return result;
}

Native::GenericReflection* Native::FunctionReflection::getGenericContainer()
{
    return m_genericContainer;
}

Native::FunctionReflection* Native::FunctionReflection::applySpecializations(GenericReflection* genRef)
{
    Native::FunctionReflection* result = new Native::FunctionReflection(m_native->applySpecializations((slang::GenericReflection*)genRef->getNative()));
    m_applySpecializationsResultsToDelete.push_back(result);
	return result;
}

Native::FunctionReflection* Native::FunctionReflection::specializeWithArgTypes(unsigned int argCount, TypeReflection* const* types)
{
    slang::TypeReflection** slangTypes = new slang::TypeReflection * [argCount];
    for (unsigned int i = 0; i < argCount; ++i) {
        slangTypes[i] = (slang::TypeReflection*)types[i]->getNative();
    }

	slang::FunctionReflection* specialized = m_native->specializeWithArgTypes(argCount, slangTypes);
	delete[] slangTypes; // Clean up the allocated array
    Native::FunctionReflection* result = new Native::FunctionReflection(specialized);
	m_specializeWithArgTypesResultsToDelete.push_back(result);
	return result;
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
	return m_overloads[index];
}