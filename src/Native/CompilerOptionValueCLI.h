#pragma once
#include <cstdint>
#include "CompilerOptionValueKindCLI.h"

namespace Native
{
    struct CompilerOptionValueCLI
    {
        CompilerOptionValueKindCLI kind = CompilerOptionValueKindCLI::CompilerOptionValueKindCLI_Int;
        int32_t intValue0 = 0;
        int32_t intValue1 = 0;
        const char* stringValue0 = nullptr;
        const char* stringValue1 = nullptr;
    };
}