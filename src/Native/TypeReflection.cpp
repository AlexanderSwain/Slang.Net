#include "TypeReflection.h"
#include "Attribute.h"
#include "VariableReflection.h"
#include "GenericReflection.h"

Native::TypeReflection::TypeReflection(void* native)
{
	m_native = (slang::TypeReflection*)native;

    // Initialize the fields array
    uint32_t fieldsCount = m_native->getFieldCount();
    m_fields = new Native::VariableReflection*[fieldsCount];
    for (uint32_t index = 0; index < fieldsCount; index++)
    {
        m_fields[index] = new VariableReflection(m_native->getFieldByIndex(index));
    }

    m_unwrappedArray = new TypeReflection(m_native->unwrapArray());
    m_elementType = new TypeReflection(m_native->getElementType());
    m_resourceResultType = new TypeReflection(m_native->getResourceResultType());
    
    // Initialize the userAttributes array
    uint32_t userAttributesCount = m_native->getUserAttributeCount();
    m_userAttributes = new Native::Attribute*[userAttributesCount];
    for (uint32_t index = 0; index < userAttributesCount; index++)
    {
        m_userAttributes[index] = new Attribute(m_native->getUserAttributeByIndex(index));
    }

    m_genericContainer = new GenericReflection(m_native->getGenericContainer());
}

Native::TypeReflection::~TypeReflection()
{
    // Clean up the fields array
    for (uint32_t index = 0; index < m_native->getFieldCount(); index++)
    {
        delete m_fields[index];
    }
    delete[] m_fields;
    m_fields = nullptr;

    // Clean up unwrapped array type
    delete m_unwrappedArray;
    m_unwrappedArray = nullptr;

    // Clean up element type
    delete m_elementType;
    m_elementType = nullptr;

    // Clean up resource result type
    delete m_resourceResultType;
    m_resourceResultType = nullptr;

    // Clean up the user attributes array
    for (uint32_t index = 0; index < m_native->getUserAttributeCount(); index++)
    {
        delete m_userAttributes[index];
    }
    delete[] m_userAttributes;
    m_userAttributes = nullptr;

    // Clean up ApplySpecializations Results list
    for (auto& result : m_applySpecializationsResultsToDelete) {
        delete result;
    }
    m_applySpecializationsResultsToDelete.clear();

    // Clean up generic container
    delete m_genericContainer;
    m_genericContainer = nullptr;

    // No need to delete m_native here, as it is managed by Slang
    //if (m_native)
    //{
    //	delete m_native;
    //	m_native = nullptr;
    //}
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
	return m_fields[index];
}

bool Native::TypeReflection::isArray() 
{ 
    return m_native->isArray(); 
}

Native::TypeReflection* Native::TypeReflection::unwrapArray()
{
    return m_unwrappedArray;
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
    return m_elementType;
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
    return m_resourceResultType;
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
    return m_userAttributes[index];
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
    m_applySpecializationsResultsToDelete.push_back(result);
    return result;
}

Native::GenericReflection* Native::TypeReflection::getGenericContainer()
{
    return m_genericContainer;
}