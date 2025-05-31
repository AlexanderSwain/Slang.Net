#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "EntryPointCLI.h"
#include <array>
#include <iostream>
#include <string>

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
		ProgramCLI(EntryPointCLI* parent);

		// Destructor
		~ProgramCLI();

		//Properties
		slang::IComponentType* getNative();
		slang::IComponentType* getLinked();
		EntryPointCLI* getEntryPoint();

		SlangResult GetCompiled(const char** output);
	private:
		EntryPointCLI* m_entryPoint = nullptr;
		slang::IComponentType* m_program = nullptr;
		slang::IComponentType* m_linkedProgram = nullptr;
		std::string m_errorBuffer; // Buffer to store error messages
	};
}