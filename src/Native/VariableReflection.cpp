#include "VariableReflection.h"

Native::VariableReflection::VariableReflection(void* native)
{
	m_native = (slang::VariableReflection*)native;

	// Initialize the type reflection
	m_type = new TypeReflection(m_native->getType());

	// Initialize user attributes
	unsigned int userAttributeCount = m_native->getUserAttributeCount();
	m_userAttributes = new Attribute * [userAttributeCount];
    for (unsigned int index = 0; index < userAttributeCount; index++)
    {
        m_userAttributes[index] = new Attribute(m_native->getUserAttributeByIndex(index));
	}

	// Initialize generic container
	m_genericContainer = new GenericReflection(m_native->getGenericContainer());
}

Native::VariableReflection::~VariableReflection()
{
    // Clean up the type reflection
    delete m_type;
    m_type = nullptr;

    // Clean up modifiers
    for (auto& pair : m_modifiers)
    {
        delete pair.second; // Delete the Modifier object
    }
    m_modifiers.clear(); // Clear the map

    // Clean up user attributes
    for (unsigned int index = 0; index < m_native->getUserAttributeCount(); index++)
    {
        delete m_userAttributes[index];
    }
    delete[] m_userAttributes;
    m_userAttributes = nullptr;

    // Clean up generic container
    delete m_genericContainer;
    m_genericContainer = nullptr;

    // Clean up applySpecializations Results list
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

slang::VariableReflection* Native::VariableReflection::getNative()
{
    return m_native;
}

char const* Native::VariableReflection::getName()
{ 
    return m_native->getName();
}

Native::TypeReflection* Native::VariableReflection::getType()
{
    return m_type;
}

Native::Modifier* Native::VariableReflection::findModifier(Modifier::ID id)
{
    // Check if the modifier is already cached
    auto it = m_modifiers.find(id);

    // If the modifier is already cached, return it
    if (it != m_modifiers.end())
        return it->second;

    // If not cached, create a new Modifier and cache it
    Native::Modifier* result = new Native::Modifier(m_native->findModifier((slang::Modifier::ID)id));
    m_modifiers[id] = result;
    return result;
}

unsigned int Native::VariableReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}

Native::Attribute* Native::VariableReflection::getUserAttributeByIndex(unsigned int index)
{
	return m_userAttributes[index];
}

Native::Attribute* Native::VariableReflection::findAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    return new Attribute(m_native->findAttributeByName(SessionCLI::GetGlobalSession(), name));
}

Native::Attribute* Native::VariableReflection::findUserAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    return new Attribute(m_native->findUserAttributeByName(SessionCLI::GetGlobalSession(), name));
}

bool Native::VariableReflection::hasDefaultValue()
{
    return m_native->hasDefaultValue();
}

SlangResult Native::VariableReflection::getDefaultValueInt(int64_t* value)
{
    return m_native->getDefaultValueInt(value);
}

Native::GenericReflection* Native::VariableReflection::getGenericContainer()
{
    return m_genericContainer;
}

Native::VariableReflection* Native::VariableReflection::applySpecializations(GenericReflection* genRef)
{
    Native::VariableReflection* result = new Native::VariableReflection(m_native->applySpecializations((slang::GenericReflection*)genRef->getNative()));
    m_applySpecializationsResultsToDelete.push_back(result);
    return result;
}