#include "TypeReflection.h"
#include "Attribute.h"
#include "VariableReflection.h"
#include "GenericReflection.h"

Native::TypeReflection::TypeReflection(void* native)
{
	m_native = (slang::TypeReflection*)native;
}

void* Native::TypeReflection::getNative()
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
    return new VariableReflection(m_native->getFieldByIndex(index));
}

bool Native::TypeReflection::isArray() 
{ 
    return m_native->isArray(); 
}

Native::TypeReflection* Native::TypeReflection::unwrapArray()
{
    return new TypeReflection(m_native->unwrapArray());
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
    return new TypeReflection(m_native->getElementType());
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
    return new TypeReflection(m_native->getResourceResultType());
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
    return new Attribute(m_native->getUserAttributeByIndex(index));
}

Native::Attribute* Native::TypeReflection::findAttributeByName(char const* name)
{
    return new Attribute(m_native->findAttributeByName(name));
}

Native::Attribute* Native::TypeReflection::findUserAttributeByName(char const* name) 
{ 
    return new Attribute(m_native->findUserAttributeByName(name));
}

Native::TypeReflection* Native::TypeReflection::applySpecializations(GenericReflection* genRef)
{
    return new TypeReflection(m_native->applySpecializations((slang::GenericReflection*)genRef->getNative()));
}

Native::GenericReflection* Native::TypeReflection::getGenericContainer()
{
    return new GenericReflection(m_native->getGenericContainer());
}