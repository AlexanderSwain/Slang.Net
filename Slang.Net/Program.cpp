// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Program.h"
#include "ShaderReflection.h"

namespace Slang
{
    // Constructor with parameters implementation
    Slang::Program::Program(Module^ parent)
    {
        void* nativeParent = parent->getNative();

        m_NativeProgram = SlangNative::CreateProgram(nativeParent);
    }

	// Constructor with native pointer implementation
    Slang::Program::Program(void* nativeProgram)
    {
        if (nativeProgram == nullptr)
        {
            throw gcnew System::ArgumentNullException("nativeProgram", "Native program pointer cannot be null.");
        }
        m_NativeProgram = nativeProgram;
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

    System::String^ Slang::Program::Compile(unsigned int entryPointIndex, unsigned int targetIndex)
    {
        const char* result = nullptr;
        int32_t compileResult = SlangNative::Compile(m_NativeProgram, entryPointIndex, targetIndex, &result);

        if (compileResult < 0)
        {
            System::String^ errorMsg = result ? gcnew System::String(result) : "Unknown compilation error.";
            throw gcnew System::Exception(errorMsg);
        }

        return gcnew System::String(result);
    }

    ShaderReflection^ Slang::Program::GetReflection()
    {
        if (!m_NativeProgram) return nullptr;
        
        // Get reflection from the native program
        void* nativeReflection = SlangNative::GetProgramReflection(m_NativeProgram);
        return nativeReflection ? gcnew ShaderReflection(nativeReflection) : nullptr;
    }
}
