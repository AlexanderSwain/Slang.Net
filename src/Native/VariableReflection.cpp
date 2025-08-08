#include "VariableReflection.h"

Native::VariableReflection::VariableReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::VariableReflection*)native;
}

Native::VariableReflection::~VariableReflection()
{
    // No cached state to clean up
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
    slang::TypeReflection* typePtr = m_native->getType();
    return typePtr ? new TypeReflection(typePtr) : nullptr;
}

Native::Modifier* Native::VariableReflection::findModifier(Modifier::ID id)
{
    slang::Modifier* nativeModifier = m_native->findModifier((slang::Modifier::ID)id);
    return nativeModifier ? new Native::Modifier(nativeModifier) : nullptr;
}

unsigned int Native::VariableReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}

Native::Attribute* Native::VariableReflection::getUserAttributeByIndex(unsigned int index)
{
    slang::Attribute* nativeUserAttribute = m_native->getUserAttributeByIndex(index);
    return nativeUserAttribute ? new Attribute(nativeUserAttribute) : nullptr;
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
    slang::GenericReflection* containerPtr = m_native->getGenericContainer();
    return containerPtr ? new GenericReflection(containerPtr) : nullptr;
}

Native::VariableReflection* Native::VariableReflection::applySpecializations(GenericReflection* genRef)
{
    slang::VariableReflection* nativeResult = m_native->applySpecializations((slang::GenericReflection*)genRef->getNative());
    return nativeResult ? new Native::VariableReflection(nativeResult) : nullptr;
}