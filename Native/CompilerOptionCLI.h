#pragma once
#include "CompilerOptionNameCLI.h"
#include "CompilerOptionValueCLI.h"

namespace Native
{
	struct CompilerOptionCLI
	{
	private:
		CompilerOptionNameCLI _Name;
		CompilerOptionValueCLI _Value;

	public:
		CompilerOptionCLI();
		CompilerOptionCLI(CompilerOptionNameCLI name, CompilerOptionValueCLI value);

		CompilerOptionNameCLI getName();
		CompilerOptionValueCLI getValue();
	};
}

