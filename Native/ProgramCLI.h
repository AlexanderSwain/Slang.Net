#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "EntryPointCLI.h"
#include <array>
#include <iostream>

namespace Native
{
	class ProgramCLI
	{
	public:
		// Constructor with parameters (example)
		ProgramCLI(EntryPointCLI* entryPoint);

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
	};
}