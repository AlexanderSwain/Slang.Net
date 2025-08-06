#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "ModuleCLI.h"
#include "EntryPointCLI.h"
#include <array>
#include <iostream>
#include <string>

// Reflection Includes
#include "LayoutRules.h"
#include "TypeDef.h"
#include "GenericArgType.h"
#include "GenericArgReflection.h"

namespace Native
{
    // Forward declarations
    struct TypeParameterReflection;
    struct TypeReflection;
    struct TypeLayoutReflection;
    struct FunctionReflection;
    struct VariableLayoutReflection;
    struct VariableReflection;
    struct EntryPointReflection;
    struct GenericReflection;
}

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	class SLANGNATIVE_API ProgramCLI
	{
	public:
		// Constructor with parameters (example)
		ProgramCLI(ModuleCLI* parent);

		// Destructor
		~ProgramCLI();

		//Properties
		slang::IComponentType* getNative();
		ModuleCLI* getParent();
		SlangResult GetCompiled(unsigned int entryPointIndex, unsigned int targetIndex, const void** output, int* outputSize);
		SlangResult GetCompiled(unsigned int targetIndex, const void** output, int* outputSize);
		void* GetLayout(int targetIndex);

	private:
		ModuleCLI* m_parent = nullptr;
		Slang::ComPtr<slang::IComponentType> m_composedProgram = nullptr;
		slang::IComponentType** getProgramComponents();
	};
}