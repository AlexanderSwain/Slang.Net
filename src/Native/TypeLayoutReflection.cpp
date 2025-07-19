#include "TypeLayoutReflection.h"
#include "TypeReflection.h"
#include "VariableReflection.h"
#include "VariableLayoutReflection.h"

Native::TypeLayoutReflection::TypeLayoutReflection(void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");

	m_native = (slang::TypeLayoutReflection*)native;

	// Use lazy initialization - only initialize when accessed
	m_type = nullptr;
	m_fields = nullptr;
    m_explicitCounter = nullptr;
    m_unwrappedArray = nullptr;
    m_elementTypeLayout = nullptr;
    m_elementVarLayout = nullptr;
    m_containerVarLayout = nullptr;
    m_resourceResultType = nullptr;
    m_pendingDataTypeLayout = nullptr;
    m_specializedTypePendingDataVarLayout = nullptr;
    m_bindingRangeLeafTypeLayouts = nullptr;
	m_bindingRangeLeafVariables = nullptr;
    m_subObjectRangeOffsets = nullptr;
}

Native::TypeLayoutReflection::~TypeLayoutReflection()
{
    // Clean up the type reflection
    delete m_type;
    m_type = nullptr;

    // Clean up the field layouts array
    if (m_fields)
    {
        for (uint32_t index = 0; index < m_native->getFieldCount(); index++)
        {
            delete m_fields[index];
        }
        delete[] m_fields;
        m_fields = nullptr;
    }

    // Clean up explicit counter
    delete m_explicitCounter;
    m_explicitCounter = nullptr;

    // Clean up unwrapped array
    delete m_unwrappedArray;
    m_unwrappedArray = nullptr;

    // Clean up element type layout
    delete m_elementTypeLayout;
    m_elementTypeLayout = nullptr;

    // Clean up element variable layout
    delete m_elementVarLayout;
    m_elementVarLayout = nullptr;

    // Clean up container variable layout
    delete m_containerVarLayout;
    m_containerVarLayout = nullptr;

    // Clean up resource result type
    delete m_resourceResultType;
    m_resourceResultType = nullptr;

    // Clean up pending data type layout
    delete m_pendingDataTypeLayout;
    m_pendingDataTypeLayout = nullptr;

    // Clean up specialized type pending data variable layout
    delete m_specializedTypePendingDataVarLayout;
    m_specializedTypePendingDataVarLayout = nullptr;

    // Clean up binding range leaf type layouts
    if (m_bindingRangeLeafTypeLayouts)
    {
        for (uint32_t index = 0; index < m_native->getBindingRangeCount(); index++)
        {
            delete m_bindingRangeLeafTypeLayouts[index];
        }
        delete[] m_bindingRangeLeafTypeLayouts;
        m_bindingRangeLeafTypeLayouts = nullptr;
    }

    // Clean up binding range leaf variables
    if (m_bindingRangeLeafVariables)
    {
        for (uint32_t index = 0; index < m_native->getBindingRangeCount(); index++)
        {
            delete m_bindingRangeLeafVariables[index];
        }
        delete[] m_bindingRangeLeafVariables;
        m_bindingRangeLeafVariables = nullptr;
    }

    // Clean up sub-object range offsets
    if (m_subObjectRangeOffsets)
    {
        for (uint32_t index = 0; index < m_native->getSubObjectRangeCount(); index++)
        {
            delete m_subObjectRangeOffsets[index];
        }
        delete[] m_subObjectRangeOffsets;
        m_subObjectRangeOffsets = nullptr;
    }

    // No need to delete m_native here, as it is managed by Slang
    //if (m_native)
    //{
    //	delete m_native;
    //	m_native = nullptr;
    //}
}

slang::TypeLayoutReflection* Native::TypeLayoutReflection::getNative()
{
    return m_native;
}

Native::TypeReflection* Native::TypeLayoutReflection::getType()
{
    if (!m_type)
    {
        slang::TypeReflection* typePtr = m_native->getType();
        if (typePtr) 
            m_type = new Native::TypeReflection(typePtr);
        else
            m_type = nullptr;
    }
    return m_type;
}

Native::TypeReflection::Kind Native::TypeLayoutReflection::getKind()
{
    return (Native::TypeReflection::Kind)m_native->getKind();
}

size_t Native::TypeLayoutReflection::getSize(ParameterCategory category)
{
    return m_native->getSize((SlangParameterCategory)category);
}

//size_t Native::TypeLayoutReflection::getSize(slang::ParameterCategory category)
//{
//    return m_native->getSize(category);
//}

size_t Native::TypeLayoutReflection::getStride(ParameterCategory category)
{
    return m_native->getStride((SlangParameterCategory)category);
}

