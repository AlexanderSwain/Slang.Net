#include "TypeReflection.h"
#include "Attribute.h"
#include "VariableReflection.h"
#include "GenericReflection.h"

Native::TypeReflection::TypeReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::TypeReflection*)native;
}

Native::TypeReflection::~TypeReflection()
{
    // No cached state to clean up
}

slang::TypeReflection* Native::TypeReflection::getNative()
{
    return m_native;
}

Native::TypeReflection::Kind Native::TypeReflection::getKind() 
{
    return (Native::TypeReflection::Kind)m_native->getKind();
}

// only useful if `getKind() == Kind::Struct`
unsigned int Native::TypeReflection::getFieldCount()
{
    return m_native->getFieldCount();
}

Native::VariableReflection* Native::TypeReflection::getFieldByIndex(unsigned int index)
{
    slang::VariableReflection* nativeField = m_native->getFieldByIndex(index);
    return nativeField ? new VariableReflection(nativeField) : nullptr;
}

bool Native::TypeReflection::isArray() 
{ 
    return m_native->isArray(); 
}

Native::TypeReflection* Native::TypeReflection::unwrapArray()
{
    slang::TypeReflection* nativeUnwrappedArray = m_native->unwrapArray();
    return nativeUnwrappedArray ? new TypeReflection(nativeUnwrappedArray) : nullptr;
}

// only useful if `getKind() == Kind::Array`
size_t Native::TypeReflection::getElementCount()
{
    return m_native->getElementCount();
}

size_t Native::TypeReflection::getTotalArrayElementCount()
{
    return m_native->getTotalArrayElementCount();
}

Native::TypeReflection* Native::TypeReflection::getElementType()
{
    slang::TypeReflection* nativeElementType = m_native->getElementType();
    return nativeElementType ? new TypeReflection(nativeElementType) : nullptr;
}

unsigned Native::TypeReflection::getRowCount() 
{ 
    return m_native->getRowCount();
}

unsigned Native::TypeReflection::getColumnCount()
{
    return m_native->getColumnCount();
}

Native::TypeReflection::ScalarType Native::TypeReflection::getScalarType()
{
    return (Native::TypeReflection::ScalarType)(int)m_native->getScalarType();
}

Native::TypeReflection* Native::TypeReflection::getResourceResultType()
{
    slang::TypeReflection* nativeResourceResultType = m_native->getResourceResultType();
    return nativeResourceResultType ? new TypeReflection(nativeResourceResultType) : nullptr;
}

Native::ResourceShape Native::TypeReflection::getResourceShape()
{
    return (Native::ResourceShape)m_native->getResourceShape();
}

Native::ResourceAccess Native::TypeReflection::getResourceAccess()
{
    return (Native::ResourceAccess)m_native->getResourceAccess();
}

char const* Native::TypeReflection::getName() 
{ 
    return m_native->getName();
}

SlangResult Native::TypeReflection::getFullName(ISlangBlob** outNameBlob)
{
    return m_native->getFullName(outNameBlob);
}

unsigned int Native::TypeReflection::getUserAttributeCount()
{
    return m_native->getUserAttributeCount();
}

Native::Attribute* Native::TypeReflection::getUserAttributeByIndex(unsigned int index)
{
    slang::Attribute* nativeUserAttribute = m_native->getUserAttributeByIndex(index);
    return nativeUserAttribute ? new Attribute(nativeUserAttribute) : nullptr;
}

Native::Attribute* Native::TypeReflection::findAttributeByName(char const* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    return new Attribute(m_native->findAttributeByName(name));
}

Native::Attribute* Native::TypeReflection::findUserAttributeByName(char const* name) 
{ 
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    return new Attribute(m_native->findUserAttributeByName(name));
}

Native::TypeReflection* Native::TypeReflection::applySpecializations(GenericReflection* genRef)
{
    Native::TypeReflection* result = new TypeReflection(m_native->applySpecializations((slang::GenericReflection*)genRef->getNative()));
    return result;
}

Native::GenericReflection* Native::TypeReflection::getGenericContainer()
{
    slang::GenericReflection* nativeGenericContainer = m_native->getGenericContainer();
    return nativeGenericContainer ? new GenericReflection(nativeGenericContainer) : nullptr;
}