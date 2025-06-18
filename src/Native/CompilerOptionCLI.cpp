#include "CompilerOptionCLI.h"

Native::CompilerOptionCLI::CompilerOptionCLI()
	: _Name((Native::CompilerOptionNameCLI)0)
	, _Value({ Native::CompilerOptionValueKindCLI::CompilerOptionValueKindCLI_Int, 0, 0, nullptr, nullptr })
{
}

Native::CompilerOptionCLI::CompilerOptionCLI(Native::CompilerOptionNameCLI name, Native::CompilerOptionValueCLI value)
	: _Name(name)
	, _Value(value)
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