int32_t Native::TypeLayoutReflection::getAlignment(ParameterCategory category)
{
    return m_native->getAlignment((SlangParameterCategory)category);
}


unsigned int Native::TypeLayoutReflection::getFieldCount()
{
    return m_native->getFieldCount();
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getFieldByIndex(unsigned int index)
{
    if (!m_fields)
    {
        uint32_t fieldCount = m_native->getFieldCount();
        m_fields = new Native::VariableLayoutReflection*[fieldCount];
        for (uint32_t i = 0; i < fieldCount; i++)
        {
            slang::VariableLayoutReflection* nativeField = m_native->getFieldByIndex(i);
            if (nativeField)
                m_fields[i] = new VariableLayoutReflection(nativeField);
            else
                m_fields[i] = nullptr;
        }
    }
    return m_fields[index];
}

SlangInt Native::TypeLayoutReflection::findFieldIndexByName(char const* nameBegin, char const* nameEnd)
{
    return m_native->findFieldIndexByName(nameBegin, nameEnd);
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getExplicitCounter()
{
    if (!m_explicitCounter)
    {
        slang::VariableLayoutReflection* explicitCounterPtr = m_native->getExplicitCounter();
        if (explicitCounterPtr) 
            m_explicitCounter = new VariableLayoutReflection(explicitCounterPtr);
        else
            m_explicitCounter = nullptr;
    }
    return m_explicitCounter;
}

bool Native::TypeLayoutReflection::isArray() 
{
    return m_native->isArray();
}

Native::TypeLayoutReflection* Native::TypeLayoutReflection::unwrapArray()
{
    if (!m_unwrappedArray)
    {
        slang::TypeLayoutReflection* unwrappedArrayPtr = m_native->unwrapArray();
        if (unwrappedArrayPtr) 
            m_unwrappedArray = new TypeLayoutReflection(unwrappedArrayPtr);
        else
            m_unwrappedArray = nullptr;
    }
    return m_unwrappedArray;
}

// only useful if `getKind() == Kind::Array`
size_t Native::TypeLayoutReflection::getElementCount() 
{
    return m_native->getElementCount();
}

size_t Native::TypeLayoutReflection::getTotalArrayElementCount() 
{
    return m_native->getTotalArrayElementCount();
}

size_t Native::TypeLayoutReflection::getElementStride(ParameterCategory category)
{
    return m_native->getElementStride((SlangParameterCategory)category);
}

Native::TypeLayoutReflection* Native::TypeLayoutReflection::getElementTypeLayout()
{
    if (!m_elementTypeLayout)
    {
        slang::TypeLayoutReflection* elementTypeLayoutPtr = m_native->getElementTypeLayout();
        if (elementTypeLayoutPtr) 
            m_elementTypeLayout = new TypeLayoutReflection(elementTypeLayoutPtr);
        else
            m_elementTypeLayout = nullptr;
    }
    return m_elementTypeLayout;
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getElementVarLayout()
{
    if (!m_elementVarLayout)
    {
        slang::VariableLayoutReflection* elementVarLayoutPtr = m_native->getElementVarLayout();
        if (elementVarLayoutPtr) 
            m_elementVarLayout = new VariableLayoutReflection(elementVarLayoutPtr);
        else
            m_elementVarLayout = nullptr;
    }
    return m_elementVarLayout;
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getContainerVarLayout()
{
    if (!m_containerVarLayout)
    {
        slang::VariableLayoutReflection* containerVarLayoutPtr = m_native->getContainerVarLayout();
        if (containerVarLayoutPtr) 
            m_containerVarLayout = new VariableLayoutReflection(containerVarLayoutPtr);
        else
            m_containerVarLayout = nullptr;
    }
    return m_containerVarLayout;
}

// How is this type supposed to be bound?
Native::ParameterCategory Native::TypeLayoutReflection::getParameterCategory()
{
    return Native::ParameterCategory(m_native->getParameterCategory());
}

unsigned int Native::TypeLayoutReflection::getCategoryCount()
{
    return m_native->getCategoryCount();
}

Native::ParameterCategory Native::TypeLayoutReflection::getCategoryByIndex(unsigned int index)
{
    return Native::ParameterCategory(m_native->getCategoryByIndex(index));
}

unsigned int Native::TypeLayoutReflection::getRowCount() 
{
    return m_native->getRowCount();
}

unsigned int Native::TypeLayoutReflection::getColumnCount() 
{
    return m_native->getColumnCount();
}

Native::TypeReflection::ScalarType Native::TypeLayoutReflection::getScalarType() 
{
    return (Native::TypeReflection::ScalarType)m_native->getScalarType();
}

Native::TypeReflection* Native::TypeLayoutReflection::getResourceResultType() 
{
    if (!m_resourceResultType)
    {
        slang::TypeReflection* resourceResultTypePtr = m_native->getResourceResultType();
        if (resourceResultTypePtr) 
            m_resourceResultType = new TypeReflection(resourceResultTypePtr);
        else
            m_resourceResultType = nullptr;
    }
    return m_resourceResultType;
}

Native::ResourceShape Native::TypeLayoutReflection::getResourceShape() 
{
    return (Native::ResourceShape)(int)m_native->getResourceShape();
}

Native::ResourceAccess Native::TypeLayoutReflection::getResourceAccess() 
{
    return (Native::ResourceAccess)(int)m_native->getResourceAccess();
}

char const* Native::TypeLayoutReflection::getName() 
{
    return m_native->getName();
}

Native::MatrixLayoutMode Native::TypeLayoutReflection::getMatrixLayoutMode()
{
    return (Native::MatrixLayoutMode)(int)m_native->getMatrixLayoutMode();
}

int Native::TypeLayoutReflection::getGenericParamIndex()
{
    return m_native->getGenericParamIndex();
}

Native::TypeLayoutReflection* Native::TypeLayoutReflection::getPendingDataTypeLayout()
{
    if (!m_pendingDataTypeLayout)
    {
        slang::TypeLayoutReflection* pendingDataTypeLayoutPtr = m_native->getPendingDataTypeLayout();
        if (pendingDataTypeLayoutPtr) 
            m_pendingDataTypeLayout = new TypeLayoutReflection(pendingDataTypeLayoutPtr);
        else
            m_pendingDataTypeLayout = nullptr;
    }
    return m_pendingDataTypeLayout;
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getSpecializedTypePendingDataVarLayout()
{
    if (!m_specializedTypePendingDataVarLayout)
    {
        slang::VariableLayoutReflection* specializedTypePendingDataVarLayoutPtr = m_native->getSpecializedTypePendingDataVarLayout();
        if (specializedTypePendingDataVarLayoutPtr) 
            m_specializedTypePendingDataVarLayout = new VariableLayoutReflection(specializedTypePendingDataVarLayoutPtr);
        else
            m_specializedTypePendingDataVarLayout = nullptr;
    }
    return m_specializedTypePendingDataVarLayout;
}

SlangInt Native::TypeLayoutReflection::getBindingRangeCount()
{
    return m_native->getBindingRangeCount();
}

Native::BindingType Native::TypeLayoutReflection::getBindingRangeType(SlangInt index)
{
    return (Native::BindingType)(int)m_native->getBindingRangeType(index);
}

bool Native::TypeLayoutReflection::isBindingRangeSpecializable(SlangInt index)
{
    return m_native->isBindingRangeSpecializable(index);
}

SlangInt Native::TypeLayoutReflection::getBindingRangeBindingCount(SlangInt index)
{
    return m_native->getBindingRangeBindingCount(index);
}

/*
SlangInt getBindingRangeIndexOffset(SlangInt index)
{
    return spReflectionTypeLayout_getBindingRangeIndexOffset(
        (SlangReflectionTypeLayout*) this,
        index);
}

SlangInt getBindingRangeSpaceOffset(SlangInt index)
{
    return spReflectionTypeLayout_getBindingRangeSpaceOffset(
        (SlangReflectionTypeLayout*) this,
        index);
}
*/

SlangInt Native::TypeLayoutReflection::getFieldBindingRangeOffset(SlangInt fieldIndex)
{
    return m_native->getFieldBindingRangeOffset(fieldIndex);
}

SlangInt Native::TypeLayoutReflection::getExplicitCounterBindingRangeOffset()
{
    return m_native->getExplicitCounterBindingRangeOffset();
}

Native::TypeLayoutReflection* Native::TypeLayoutReflection::getBindingRangeLeafTypeLayout(SlangInt index)
{
    if (!m_bindingRangeLeafTypeLayouts)
    {
        uint32_t bindingRangeCount = m_native->getBindingRangeCount();
        m_bindingRangeLeafTypeLayouts = new Native::TypeLayoutReflection*[bindingRangeCount];
        for (uint32_t i = 0; i < bindingRangeCount; i++)
        {
            slang::TypeLayoutReflection* nativeBindingRangeLeafTypeLayout = m_native->getBindingRangeLeafTypeLayout(i);
            if (nativeBindingRangeLeafTypeLayout)
                m_bindingRangeLeafTypeLayouts[i] = new TypeLayoutReflection(nativeBindingRangeLeafTypeLayout);
            else
                m_bindingRangeLeafTypeLayouts[i] = nullptr;
        }
    }
	return m_bindingRangeLeafTypeLayouts[index];
}

Native::VariableReflection* Native::TypeLayoutReflection::getBindingRangeLeafVariable(SlangInt index)
{
    if (!m_bindingRangeLeafVariables)
    {
        uint32_t bindingRangeCount = m_native->getBindingRangeCount();
        m_bindingRangeLeafVariables = new Native::VariableReflection*[bindingRangeCount];
        for (uint32_t i = 0; i < bindingRangeCount; i++)
        {
            slang::VariableReflection* nativeBindingRangeLeafVariable = m_native->getBindingRangeLeafVariable(i);
            if (nativeBindingRangeLeafVariable)
                m_bindingRangeLeafVariables[i] = new VariableReflection(nativeBindingRangeLeafVariable);
            else
                m_bindingRangeLeafVariables[i] = nullptr;
        }
    }
    return m_bindingRangeLeafVariables[index];
}

Native::ImageFormat Native::TypeLayoutReflection::getBindingRangeImageFormat(SlangInt index)
{
    return (Native::ImageFormat)(int)m_native->getBindingRangeImageFormat(index);
}

SlangInt Native::TypeLayoutReflection::getBindingRangeDescriptorSetIndex(SlangInt index)
{
    return m_native->getBindingRangeDescriptorSetIndex(index);
}

SlangInt Native::TypeLayoutReflection::getBindingRangeFirstDescriptorRangeIndex(SlangInt index)
{
    return m_native->getBindingRangeFirstDescriptorRangeIndex(index);
}

SlangInt Native::TypeLayoutReflection::getBindingRangeDescriptorRangeCount(SlangInt index)
{
    return m_native->getBindingRangeDescriptorRangeCount(index);
}

SlangInt Native::TypeLayoutReflection::getDescriptorSetCount()
{
    return m_native->getDescriptorSetCount();
}

SlangInt Native::TypeLayoutReflection::getDescriptorSetSpaceOffset(SlangInt setIndex)
{
    return m_native->getDescriptorSetSpaceOffset(setIndex);
}

SlangInt Native::TypeLayoutReflection::getDescriptorSetDescriptorRangeCount(SlangInt setIndex)
{
    return m_native->getDescriptorSetDescriptorRangeCount(setIndex);
}

SlangInt Native::TypeLayoutReflection::getDescriptorSetDescriptorRangeIndexOffset(SlangInt setIndex, SlangInt rangeIndex)
{
    return m_native->getDescriptorSetDescriptorRangeIndexOffset(setIndex, rangeIndex);
}

SlangInt Native::TypeLayoutReflection::getDescriptorSetDescriptorRangeDescriptorCount(SlangInt setIndex, SlangInt rangeIndex)
{
    return m_native->getDescriptorSetDescriptorRangeDescriptorCount(setIndex, rangeIndex);
}

Native::BindingType Native::TypeLayoutReflection::getDescriptorSetDescriptorRangeType(SlangInt setIndex, SlangInt rangeIndex)
{
    return (Native::BindingType)(int)m_native->getDescriptorSetDescriptorRangeType(setIndex, rangeIndex);
}

Native::ParameterCategory Native::TypeLayoutReflection::getDescriptorSetDescriptorRangeCategory(
    SlangInt setIndex,
    SlangInt rangeIndex)
{
    return (Native::ParameterCategory)(int)m_native->getDescriptorSetDescriptorRangeCategory(setIndex, rangeIndex);
}

SlangInt Native::TypeLayoutReflection::getSubObjectRangeCount()
{
    return m_native->getSubObjectRangeCount();
}

SlangInt Native::TypeLayoutReflection::getSubObjectRangeBindingRangeIndex(SlangInt subObjectRangeIndex)
{
    return m_native->getSubObjectRangeBindingRangeIndex(subObjectRangeIndex);
}

SlangInt Native::TypeLayoutReflection::getSubObjectRangeSpaceOffset(SlangInt subObjectRangeIndex)
{
    return m_native->getSubObjectRangeSpaceOffset(subObjectRangeIndex);
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getSubObjectRangeOffset(SlangInt subObjectRangeIndex)
{
    if (!m_subObjectRangeOffsets)
    {
        uint32_t subObjectRangeCount = m_native->getSubObjectRangeCount();
        m_subObjectRangeOffsets = new Native::VariableLayoutReflection*[subObjectRangeCount];
        for (uint32_t i = 0; i < subObjectRangeCount; i++)
        {
            slang::VariableLayoutReflection* nativeSubObjectRangeOffset = m_native->getSubObjectRangeOffset(i);
            if (nativeSubObjectRangeOffset)
                m_subObjectRangeOffsets[i] = new VariableLayoutReflection(nativeSubObjectRangeOffset);
            else
                m_subObjectRangeOffsets[i] = nullptr;
        }
    }
	return m_subObjectRangeOffsets[subObjectRangeIndex];
}