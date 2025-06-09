#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "DeclKind.h"
#include "../Native/SlangNative.h"

namespace Slang
{
    // Forward declarations
    ref class VariableReflection;
    ref class TypeReflection;

    public ref class GenericReflection : public System::IDisposable
    {
    public:
        // Constructor
        GenericReflection(void* native);

        // Destructor
        ~GenericReflection();        // Finalizer
        !GenericReflection();

        // Properties and Methods
        property System::String^ Name { System::String^ get(); }

        // Type parameters
        property unsigned int TypeParameterCount { unsigned int get(); }
        VariableReflection^ GetTypeParameter(unsigned int index);

        // Value parameters
        property unsigned int ValueParameterCount { unsigned int get(); }
        VariableReflection^ GetValueParameter(unsigned int index);

        // Type parameter constraints
        unsigned int GetTypeParameterConstraintCount(VariableReflection^ typeParam);
        TypeReflection^ GetTypeParameterConstraintType(VariableReflection^ typeParam, unsigned int index);

        // Container operations
        property DeclKind InnerKind { DeclKind get(); }
        property GenericReflection^ OuterGenericContainer { GenericReflection^ get(); }

        // Specialization operations
        TypeReflection^ GetConcreteType(VariableReflection^ typeParam);
        System::Nullable<System::Int64> GetConcreteIntVal(VariableReflection^ valueParam);
        GenericReflection^ ApplySpecializations(GenericReflection^ genRef);

        // Internal
        void* getNative();

    private:
        void* m_NativeGenericReflection;
        bool m_bOwnsNative;
    };
}
