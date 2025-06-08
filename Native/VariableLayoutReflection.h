#pragma once
#include "VariableReflection.h"
#include "TypeLayoutReflection.h"
#include "Modifier.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	// This type is empty in slang.h for some reason
	struct SLANGNATIVE_API VariableLayoutReflection
	{

	public:
		VariableLayoutReflection(void* native);

        VariableReflection* getVariable();
        char const* getName();
        Modifier* findModifier(Modifier::ID id);
        TypeLayoutReflection* getTypeLayout();
        ParameterCategory getCategory();
        unsigned int getCategoryCount();
        ParameterCategory getCategoryByIndex(unsigned int index);
        size_t getOffset(SlangParameterCategory category);
        size_t getOffset(slang::ParameterCategory category = slang::ParameterCategory::Uniform);
        TypeReflection* getType();
        unsigned int getBindingIndex();
        unsigned int getBindingSpace();
        size_t getBindingSpace(SlangParameterCategory category);
        size_t getBindingSpace(slang::ParameterCategory category);
        SlangImageFormat getImageFormat();
        char const* getSemanticName();
        size_t getSemanticIndex();
        SlangStage getStage();
        VariableLayoutReflection* getPendingDataLayout();

	private:
		slang::VariableLayoutReflection* m_native;
	};
}

