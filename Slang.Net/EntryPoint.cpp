// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "EntryPoint.h"
#include <msclr/marshal.h>
#include <stdexcept>

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

    static void ThrowErrorMessage(const char* errorMessage)
    {
        // If an error message is provided, throw an exception with that message
        if (errorMessage != nullptr)
        {
            System::String^ errorStr = gcnew System::String(errorMessage);
            throw gcnew System::ArgumentException(errorStr);
        }        else
        {
            throw gcnew System::Exception("There was a problem generating an error message.");
        }
    }

    // Constructor with parameters implementation
    Slang::EntryPoint::EntryPoint(Module^ parent, System::String^ entryPointName)
    {
        if (parent == nullptr)
            throw gcnew System::ArgumentNullException("parent", "Parent module cannot be null.");
        if (entryPointName == nullptr)
            throw gcnew System::ArgumentNullException("entryPointName", "Entry point name cannot be null.");

        void* nativeParent = parent->getNative();
        const char* searchName = FromString(entryPointName);
		const char* errorMessage = nullptr;

        m_NativeEntryPoint = SlangNative::FindEntryPoint(nativeParent, searchName, &errorMessage);

        if (!m_NativeEntryPoint)
            ThrowErrorMessage(errorMessage);
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

    array<ParameterInfo^>^ Slang::EntryPoint::getParameters()
    {
        ParameterInfoCLI* params = nullptr;
        int paramCount = 0;
        SlangNative::GetParameterInfo(getNative(), &params, &paramCount);
        array<ParameterInfo^>^ parameterArray = gcnew array<ParameterInfo^>(paramCount);
        for (int i = 0; i < paramCount; ++i)
        {
            parameterArray[i] = gcnew ParameterInfo(gcnew String(params[i].getName()), params[i].getCategory(), params[i].getBindingIndex(), params[i].getBindingSpace());
        }
        return parameterArray;
    }

    void* Slang::EntryPoint::getNative()
    {
        return m_NativeEntryPoint;
    }
}
