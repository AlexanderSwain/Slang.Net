#include "StringUtils.h"

namespace Slang::Cpp
{
    namespace StringUtilities
    {
        const char* FromString(System::String^ str)
        {
            if (str == nullptr)
                return nullptr;
            System::IntPtr strPtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(str);
            const char* nativeStr = static_cast<const char*>(strPtr.ToPointer());
            return nativeStr;
        }

        System::String^ ToString(const char* str)
        {
            if (str == nullptr)
                return nullptr;
            return gcnew System::String(str);
        }
    }
}
