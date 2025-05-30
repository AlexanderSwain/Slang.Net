#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "SessionCLI.h"
#include "EntryPointCLI.h"

namespace Native
{
	class ModuleCLI
	{
	public:
		// Constructor with parameters (example)
		ModuleCLI(slang::ISession* parent, const char* moduleName, const char* modulePath, const char* shaderSource);

		// Destructor
		~ModuleCLI();

		slang::IModule* getNative();

	private:
		slang::ISession* m_parent;
		slang::IModule* m_slangModule;
	};
}

