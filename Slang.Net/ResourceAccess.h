#pragma once

namespace Slang
{
    public enum class ResourceAccess
    {
        None = 0,
        Read = 1,
        ReadWrite = 2,
        RasterOrdered = 3,
        Append = 4,
        Consume = 5,
        Write = 6,
        Feedback = 7,
        Unknown = 0x7FFFFFFF,
    };
}
