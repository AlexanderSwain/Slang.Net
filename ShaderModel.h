#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"

using namespace System;

namespace Slang
{
    public ref class ShaderModel
    {
    private:
        SlangCompileTarget _Target;
        String^ _Profile;

    public:
        ShaderModel(SlangCompileTarget target, String^ profile);

        SlangCompileTarget getTarget();

        String^ getProfile();
    };
}