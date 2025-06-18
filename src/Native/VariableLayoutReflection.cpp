#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "TypeLayoutReflection.h"
#include "TypeReflection.h"
#include "Modifier.h"

Native::VariableLayoutReflection::VariableLayoutReflection(void* native)
{
	m_native = (slang::VariableLayoutReflection*)native;
}

slang::VariableLayoutReflection* Native::VariableLayoutReflection::getNative()
{
    return m_native;
}

Native::VariableReflection* Native::VariableLayoutReflection::getVariable()
{
    return new Native::VariableReflection(m_native->getVariable());
}

char const* Native::VariableLayoutReflection::getName() 
{
    return m_native->getName();
}

Native::Modifier* Native::VariableLayoutReflection::findModifier(Modifier::ID id)
{
    return new Native::Modifier(m_native->findModifier((slang::Modifier::ID)id));
}

Native::TypeLayoutReflection* Native::VariableLayoutReflection::getTypeLayout()
{
    return new Native::TypeLayoutReflection(m_native->getTypeLayout());
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
    return new TypeReflection(m_native->getType());
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
    return new VariableLayoutReflection(m_native->getPendingDataLayout());
}