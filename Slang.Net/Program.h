#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "EntryPoint.h"
#include "../Native/SlangNative.h"

namespace Slang
{
    public ref class Program : public System::IDisposable
    {
    public:
        // Constructor with parameters (example)
        Program(EntryPoint^ parent);

        // Destructor (this automatically implements IDisposable::Dispose in C++/CLI)
        ~Program();

        // Finalizer
        !Program();

        void* getNative();

        System::String^ Compile();

    private:
        void* m_NativeProgram;
    };
}