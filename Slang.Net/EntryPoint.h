#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Module.h"
#include "../Native/SlangNative.h"
#include "ParameterInfo.h"

namespace Slang
{
    public ref class EntryPoint : public System::IDisposable
    {
    public:
        // Constructor with parameters (example)
        EntryPoint(Module^ parent, System::String^ entryPointName);

        // Destructor (this automatically implements IDisposable::Dispose in C++/CLI)
        ~EntryPoint();

        // Finalizer
        !EntryPoint();

        property array<ParameterInfo^>^ Parameters
        {
            array<ParameterInfo^>^ get() { return getParameters(); }
		}

        void* getNative();

    private:
        void* m_NativeEntryPoint;
        array<ParameterInfo^>^ Slang::EntryPoint::getParameters();
    };
}