#include "ShaderModel.h"

ShaderModel::ShaderModel(CompileTarget target, String^ profile)
	: _Target(target)
	, _Profile(profile)
{
}

CompileTarget ShaderModel::getTarget()
{
	return _Target;
}

String^ ShaderModel::getProfile()
{
	return _Profile;
}
