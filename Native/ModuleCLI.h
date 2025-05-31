#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "SessionCLI.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	class SLANGNATIVE_API ModuleCLI
	{
	public:
		// Constructor with parameters (example)
		ModuleCLI(SessionCLI* parent, const char* moduleName, const char* modulePath, const char* shaderSource);

		// Destructor
		~ModuleCLI();

		slang::IModule* getNative();

	private:
		slang::ISession* m_parent;
		slang::IModule* m_slangModule;
	};
}

