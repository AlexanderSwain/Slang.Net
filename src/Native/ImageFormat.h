#pragma once

namespace Native
{
    enum ImageFormat
    {
#define SLANG_FORMAT(NAME, DESC) SLANG_IMAGE_FORMAT_##NAME,
#include "slang-image-format-defs.h"
#undef SLANG_FORMAT
    };
}