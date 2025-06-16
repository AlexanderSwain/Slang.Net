#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "../Native/SlangNative.h"

namespace Slang
{
    // Forward declarations
    ref class TypeReflection;

    public ref class TypeParameterReflection : public System::IDisposable
    {
    public:
        // Constructor
        TypeParameterReflection(void* native);

        // Destructor
        ~TypeParameterReflection();

        // Finalizer
        !TypeParameterReflection();        // Properties and Methods
        property System::String^ Name { System::String^ get(); }
        property unsigned int Index { unsigned int get(); }
        property unsigned int ConstraintCount { unsigned int get(); }
        TypeReflection^ GetConstraintByIndex(int index);

        // Internal
        void* getNative();
        void* slangPtr();

    private:
        void* m_NativeTypeParameterReflection;
        bool m_bOwnsNative;
    };
}
