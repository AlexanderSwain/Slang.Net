// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Module.h"
#include "StringUtils.h"
#include <msclr/marshal.h>

namespace Slang::Cpp
{

    static void ThrowErrorMessage(const char* errorMessage)
    {
        // If an error message is provided, throw an exception with that message
        if (errorMessage != nullptr)
        {
            System::String^ errorStr = gcnew System::String(errorMessage);
            throw gcnew System::ArgumentException(errorStr);
        }
        else
        {
            throw gcnew System::Exception("There was a problem generating an error message.");
        }
    }    // Constructor with parameters implementation
    Slang::Cpp::Module::Module(Session^ parent, System::String^ moduleName, System::String^ modulePath, System::String^ shaderSource)
    {
        void* nativeParent = parent->getNative();
        const char* name = Slang::Cpp::StringUtilities::FromString(moduleName);
        const char* path = Slang::Cpp::StringUtilities::FromString(modulePath);
        const char* source = Slang::Cpp::StringUtilities::FromString(shaderSource);

        m_NativeModule = SlangNative::CreateModule(nativeParent, name, path, source);
        const char* errorMessage = SlangNative::SlangNative_GetLastError();

        if (!m_NativeModule)
            ThrowErrorMessage(errorMessage);
    }

    // Destructor implementation (this implements IDisposable::Dispose automatically in C++/CLI)
    Slang::Cpp::Module::~Module()
    {
        this->!Module();
        System::GC::SuppressFinalize(this);
    }

    // Finalizer implementation
    Slang::Cpp::Module::!Module()
    {
        // Clean up resources if needed
        if (m_NativeModule != nullptr)
        {
            // The native module should be cleaned up by the native library
            // Don't delete the pointer directly as it's managed by the native code
            m_NativeModule = nullptr;
        }
    }

    void* Slang::Cpp::Module::getNative()
    {
        return m_NativeModule;
    }
}
