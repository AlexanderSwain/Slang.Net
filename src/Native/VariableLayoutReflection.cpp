#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "TypeLayoutReflection.h"
#include "TypeReflection.h"
#include "Modifier.h"

Native::VariableLayoutReflection::VariableLayoutReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::VariableLayoutReflection*)native;
}

Native::VariableLayoutReflection::~VariableLayoutReflection()
{
    // No cached state to clean up
}

slang::VariableLayoutReflection* Native::VariableLayoutReflection::getNative()
{
    return m_native;
}

Native::VariableReflection* Native::VariableLayoutReflection::getVariable()
{
    slang::VariableReflection* variablePtr = m_native->getVariable();
    return variablePtr ? new Native::VariableReflection(variablePtr) : nullptr;
}

char const* Native::VariableLayoutReflection::getName() 
{
    return m_native->getName();
}

Native::Modifier* Native::VariableLayoutReflection::findModifier(Modifier::ID id)
{
    slang::Modifier* nativeModifier = m_native->findModifier((slang::Modifier::ID)id);
    return nativeModifier ? new Native::Modifier(nativeModifier) : nullptr;
}

Native::TypeLayoutReflection* Native::VariableLayoutReflection::getTypeLayout()
{
    slang::TypeLayoutReflection* typeLayoutPtr = m_native->getTypeLayout();
    return typeLayoutPtr ? new Native::TypeLayoutReflection(typeLayoutPtr) : nullptr;
}

Native::ParameterCategory Native::VariableLayoutReflection::getCategory() 
{
    return (Native::ParameterCategory)m_native->getCategory();
}

unsigned int Native::VariableLayoutReflection::getCategoryCount()
{
    return m_native->getCategoryCount();
}

Native::ParameterCategory Native::VariableLayoutReflection::getCategoryByIndex(unsigned int index)
{
    return (Native::ParameterCategory)m_native->getCategoryByIndex(index);
}


size_t Native::VariableLayoutReflection::getOffset(SlangParameterCategory category)
{
    return m_native->getOffset(category);
}
size_t Native::VariableLayoutReflection::getOffset(slang::ParameterCategory category)
{
    return m_native->getOffset(category);
}


Native::TypeReflection* Native::VariableLayoutReflection::getType() 
{
    slang::TypeReflection* typePtr = m_native->getType();
    return typePtr ? new TypeReflection(typePtr) : nullptr;
}

unsigned int Native::VariableLayoutReflection::getBindingIndex()
{
    return m_native->getBindingIndex();
}

unsigned int Native::VariableLayoutReflection::getBindingSpace()
{
    return m_native->getBindingSpace();
}

size_t Native::VariableLayoutReflection::getBindingSpace(SlangParameterCategory category)
{
    return m_native->getBindingSpace(category);
}
size_t Native::VariableLayoutReflection::getBindingSpace(slang::ParameterCategory category)
{
    return m_native->getBindingSpace(category);
}

Native::ImageFormat Native::VariableLayoutReflection::getImageFormat()
{
    return (Native::ImageFormat)m_native->getImageFormat();
}

char const* Native::VariableLayoutReflection::getSemanticName()
{
    return m_native->getSemanticName();
}

size_t Native::VariableLayoutReflection::getSemanticIndex()
{
    return m_native->getSemanticIndex();
}

SlangStage Native::VariableLayoutReflection::getStage()
{
    return m_native->getStage();
}

Native::VariableLayoutReflection* Native::VariableLayoutReflection::getPendingDataLayout()
{
    slang::VariableLayoutReflection* pendingDataLayoutPtr = m_native->getPendingDataLayout();
    return pendingDataLayoutPtr ? new VariableLayoutReflection(pendingDataLayoutPtr) : nullptr;
}