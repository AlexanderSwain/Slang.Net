#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "CompilerOption.h"


Slang::CompilerOption::CompilerOption(slang::CompilerOptionName name, slang::CompilerOptionValue value)
	: _Name(name)
	, _Value(value)
{
}

slang::CompilerOptionName Slang::CompilerOption::getName()
{
	return _Name;
}

slang::CompilerOptionValue Slang::CompilerOption::getValue()
{
	return _Value;
}