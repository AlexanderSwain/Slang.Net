#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "CompilerOption.h"
#include "PreprocessorMacroDesc.h"
#include "ShaderModel.h"
#include "../Native/SlangNative.h"

// Removed the "using namespace System::Runtime::InteropServices;" that was causing conflicts

namespace Slang::Cpp
{
    public ref class Session
    {
    public:
        // Constructor with parameters (example)
        Session(array<Slang::Cpp::CompilerOption^>^ options,
            array<Slang::Cpp::PreprocessorMacroDesc^>^ macros,
            array<Slang::Cpp::ShaderModel^>^ models,
            array<System::String^>^ searchPaths);

        // Destructor
        ~Session();

        void* getNative();

    private:
        void* m_NativeSession;
    };
}
