#pragma once
#include <cstdint>

namespace Slang
{
    typedef int32_t SlangResult;
    typedef int64_t SlangInt;
    typedef uint64_t SlangUInt;

#define SLANG_FAILED(status) ((status) < 0)
    //! Use to test if a result succeeded. Never use result == SLANG_OK to test for success, as will
    //! detect other successful codes as a failure.
#define SLANG_SUCCEEDED(status) ((status) >= 0)

    //! Get the facility the result is associated with
#define SLANG_GET_RESULT_FACILITY(r) ((int32_t)(((r) >> 16) & 0x7fff))
    //! Get the result code for the facility
#define SLANG_GET_RESULT_CODE(r) ((int32_t)((r) & 0xffff))

#define SLANG_MAKE_ERROR(fac, code) \
    ((((int32_t)(fac)) << 16) | ((int32_t)(code)) | int32_t(0x80000000))
#define SLANG_MAKE_SUCCESS(fac, code) ((((int32_t)(fac)) << 16) | ((int32_t)(code)))
}