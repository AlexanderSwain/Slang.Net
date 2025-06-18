#pragma once
#include <slang.h>
 
namespace Native
{
    enum class LayoutRules
    {
        Default = SLANG_LAYOUT_RULES_DEFAULT,
        MetalArgumentBufferTier2 = SLANG_LAYOUT_RULES_METAL_ARGUMENT_BUFFER_TIER_2,
    };
}