#pragma once
#include "CompileTarget.h"

using namespace System;
using namespace Slang::Cpp;

namespace Slang::Cpp
{
    public ref class ShaderModel
    {
    private:
        CompileTarget _Target;
        String^ _Profile;

    public:
        ShaderModel(CompileTarget target, String^ profile);

        CompileTarget getTarget();

        String^ getProfile();
    };
}