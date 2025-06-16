#pragma once

#include <msclr/marshal.h>

namespace Slang
{
    public ref class StringUtilities
    {
    public:
        /// <summary>
        /// Converts a managed System::String to a native const char*
        /// </summary>
        /// <param name="str">The managed string to convert</param>
        /// <returns>A const char* pointer to the native string, or nullptr if input is null</returns>
        static const char* FromString(System::String^ str);

        /// <summary>
        /// Converts a native const char* to a managed System::String
        /// </summary>
        /// <param name="str">The native string to convert</param>
        /// <returns>A managed System::String, or nullptr if input is null</returns>
        static System::String^ ToString(const char* str);
    };
}
