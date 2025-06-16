#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "../Native/SlangNative.h"

namespace Slang::Cpp
{
    // Forward declarations
    ref class TypeReflection;
    ref class VariableReflection;
    ref class Attribute;
    ref class GenericReflection;
    ref class Modifier;

    public ref class FunctionReflection : public System::IDisposable
    {
    public:
        // Constructor
        FunctionReflection(void* native);

        // Destructor
        ~FunctionReflection();

        // Finalizer
        !FunctionReflection();        // Properties and Methods
        property System::String^ Name { System::String^ get(); }
        property TypeReflection^ ReturnType { TypeReflection^ get(); }

        // Parameters
        property unsigned int ParameterCount { unsigned int get(); }
        VariableReflection^ GetParameterByIndex(unsigned int index);

        // Attributes
        property unsigned int UserAttributeCount { unsigned int get(); }
        Attribute^ GetUserAttributeByIndex(unsigned int index);
        Attribute^ FindAttributeByName(System::String^ name);
        Attribute^ FindUserAttributeByName(System::String^ name);

        // Modifiers
        Modifier^ FindModifier(int id);

        // Generic operations
        property GenericReflection^ GenericContainer { GenericReflection^ get(); }
        FunctionReflection^ ApplySpecializations(GenericReflection^ genRef);
        FunctionReflection^ SpecializeWithArgTypes(array<TypeReflection^>^ types);

        // Overloading
        property bool IsOverloaded { bool get(); }
        property unsigned int OverloadCount { unsigned int get(); }
        FunctionReflection^ GetOverload(unsigned int index);

        // Internal
        void* getNative();
        void* slangPtr();

    private:
        void* m_NativeFunctionReflection;
        bool m_bOwnsNative;
    };
}
