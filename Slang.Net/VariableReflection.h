#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "../Native/SlangNative.h"
#include "VariableReflection.h"
#include "StringUtils.h"
#include "TypeReflection.h"
#include "Attribute.h"
#include "GenericReflection.h"
#include "Modifier.h"
#include "TypeDef.h"
#include <msclr/marshal.h>

namespace Slang
{    // Forward declarations
    ref class TypeReflection;
    ref class Attribute;
    ref class GenericReflection;
    ref class Modifier;
    ref class FunctionReflection;

    public ref class VariableReflection : public System::IDisposable
    {
    public:
        // Constructor
        VariableReflection(void* native);

        // Destructor
        ~VariableReflection();

        // Finalizer
        !VariableReflection();        // Properties and Methods
        property System::String^ Name { System::String^ get(); }
        property TypeReflection^ Type { TypeReflection^ get(); }

        // Modifiers
        Modifier^ FindModifier(int id);

        // Attributes
        property unsigned int UserAttributeCount { unsigned int get(); }
        Attribute^ GetUserAttributeByIndex(unsigned int index);
        Attribute^ FindAttributeByName(System::String^ name);
        Attribute^ FindUserAttributeByName(System::String^ name);

        // Default values
        property bool HasDefaultValue { bool get(); }
        System::Nullable<System::Int64> GetDefaultValueInt();

        // Generic operations
        property GenericReflection^ GenericContainer { GenericReflection^ get(); }
        VariableReflection^ ApplySpecializations(GenericReflection^ genRef);

        // Internal
        void* getNative();
        void* slangPtr();

    private:
        void* m_NativeVariableReflection;
        bool m_bOwnsNative;
    };
}
