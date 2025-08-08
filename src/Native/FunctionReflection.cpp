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
}

Native::FunctionReflection::~FunctionReflection()
{
    // No cached state to clean up
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
    slang::TypeReflection* returnTypePtr = m_native->getReturnType();
    return returnTypePtr ? new Native::TypeReflection(returnTypePtr) : nullptr;
}

unsigned int Native::FunctionReflection::getParameterCount()
{
    return m_native->getParameterCount();
}

Native::VariableReflection* Native::FunctionReflection::getParameterByIndex(unsigned int index)
{
    slang::VariableReflection* nativeParameter = m_native->getParameterByIndex(index);
    return nativeParameter ? new Native::VariableReflection(nativeParameter) : nullptr;
}

unsigned int Native::FunctionReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}

Native::Attribute* Native::FunctionReflection::getUserAttributeByIndex(unsigned int index)
{
    slang::Attribute* nativeUserAttribute = m_native->getUserAttributeByIndex(index);
    return nativeUserAttribute ? new Native::Attribute(nativeUserAttribute) : nullptr;
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
    slang::Modifier* nativeModifier = m_native->findModifier((slang::Modifier::ID)(int)id);
    return nativeModifier ? new Native::Modifier(nativeModifier) : nullptr;
}

Native::GenericReflection* Native::FunctionReflection::getGenericContainer()
{
    slang::GenericReflection* containerPtr = m_native->getGenericContainer();
    return containerPtr ? new GenericReflection(containerPtr) : nullptr;
}

Native::FunctionReflection* Native::FunctionReflection::applySpecializations(GenericReflection* genRef)
{
    slang::FunctionReflection* nativeResult = m_native->applySpecializations((slang::GenericReflection*)genRef->getNative());
    return nativeResult ? new Native::FunctionReflection(nativeResult) : nullptr;
}

Native::FunctionReflection* Native::FunctionReflection::specializeWithArgTypes(unsigned int argCount, TypeReflection* const* types)
{
    slang::TypeReflection** slangTypes = new slang::TypeReflection * [argCount];
    for (unsigned int i = 0; i < argCount; ++i) {
        slangTypes[i] = (slang::TypeReflection*)types[i]->getNative();
    }

	slang::FunctionReflection* specialized = m_native->specializeWithArgTypes(argCount, slangTypes);
	delete[] slangTypes; // Clean up the allocated array
    
    return specialized ? new Native::FunctionReflection(specialized) : nullptr;
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
    slang::FunctionReflection* nativeOverload = m_native->getOverload(index);
    return nativeOverload ? new Native::FunctionReflection(nativeOverload) : nullptr;
}