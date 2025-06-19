#pragma once
#include "CompilerOptionName.h"
#include "CompilerOptionValue.h"
#include <cstdint>

using namespace System;
using namespace Slang::Cpp;

namespace Slang::Cpp
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