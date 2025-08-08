#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "ParameterCategory.h"
#include "ImageFormat.h"
#include "Modifier.h"
#include <map>

// Forward declarations
namespace Native
{
    struct VariableReflection;
    struct TypeLayoutReflection;
    struct TypeReflection;
}

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	struct SLANGNATIVE_API VariableLayoutReflection
	{

	public:
		VariableLayoutReflection(void* native);
        ~VariableLayoutReflection();

        slang::VariableLayoutReflection* getNative();

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
        ImageFormat getImageFormat();
        char const* getSemanticName();
        size_t getSemanticIndex();
        SlangStage getStage();
        VariableLayoutReflection* getPendingDataLayout();

	private:
		slang::VariableLayoutReflection* m_native;
	};
}

