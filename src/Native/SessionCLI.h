#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "CompilerOptionCLI.h"
#include "PreprocessorMacroDescCLI.h"
#include "TargetCLI.h"
#include <map>

// Forward declaration to avoid circular dependency
namespace Native
{
	class ModuleCLI;
}

namespace Native
{
	class SessionCLI
	{
	public:

		// Constructor with parameters (example)
		SessionCLI(
			CompilerOptionCLI* options, int optionsLength,
			PreprocessorMacroDescCLI* macros, int macrosLength,
			TargetCLI* models, int modelsLength,
			char** searchPaths, int searchPathsLength);

		// Destructor
		~SessionCLI();

		slang::ISession* getNative();
		static slang::IGlobalSession* GetGlobalSession();

		unsigned int getModuleCount();
		ModuleCLI* getModuleByIndex(unsigned index);

		static slang::IGlobalSession* s_context;
		static bool s_isEnableGlsl;


	private:
		Slang::ComPtr<slang::ISession> m_session;
		std::map<unsigned int, ModuleCLI*> m_modules;
	};
}