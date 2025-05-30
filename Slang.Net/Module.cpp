// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Module.h"
#include <msclr/marshal.h>

namespace Slang
{
    static const char* FromString(System::String^ str)
    {
        if (str == nullptr)
            return nullptr;
        System::IntPtr strPtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(str);
        const char* nativeStr = static_cast<const char*>(strPtr.ToPointer());
        return nativeStr;
    }

    // Constructor with parameters implementation
    Slang::Module::Module(Session^ parent, System::String^ moduleName, System::String^ modulePath, System::String^ shaderSource)
    {
        void* nativeParent = parent->getNative();
        const char* name = FromString(moduleName);
        const char* path = FromString(modulePath);
        const char* source = FromString(shaderSource);

        m_NativeModule = SlangNative::CreateModule(nativeParent, name, path, source);
    }

    // Destructor implementation (this implements IDisposable::Dispose automatically in C++/CLI)
    Slang::Module::~Module()
    {
        this->!Module();
        System::GC::SuppressFinalize(this);
    }

    // Finalizer implementation
    Slang::Module::!Module()
    {
        // Clean up resources if needed
        if (m_NativeModule != nullptr)
        {
            // The native module should be cleaned up by the native library
            // Don't delete the pointer directly as it's managed by the native code
            m_NativeModule = nullptr;
        }
    }

    void* Slang::Module::getNative()
    {
        return m_NativeModule;
    }
}
