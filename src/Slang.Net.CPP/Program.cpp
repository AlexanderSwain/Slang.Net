// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Program.h"
#include "ShaderReflection.h"

namespace Slang::Cpp
{
    // Constructor with parameters implementation
    Slang::Cpp::Program::Program(Module^ parent)
    {
        void* nativeParent = parent->getNative();

        m_NativeProgram = SlangNative::CreateProgram(nativeParent);
        const char* errorMessage = SlangNative::SlangNative_GetLastError();

        if (!m_NativeProgram)
        {
            System::String^ errorMsg = errorMessage ? gcnew System::String(errorMessage) : "Unknown compilation error.";
            throw gcnew System::Exception(errorMsg);
        }
    }

	// Constructor with native pointer implementation
    Slang::Cpp::Program::Program(void* nativeProgram)
    {
        if (nativeProgram == nullptr)
        {
            throw gcnew System::ArgumentNullException("nativeProgram", "Native program pointer cannot be null.");
        }
        m_NativeProgram = nativeProgram;
	}

    // Destructor implementation (this implements IDisposable::Dispose automatically in C++/CLI)
    Slang::Cpp::Program::~Program()
    {
        this->!Program();
        System::GC::SuppressFinalize(this);
    }

    // Finalizer implementation
    Slang::Cpp::Program::!Program()
    {
        // Clean up resources if needed
        if (m_NativeProgram != nullptr)
        {
            // The native module should be cleaned up by the native library
            // Don't delete the pointer directly as it's managed by the native code
            m_NativeProgram = nullptr;
        }
    }

    void* Slang::Cpp::Program::getNative()
    {
        return m_NativeProgram;
    }

    System::String^ Slang::Cpp::Program::Compile(unsigned int entryPointIndex, unsigned int targetIndex)
    {
        const char* result = nullptr;
        int32_t compileResult = SlangNative::CompileProgram(m_NativeProgram, entryPointIndex, targetIndex, &result);
        const char* errorMessage = SlangNative::SlangNative_GetLastError();

        if (compileResult < 0)
        {
            System::String^ errorMsg = errorMessage ? gcnew System::String(errorMessage) : "Unknown compilation error.";
            throw gcnew System::Exception(errorMsg);
        }

        return gcnew System::String(result);
    }

    ShaderReflection^ Slang::Cpp::Program::GetReflection(unsigned int targetIndex)
    {
        if (!m_NativeProgram) return nullptr;
        
        // Get reflection from the native program
        void* nativeReflection = SlangNative::GetProgramReflection(m_NativeProgram, targetIndex);
        const char* errorMessage = SlangNative::SlangNative_GetLastError();

        if (!nativeReflection)
        {
            System::String^ errorMsg = errorMessage ? gcnew System::String(errorMessage) : "Unknown compilation error.";
            throw gcnew System::Exception(errorMsg);
        }

        return gcnew ShaderReflection(nativeReflection);
    }
}
