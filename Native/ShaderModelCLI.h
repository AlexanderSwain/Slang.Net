#pragma once
#include <string>
#include "CompileTargetCLI.h"

namespace Native
{
	struct ShaderModelCLI
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