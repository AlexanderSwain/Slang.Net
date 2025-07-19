#include "VariableReflection.h"

Native::VariableReflection::VariableReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::VariableReflection*)native;

	// Use lazy initialization - only initialize when accessed
	m_type = nullptr;
	m_userAttributes = nullptr;
	m_genericContainer = nullptr;
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
    if (m_userAttributes)
    {
        for (unsigned int index = 0; index < m_native->getUserAttributeCount(); index++)
        {
            delete m_userAttributes[index];
        }
        delete[] m_userAttributes;
        m_userAttributes = nullptr;
    }

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
    if (!m_type)
    {
        slang::TypeReflection* typePtr = m_native->getType();
        if (typePtr) 
            m_type = new TypeReflection(typePtr);
        else
            m_type = nullptr;
    }
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
    slang::Modifier* nativeModifier = m_native->findModifier((slang::Modifier::ID)id);
    if (nativeModifier)
    {
        Native::Modifier* result = new Native::Modifier(nativeModifier);
        m_modifiers[id] = result;
        return result;
    }
    return nullptr;
}

unsigned int Native::VariableReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}

Native::Attribute* Native::VariableReflection::getUserAttributeByIndex(unsigned int index)
{
    if (!m_userAttributes)
    {
        unsigned int userAttributeCount = m_native->getUserAttributeCount();
        m_userAttributes = new Attribute * [userAttributeCount];
        for (unsigned int i = 0; i < userAttributeCount; i++)
        {
            slang::Attribute* nativeUserAttribute = m_native->getUserAttributeByIndex(i);
            if (nativeUserAttribute)
                m_userAttributes[i] = new Attribute(nativeUserAttribute);
            else
                m_userAttributes[i] = nullptr;
        }
    }
	return m_userAttributes[index];
}

Native::Attribute* Native::VariableReflection::findAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    slang::Attribute* nativeAttribute = m_native->findAttributeByName(SessionCLI::GetGlobalSession(), name);
    if (nativeAttribute)
        return new Attribute(nativeAttribute);
    return nullptr;
}

Native::Attribute* Native::VariableReflection::findUserAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    slang::Attribute* nativeAttribute = m_native->findUserAttributeByName(SessionCLI::GetGlobalSession(), name);
    if (nativeAttribute)
        return new Attribute(nativeAttribute);
    return nullptr;
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

Native::VariableReflection* Native::VariableReflection::applySpecializations(GenericReflection* genRef)
{
    slang::VariableReflection* nativeResult = m_native->applySpecializations((slang::GenericReflection*)genRef->getNative());
    if (nativeResult)
    {
        Native::VariableReflection* result = new Native::VariableReflection(nativeResult);
        m_applySpecializationsResultsToDelete.push_back(result);
        return result;
    }
    return nullptr;
}