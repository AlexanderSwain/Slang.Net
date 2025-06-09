// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Modifier.h"

namespace Slang
{
    // Constructor
    Modifier::Modifier(void* native)
    {
        m_NativeModifier = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    Modifier::~Modifier()
    {
        this->!Modifier();
    }

    // Finalizer
    Modifier::!Modifier()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeModifier = nullptr;
    }

    void* Modifier::getNative()
    {
        return m_NativeModifier;
    }
}
