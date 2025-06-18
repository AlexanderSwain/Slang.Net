#include "CompilerOption.h"


CompilerOption::CompilerOption(CompilerOptionName name, CompilerOptionValue^ value)
	: m_name(name),
      m_value(value)
{
}

CompilerOptionName CompilerOption::getName()
{
	return m_name;
}

CompilerOptionValue^ CompilerOption::getValue()
{
	return m_value;
}
