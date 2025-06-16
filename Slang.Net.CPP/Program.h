#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Module.h"
#include "../Native/SlangNative.h"

namespace Slang
{
    // Forward declarations
    ref class ShaderReflection;

    public ref class Program : public System::IDisposable
    {
    public:
        // Constructor with parameters (example)
        Program(Module^ parent);

		// Constructor with native pointer (for internal use)
		Program(void* nativeProgram);

        // Destructor (this automatically implements IDisposable::Dispose in C++/CLI)
        ~Program();

        // Finalizer
        !Program();

        void* getNative();

        System::String^ Compile(unsigned int entryPointIndex, unsigned int targetIndex);
        
        // Reflection
        ShaderReflection^ GetReflection();

    private:
        void* m_NativeProgram;
    };
}