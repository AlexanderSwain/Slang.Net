#include "VariableReflection.h"

Native::VariableReflection::VariableReflection(void* native)
{
	m_native = (slang::VariableReflection*)native;
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
    return new TypeReflection(m_native->getType());
}

Native::Modifier* Native::VariableReflection::findModifier(Modifier::ID id)
{
    return new Modifier(m_native->findModifier((slang::Modifier::ID)id));
}

unsigned int Native::VariableReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}

Native::Attribute* Native::VariableReflection::getUserAttributeByIndex(unsigned int index)
{
    return new Attribute(m_native->getUserAttributeByIndex(index));
}

Native::Attribute* Native::VariableReflection::findAttributeByName(char const* name)
{
    return new Attribute(m_native->findAttributeByName(SessionCLI::GetGlobalSession(), name));
}

Native::Attribute* Native::VariableReflection::findUserAttributeByName(char const* name)
{
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
    return new GenericReflection(m_native->getGenericContainer());
}

Native::VariableReflection* Native::VariableReflection::applySpecializations(GenericReflection* genRef)
{
    return new VariableReflection(m_native->applySpecializations((slang::GenericReflection*)genRef->getNative()));
}