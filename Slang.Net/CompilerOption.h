#pragma once
#include "CompilerOptionName.h"
#include "CompilerOptionValue.h"
#include <cstdint>

using namespace System;
using namespace Slang;

namespace Slang
{
    public ref class CompilerOption
    {
    private:
        CompilerOptionName m_name;
        CompilerOptionValue^ m_value;

    public:
        CompilerOption(CompilerOptionName name, CompilerOptionValue^ value);

        CompilerOptionName getName();
        CompilerOptionValue^ getValue();
    };
}