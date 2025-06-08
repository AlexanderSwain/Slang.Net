#pragma once
#include "TypeReflection.h"
#include "VariableReflection.h"
#include "VariableLayoutReflection.h"
#include "ParameterCategory.h"
#include "BindingType.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	// This type is empty in slang.h for some reason
	struct SLANGNATIVE_API TypeLayoutReflection
	{

	public:
		TypeLayoutReflection(void* native);
        TypeReflection* getType();
        TypeReflection::Kind getKind();
        size_t getSize(SlangParameterCategory category);
        size_t getStride(SlangParameterCategory category);
        int32_t getAlignment(SlangParameterCategory category);
        size_t getSize(slang::ParameterCategory category = slang::ParameterCategory::Uniform);
        size_t getStride(slang::ParameterCategory category = slang::ParameterCategory::Uniform);
        int32_t getAlignment(slang::ParameterCategory category = slang::ParameterCategory::Uniform);
        unsigned int getFieldCount();
        VariableLayoutReflection* getFieldByIndex(unsigned int index);
        SlangInt findFieldIndexByName(char const* nameBegin, char const* nameEnd = nullptr);
        VariableLayoutReflection* getExplicitCounter();
        bool isArray();
        TypeLayoutReflection* unwrapArray();
        // only useful if `getKind() == Kind::Array`
        size_t getElementCount();
        size_t getTotalArrayElementCount();
        size_t getElementStride(SlangParameterCategory category);
        TypeLayoutReflection* getElementTypeLayout();
        VariableLayoutReflection* getElementVarLayout();
        VariableLayoutReflection* getContainerVarLayout();
        // How is this type supposed to be bound?
        ParameterCategory getParameterCategory();
        unsigned int getCategoryCount();
        ParameterCategory getCategoryByIndex(unsigned int index);
        unsigned int getRowCount();
        unsigned int getColumnCount();
        TypeReflection::ScalarType getScalarType();
        TypeReflection* getResourceResultType();
        SlangResourceShape getResourceShape();
        SlangResourceAccess getResourceAccess();
        char const* getName();
        SlangMatrixLayoutMode getMatrixLayoutMode();
        int getGenericParamIndex();
        TypeLayoutReflection* getPendingDataTypeLayout();
        VariableLayoutReflection* getSpecializedTypePendingDataVarLayout();
        SlangInt getBindingRangeCount();
        BindingType getBindingRangeType(SlangInt index);
        bool isBindingRangeSpecializable(SlangInt index);
        SlangInt getBindingRangeBindingCount(SlangInt index);
        /*
        SlangInt getBindingRangeIndexOffset(SlangInt index);
        SlangInt getBindingRangeSpaceOffset(SlangInt index);
        */
        SlangInt getFieldBindingRangeOffset(SlangInt fieldIndex);
        SlangInt getExplicitCounterBindingRangeOffset();
        TypeLayoutReflection* getBindingRangeLeafTypeLayout(SlangInt index);
        VariableReflection* getBindingRangeLeafVariable(SlangInt index);
        SlangImageFormat getBindingRangeImageFormat(SlangInt index);
        SlangInt getBindingRangeDescriptorSetIndex(SlangInt index);
        SlangInt getBindingRangeFirstDescriptorRangeIndex(SlangInt index);
        SlangInt getBindingRangeDescriptorRangeCount(SlangInt index);
        SlangInt getDescriptorSetCount();
        SlangInt getDescriptorSetSpaceOffset(SlangInt setIndex);
        SlangInt getDescriptorSetDescriptorRangeCount(SlangInt setIndex);
        SlangInt getDescriptorSetDescriptorRangeIndexOffset(SlangInt setIndex, SlangInt rangeIndex);
        SlangInt getDescriptorSetDescriptorRangeDescriptorCount(SlangInt setIndex, SlangInt rangeIndex);
        BindingType getDescriptorSetDescriptorRangeType(SlangInt setIndex, SlangInt rangeIndex);
        ParameterCategory getDescriptorSetDescriptorRangeCategory(SlangInt setIndex, SlangInt rangeIndex);
        SlangInt getSubObjectRangeCount();
        SlangInt getSubObjectRangeBindingRangeIndex(SlangInt subObjectRangeIndex);
        SlangInt getSubObjectRangeSpaceOffset(SlangInt subObjectRangeIndex);
        VariableLayoutReflection* getSubObjectRangeOffset(SlangInt subObjectRangeIndex);

	private:
		slang::TypeLayoutReflection* m_native;
	};
}

