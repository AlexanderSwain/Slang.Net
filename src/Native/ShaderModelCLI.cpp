#include "ShaderModelCLI.h"

Native::CompileTargetCLI _Target;
std::string _Profile;

Native::ShaderModelCLI::ShaderModelCLI()
	: _Target(Native::CompileTargetCLI::SLANG_TARGET_UNKNOWN2)
	, _Profile(nullptr)
{
}

Native::ShaderModelCLI::ShaderModelCLI(Native::CompileTargetCLI target, const char* profile)
	: _Target(target)
	, _Profile(profile)
{
}
 
Native::CompileTargetCLI Native::ShaderModelCLI::getTarget()
{
	return _Target;
}

const char* Native::ShaderModelCLI::getProfile()
{
	return _Profile;
}