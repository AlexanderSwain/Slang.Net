#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"

namespace Slang
{
    struct CompilerOption
    {
    private:
        slang::CompilerOptionName _Name;
        slang::CompilerOptionValue _Value;

    public:
        CompilerOption(slang::CompilerOptionName name, slang::CompilerOptionValue value);

        slang::CompilerOptionName getName();

        slang::CompilerOptionValue getValue();
    };
}