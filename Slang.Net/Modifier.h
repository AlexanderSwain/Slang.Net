#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "../Native/SlangNative.h"

namespace Slang
{
    public ref class Modifier : public System::IDisposable
    {
    public:
        // Constructor
        Modifier(void* native);

        // Destructor
        ~Modifier();

        // Finalizer
        !Modifier();

        // Internal
        void* getNative();

    private:
        void* m_NativeModifier;
        bool m_bOwnsNative;
    };
}
