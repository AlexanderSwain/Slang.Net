#include "ShaderModel.h"

Slang::Cpp::ShaderModel::ShaderModel(CompileTarget target, String^ profile)
	: _Target(target)
	, _Profile(profile)
{
}

CompileTarget Slang::Cpp::ShaderModel::getTarget()
{
	return _Target;
}

String^ Slang::Cpp::ShaderModel::getProfile()
{
	return _Profile;
}
