#include "ShaderModel.h"

SlangCompileTarget _Target;
std::string _Profile;

Slang::ShaderModel::ShaderModel(SlangCompileTarget target, const char* profile)
	: _Target(target)
	, _Profile(profile)
{
}

SlangCompileTarget Slang::ShaderModel::getTarget()
{
	return _Target;
}

const char* Slang::ShaderModel::getProfile()
{
	return _Profile.c_str();
}