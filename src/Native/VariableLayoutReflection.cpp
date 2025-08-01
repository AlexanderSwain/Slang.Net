#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "TypeLayoutReflection.h"
#include "TypeReflection.h"
#include "Modifier.h"

Native::VariableLayoutReflection::VariableLayoutReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::VariableLayoutReflection*)native;

    // Use lazy initialization - only initialize when accessed
    m_variable = nullptr;
    m_typeLayout = nullptr;
    m_type = nullptr;
    m_pendingDataLayout = nullptr;
}

Native::VariableLayoutReflection::~VariableLayoutReflection()
{
    // Clean up m_variable 
    if (m_variable)
    {
        delete m_variable;
        m_variable = nullptr;
    }

    // Clean up modifiers
    for (auto& pair : m_modifiers)
    {
        delete pair.second; // Delete the Modifier object
    }
    m_modifiers.clear(); // Clear the map

    // Clean up m_typeLayout 
    if (m_typeLayout)
    {
        delete m_typeLayout;
        m_typeLayout = nullptr;
    }

    // Clean up m_type 
    if (m_type)
    {
        delete m_type;
        m_type = nullptr;
    }

    // Clean up m_pendingDataLayout 
    if (m_pendingDataLayout)
    {
        delete m_pendingDataLayout;
        m_pendingDataLayout = nullptr;
    }
}

slang::VariableLayoutReflection* Native::VariableLayoutReflection::getNative()
{
    return m_native;
}

Native::VariableReflection* Native::VariableLayoutReflection::getVariable()
{
    if (!m_variable)
    {
        slang::VariableReflection* variablePtr = m_native->getVariable();
        if (variablePtr) 
            m_variable = new Native::VariableReflection(variablePtr);
        else
            m_variable = nullptr;
    }
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
    slang::Modifier* nativeModifier = m_native->findModifier((slang::Modifier::ID)id);
    if (nativeModifier)
    {
        Native::Modifier* result = new Native::Modifier(nativeModifier);
        m_modifiers[id] = result;
        return result;
    }
    return nullptr;
}

Native::TypeLayoutReflection* Native::VariableLayoutReflection::getTypeLayout()
{
    if (!m_typeLayout)
    {
        slang::TypeLayoutReflection* typeLayoutPtr = m_native->getTypeLayout();
        if (typeLayoutPtr) 
            m_typeLayout = new Native::TypeLayoutReflection(typeLayoutPtr);
        else
            m_typeLayout = nullptr;
    }
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
    if (!m_pendingDataLayout)
    {
        slang::VariableLayoutReflection* pendingDataLayoutPtr = m_native->getPendingDataLayout();
        if (pendingDataLayoutPtr) 
            m_pendingDataLayout = new VariableLayoutReflection(pendingDataLayoutPtr);
        else
            m_pendingDataLayout = nullptr;
    }
    return m_pendingDataLayout;
}