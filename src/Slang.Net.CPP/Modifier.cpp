// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Modifier.h"

namespace Slang::Cpp
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
		SlangNative::Modifier_Release(m_NativeModifier);
        m_NativeModifier = nullptr;
    }

    void* Modifier::getNative()
    {
        return m_NativeModifier;
    }
    void* Modifier::slangPtr()
    {
        return SlangNative::GenericReflection_GetNative(m_NativeModifier);
    }
}
