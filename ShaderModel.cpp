#include "ShaderModel.h"

Slang::ShaderModel::ShaderModel(SlangCompileTarget target, String^ profile)
	: _Target(target)
	, _Profile(profile)
{
}

SlangCompileTarget Slang::ShaderModel::getTarget()
{
	return _Target;
}

String^ Slang::ShaderModel::getProfile()
{
	return _Profile;
}