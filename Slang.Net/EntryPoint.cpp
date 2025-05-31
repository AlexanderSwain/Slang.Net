// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "EntryPoint.h"
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
    Slang::EntryPoint::EntryPoint(Module^ parent, System::String^ entryPointName)
    {
        void* nativeParent = parent->getNative();
        const char* searchName = FromString(entryPointName);

        m_NativeEntryPoint = SlangNative::FindEntryPoint(nativeParent, searchName);
    }

    // Destructor implementation (this implements IDisposable::Dispose automatically in C++/CLI)
    Slang::EntryPoint::~EntryPoint()
    {
        this->!EntryPoint();
        System::GC::SuppressFinalize(this);
    }

    // Finalizer implementation
    Slang::EntryPoint::!EntryPoint()
    {
        // Clean up resources if needed
        if (m_NativeEntryPoint != nullptr)
        {
            // The native module should be cleaned up by the native library
            // Don't delete the pointer directly as it's managed by the native code
            m_NativeEntryPoint = nullptr;
        }
    }

    void* Slang::EntryPoint::getNative()
    {
        return m_NativeEntryPoint;
    }
}
