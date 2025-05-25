#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include <string>

namespace Slang
{
    struct ShaderModel
    {
    private:
        SlangCompileTarget _Target;
        std::string _Profile;

    public:
        ShaderModel(SlangCompileTarget target, const char* profile);

        SlangCompileTarget getTarget();

        const char* getProfile();
    };
}