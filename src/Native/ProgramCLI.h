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
		slang::IComponentType* getLinked();
		ModuleCLI* getParent();
		SlangResult GetCompiled(unsigned int entryPointIndex, unsigned int targetIndex, const char** output);
		void* GetLayout(int targetIndex);

	private:
		ModuleCLI* m_parent = nullptr;
		slang::IComponentType* m_composedProgram = nullptr;
		slang::IComponentType* m_linkedProgram = nullptr;
        std::string m_linkedProgram_Diagnostics; // Check for memory leaks for this
		std::string m_errorBuffer; // Check to see if this is unused // Buffer to store error messages
		slang::IComponentType** getProgramComponents();
	};
}