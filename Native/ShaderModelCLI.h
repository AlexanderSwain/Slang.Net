#pragma once
#include <string>
#include "CompileTargetCLI.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	struct SLANGNATIVE_API ShaderModelCLI
	{
	private:
		CompileTargetCLI _Target;
		const char* _Profile;

	public:
		ShaderModelCLI();
		ShaderModelCLI(CompileTargetCLI target, const char* profile);

		CompileTargetCLI getTarget();

		const char* getProfile();
	};
}