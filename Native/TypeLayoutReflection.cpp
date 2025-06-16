#include "TypeLayoutReflection.h"
#include "TypeReflection.h"
#include "VariableReflection.h"
#include "VariableLayoutReflection.h"

Native::TypeLayoutReflection::TypeLayoutReflection(void* native)
{
	m_native = (slang::TypeLayoutReflection*)native;
}

slang::TypeLayoutReflection* Native::TypeLayoutReflection::getNative()
{
    return m_native;
}

Native::TypeReflection* Native::TypeLayoutReflection::getType()
{
    return new TypeReflection(m_native->getType());
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
    return new VariableLayoutReflection(m_native->getFieldByIndex(index));
}

SlangInt Native::TypeLayoutReflection::findFieldIndexByName(char const* nameBegin, char const* nameEnd)
{
    return m_native->findFieldIndexByName(nameBegin, nameEnd);
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getExplicitCounter()
{
    return new VariableLayoutReflection(m_native->getExplicitCounter());
}

bool Native::TypeLayoutReflection::isArray() 
{
    return m_native->isArray();
}

Native::TypeLayoutReflection* Native::TypeLayoutReflection::unwrapArray()
{
    return new TypeLayoutReflection(m_native->unwrapArray());
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
    return new TypeLayoutReflection(m_native->getElementTypeLayout());
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getElementVarLayout()
{
    return new VariableLayoutReflection(m_native->getElementVarLayout());
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getContainerVarLayout()
{
    return new VariableLayoutReflection(m_native->getContainerVarLayout());
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
    return new TypeReflection(m_native->getResourceResultType());
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
    return new TypeLayoutReflection(m_native->getPendingDataTypeLayout());
}

Native::VariableLayoutReflection* Native::TypeLayoutReflection::getSpecializedTypePendingDataVarLayout()
{
    return new VariableLayoutReflection(m_native->getSpecializedTypePendingDataVarLayout());
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
    return new TypeLayoutReflection(m_native->getBindingRangeLeafTypeLayout(index));
}

Native::VariableReflection* Native::TypeLayoutReflection::getBindingRangeLeafVariable(SlangInt index)
{
    return new VariableReflection(m_native->getBindingRangeLeafVariable(index));
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
    return new VariableLayoutReflection(m_native->getSubObjectRangeOffset(subObjectRangeIndex));
}