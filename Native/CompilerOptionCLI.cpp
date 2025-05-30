#include "CompilerOptionCLI.h"

Native::CompilerOptionCLI::CompilerOptionCLI()
	: _Name((Native::CompilerOptionNameCLI)0)
	, _Value({ Native::CompilerOptionValueKindCLI::CompilerOptionValueKindCLI_Int, 0, 0, nullptr, nullptr })
{
}

Native::CompilerOptionCLI::CompilerOptionCLI(Native::CompilerOptionNameCLI name2, Native::CompilerOptionValueCLI value2)
	: _Name(name2)
	, _Value(value2)
{
}

Native::CompilerOptionNameCLI Native::CompilerOptionCLI::getName()
{
	return _Name;
}

Native::CompilerOptionValueCLI Native::CompilerOptionCLI::getValue()
{
	return _Value;
}