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
    return new Native::TypeReflection(m_native->getReturnType());
}

unsigned int Native::FunctionReflection::getParameterCount()
{
    return m_native->getParameterCount();
}

Native::VariableReflection* Native::FunctionReflection::getParameterByIndex(unsigned int index)
{
    return new Native::VariableReflection(m_native->getParameterByIndex(index));
}

unsigned int Native::FunctionReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}
Native::Attribute* Native::FunctionReflection::getUserAttributeByIndex(unsigned int index)
{
    return new Native::Attribute(m_native->getUserAttributeByIndex(index));
}
Native::Attribute* Native::FunctionReflection::findAttributeByName(char const* name)
{
    return new Native::Attribute(m_native->findAttributeByName(SessionCLI::GetGlobalSession(), name));
}
Native::Attribute* Native::FunctionReflection::findUserAttributeByName(char const* name)
{
    return new Native::Attribute(m_native->findUserAttributeByName(SessionCLI::GetGlobalSession(), name));
}

Native::Modifier* Native::FunctionReflection::findModifier(Modifier::ID id)
{
    return new Native::Modifier(m_native->findModifier((slang::Modifier::ID)(int)id));
}

Native::GenericReflection* Native::FunctionReflection::getGenericContainer()
{
    return new Native::GenericReflection(m_native->getGenericContainer());
}

Native::FunctionReflection* Native::FunctionReflection::applySpecializations(GenericReflection* genRef)
{
    return new Native::FunctionReflection(m_native->applySpecializations((slang::GenericReflection*)genRef->getNative()));
}

Native::FunctionReflection* Native::FunctionReflection::specializeWithArgTypes(unsigned int argCount, TypeReflection* const* types)
{
    slang::TypeReflection** slangTypes = new slang::TypeReflection * [argCount];
    for (unsigned int i = 0; i < argCount; ++i) {
        slangTypes[i] = (slang::TypeReflection*)types[i]->getNative();
    }

	slang::FunctionReflection* specialized = m_native->specializeWithArgTypes(argCount, slangTypes);
	delete[] slangTypes; // Clean up the allocated array
    return new Native::FunctionReflection(specialized);
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
    return new Native::FunctionReflection(m_native->getOverload(index));
}