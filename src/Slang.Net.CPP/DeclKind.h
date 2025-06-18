#pragma once

namespace Slang::Cpp
{
    public enum class DeclKind
    {
        Unsupported = 0,
        Struct = 1,
        Function = 2,
        Module = 3,
        Generic = 4,
        Variable = 5,
        Namespace = 6
    };
}
