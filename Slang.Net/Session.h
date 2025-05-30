#pragma once
#include "CompilerOption.h"
#include "PreprocessorMacroDesc.h"
#include "ShaderModel.h"
#include "../Native/SlangNative.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace Slang
{
    public ref class Session
    {
    public:
        // Constructor with parameters (example)
        Session(array<Slang::CompilerOption^>^ options,
            array<Slang::PreprocessorMacroDesc^>^ macros,
            array<Slang::ShaderModel^>^ models,
            array<String^>^ searchPaths);

        // Destructor
        ~Session();

        void* getNative();

    private:
        void* m_NativeSession;
    };
}
