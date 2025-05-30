#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "CompilerOptionCLI.h"
#include "PreprocessorMacroDescCLI.h"
#include "ShaderModelCLI.h"

namespace Native
{
	class SessionCLI
	{
	public:
		// Constructor with parameters (example)
		SessionCLI(CompilerOptionCLI* options, int optionsLength,
			PreprocessorMacroDescCLI* macros, int macrosLength,
			ShaderModelCLI* models, int modelsLength,
			char** searchPaths, int searchPathsLength);

		// Destructor
		~SessionCLI();

		slang::ISession* getNative();
		static slang::IGlobalSession* GetGlobalSession();


	private:
		Slang::ComPtr<slang::ISession> m_session;
		static slang::IGlobalSession* s_context;
	};
}