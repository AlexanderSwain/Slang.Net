#include "StringUtils.h"

const char* Slang::Cpp::StringUtilities::FromString(System::String^ str)
{
    if (str == nullptr)
        return nullptr;
    System::IntPtr strPtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(str);
    const char* nativeStr = static_cast<const char*>(strPtr.ToPointer());
    return nativeStr;
}

System::String^ Slang::Cpp::StringUtilities::ToString(const char* str)
{
    if (str == nullptr)
        return nullptr;

    return gcnew System::String(str);
}
