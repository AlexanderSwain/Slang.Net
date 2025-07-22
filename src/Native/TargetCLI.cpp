#include "TargetCLI.h"

Native::CompileTargetCLI _Target;
std::string _Profile;

Native::TargetCLI::TargetCLI()
	: _Target(Native::CompileTargetCLI::SLANG_TARGET_UNKNOWN2)
	, _Profile(nullptr)
{
}

Native::TargetCLI::TargetCLI(Native::CompileTargetCLI target, const char* profile)
	: _Target(target)
	, _Profile(profile)
{
}
 
Native::CompileTargetCLI Native::TargetCLI::getTarget()
{
	return _Target;
}

const char* Native::TargetCLI::getProfile()
{
	return _Profile;
}