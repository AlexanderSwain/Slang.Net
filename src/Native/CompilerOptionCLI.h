#pragma once
#include "CompilerOptionNameCLI.h"
#include "CompilerOptionValueCLI.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	struct SLANGNATIVE_API CompilerOptionCLI
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

