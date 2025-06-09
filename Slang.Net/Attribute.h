#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "../Native/SlangNative.h"
#include "Attribute.h"
#include "StringUtils.h"
#include "TypeDef.h"
#include <msclr/marshal.h>

namespace Slang
{
    public ref class Attribute : public System::IDisposable
    {
    public:
        // Constructor
        Attribute(void* native);

        // Destructor
        ~Attribute();

        // Finalizer
        !Attribute();        // Properties and Methods
        property System::String^ Name { System::String^ get(); }
        property unsigned int ArgumentCount { unsigned int get(); }
        System::String^ GetArgumentValueString(unsigned int index);
        System::Nullable<System::Int32> GetArgumentValueInt(unsigned int index);
        System::Nullable<System::Single> GetArgumentValueFloat(unsigned int index);

        // Internal
        void* getNative();

    private:
        void* m_NativeAttribute;
        bool m_bOwnsNative;
    };
}
