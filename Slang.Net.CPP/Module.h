#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Session.h"
#include "../Native/SlangNative.h"

namespace Slang::Cpp
{
    public ref class Module : public System::IDisposable
    {
    public:
        // Constructor with parameters (example)
        Module(Session^ parent, System::String^ moduleName, System::String^ modulePath, System::String^ shaderSource);

        // Destructor (this automatically implements IDisposable::Dispose in C++/CLI)
        ~Module();

        // Finalizer
        !Module();

        void* getNative();

    private:
        void* m_NativeModule;
    };
}