#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include <stdexcept>

namespace Native
{
    // Forward declarations
    struct ShaderReflection;
    struct FunctionReflection;
    struct VariableLayoutReflection;
    struct TypeLayoutReflection;
}

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	// This type is empty in slang.h for some reason
	struct SLANGNATIVE_API EntryPointReflection
	{
	public:
        EntryPointReflection(void* parent, void* native);

        ShaderReflection* getParent();
        char const* getName();
        char const* getNameOverride();
        unsigned getParameterCount();
        FunctionReflection* getFunction();
        VariableLayoutReflection* getParameterByIndex(unsigned index);
        SlangStage getStage();
        void getComputeThreadGroupSize(SlangUInt axisCount, SlangUInt* outSizeAlongAxis);
        void getComputeWaveSize(SlangUInt* outWaveSize);
        bool usesAnySampleRateInput();
        VariableLayoutReflection* getVarLayout();
        TypeLayoutReflection* getTypeLayout();        VariableLayoutReflection* getResultVarLayout();
        bool hasDefaultConstantBuffer();
        void* getNative();

	private:
        slang::ShaderReflection* m_parent;
		slang::EntryPointReflection* m_native;
	};
}