#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "TypeLayoutReflection.h"
#include "TypeReflection.h"
#include "Modifier.h"

Native::VariableLayoutReflection::VariableLayoutReflection(void* native)
{
	m_native = (slang::VariableLayoutReflection*)native;

    m_variable = new Native::VariableReflection(m_native->getVariable());
    m_typeLayout = new Native::TypeLayoutReflection(m_native->getTypeLayout());
    m_type = new TypeReflection(m_native->getType());
    m_pendingDataLayout = new VariableLayoutReflection(m_native->getPendingDataLayout());
}

Native::VariableLayoutReflection::~VariableLayoutReflection()
{
    // Clean up m_variable 
    delete m_variable;
    m_variable = nullptr;

    // Clean up modifiers
    for (auto& pair : m_modifiers)
    {
        delete pair.second; // Delete the Modifier object
    }
    m_modifiers.clear(); // Clear the map

    // Clean up m_typeLayout 
    delete m_typeLayout;
    m_typeLayout = nullptr;

    // Clean up m_type 
    delete m_type;
    m_type = nullptr;

    // Clean up m_pendingDataLayout 
    delete m_pendingDataLayout;
    m_pendingDataLayout = nullptr;
}

slang::VariableLayoutReflection* Native::VariableLayoutReflection::getNative()
{
    return m_native;
}

Native::VariableReflection* Native::VariableLayoutReflection::getVariable()
{
    return m_variable;
}

char const* Native::VariableLayoutReflection::getName() 
{
    return m_native->getName();
}

Native::Modifier* Native::VariableLayoutReflection::findModifier(Modifier::ID id)
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

Native::TypeLayoutReflection* Native::VariableLayoutReflection::getTypeLayout()
{
    return m_typeLayout;
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
    return m_type;
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
    return m_pendingDataLayout;
}