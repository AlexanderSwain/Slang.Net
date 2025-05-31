// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Program.h"

namespace Slang
{
    // Constructor with parameters implementation
    Slang::Program::Program(EntryPoint^ parent)
    {
        void* nativeParent = parent->getNative();

        m_NativeProgram = SlangNative::CreateProgram(nativeParent);
    }

    // Destructor implementation (this implements IDisposable::Dispose automatically in C++/CLI)
    Slang::Program::~Program()
    {
        this->!Program();
        System::GC::SuppressFinalize(this);
    }

    // Finalizer implementation
    Slang::Program::!Program()
    {
        // Clean up resources if needed
        if (m_NativeProgram != nullptr)
        {
            // The native module should be cleaned up by the native library
            // Don't delete the pointer directly as it's managed by the native code
            m_NativeProgram = nullptr;
        }
    }

    void* Slang::Program::getNative()
    {
        return m_NativeProgram;
    }

    System::String^ Slang::Program::Compile()
    {
        const char* result = nullptr;
        int32_t compileResult = SlangNative::Compile(m_NativeProgram, &result);

        if (compileResult < 0)
        {
            System::String^ errorMsg = result ? gcnew System::String(result) : "Unknown compilation error.";
            throw gcnew System::Exception(errorMsg);
        }

        return gcnew System::String(result);
    }
}
