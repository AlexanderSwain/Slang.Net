#include "TypeReflection.h"
#include "Attribute.h"
#include "VariableReflection.h"
#include "GenericReflection.h"

Native::TypeReflection::TypeReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::TypeReflection*)native;
    m_fields = nullptr;
    m_unwrappedArray = nullptr;
    m_elementType = nullptr;
    m_resourceResultType = nullptr;
    m_userAttributes = nullptr;
    m_genericContainer = nullptr;
}

Native::TypeReflection::~TypeReflection()
{
    // Clean up the fields array
    if (m_fields)
    {
        uint32_t fieldsCount = m_native->getFieldCount();
        for (uint32_t index = 0; index < fieldsCount; index++)
        {
            delete m_fields[index];
        }
        delete[] m_fields;
        m_fields = nullptr;
	}

    // Clean up unwrapped array type
    if (m_unwrappedArray)
    {
        delete m_unwrappedArray;
        m_unwrappedArray = nullptr;
    }

    // Clean up element type
    if (m_elementType)
    {
        delete m_elementType;
        m_elementType = nullptr;
    }

    // Clean up resource result type
    if (m_resourceResultType)
    {
        delete m_resourceResultType;
        m_resourceResultType = nullptr;
    }

    // Clean up the user attributes array
    if (m_userAttributes)
    {
        uint32_t userAttributesCount = m_native->getUserAttributeCount();
        for (uint32_t index = 0; index < userAttributesCount; index++)
        {
            delete m_userAttributes[index];
        }
        delete[] m_userAttributes;
        m_userAttributes = nullptr;
	}

    // Clean up ApplySpecializations Results list
    for (auto& result : m_applySpecializationsResultsToDelete) {
        delete result;
    }
    m_applySpecializationsResultsToDelete.clear();

    // Clean up generic container
    if (m_genericContainer)
    {
        delete m_genericContainer;
        m_genericContainer = nullptr;
    }

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
    if (!m_fields)
    {
        uint32_t fieldsCount = m_native->getFieldCount();
        m_fields = new Native::VariableReflection * [fieldsCount];
        for (uint32_t index = 0; index < fieldsCount; index++)
        {
			slang::VariableReflection* nativeField = m_native->getFieldByIndex(index);
			if (nativeField)
                m_fields[index] = new VariableReflection(nativeField);
			else
				m_fields[index] = nullptr;
        }
    }

	return m_fields[index];
}

bool Native::TypeReflection::isArray() 
{ 
    return m_native->isArray(); 
}

Native::TypeReflection* Native::TypeReflection::unwrapArray()
{
    if (!m_unwrappedArray)
    {
		slang::TypeReflection* nativeUnwrappedArray = m_native->unwrapArray();
        if (nativeUnwrappedArray)
            m_unwrappedArray = new TypeReflection(nativeUnwrappedArray);
		else
			m_unwrappedArray = nullptr;
    }

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
    if (!m_elementType)
    {
        slang::TypeReflection* nativeElementType = m_native->getElementType();
        if (nativeElementType)
            m_elementType = new TypeReflection(nativeElementType);
		else
			m_elementType = nullptr;
    }

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
    if (!m_resourceResultType)
    {
		slang::TypeReflection* nativeResourceResultType = m_native->getResourceResultType();

        if (nativeResourceResultType)
            m_resourceResultType = new TypeReflection(nativeResourceResultType);
		else
			m_resourceResultType = nullptr;
    }

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
    if (!m_userAttributes)
    {
        uint32_t userAttributesCount = m_native->getUserAttributeCount();
        m_userAttributes = new Native::Attribute * [userAttributesCount];
        for (uint32_t index = 0; index < userAttributesCount; index++)
        {
			slang::Attribute* nativeUserAttribute = m_native->getUserAttributeByIndex(index);
			if (nativeUserAttribute)
                m_userAttributes[index] = new Attribute(nativeUserAttribute);
			else
				m_userAttributes[index] = nullptr;
        }
    }

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
    if (!m_genericContainer)
    {
		slang::GenericReflection* nativeGenericContainer = m_native->getGenericContainer();
		if (nativeGenericContainer)
            m_genericContainer = new GenericReflection(m_native->getGenericContainer());
		else
			m_genericContainer = nullptr;
    }

    return m_genericContainer;
}