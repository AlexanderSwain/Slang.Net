#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "TypeKind.h"
#include "ParameterCategory.h"
#include "../Native/SlangNative.h"

namespace Slang::Cpp
{
    // Forward declarations
    ref class TypeReflection;
    ref class VariableLayoutReflection;

    public ref class TypeLayoutReflection : public System::IDisposable
    {
    public:
        // Constructor
        TypeLayoutReflection(void* native);

        // Destructor
        ~TypeLayoutReflection();        // Finalizer
        !TypeLayoutReflection();

        // Properties and Methods
        property TypeReflection^ Type { TypeReflection^ get(); }
        property TypeKind Kind { TypeKind get(); }

        // Size and alignment
        System::UIntPtr GetSize(ParameterCategory category);
        System::UIntPtr GetStride(ParameterCategory category);
        int GetAlignment(ParameterCategory category);        // Field access
        property unsigned int FieldCount { unsigned int get(); }
        VariableLayoutReflection^ GetFieldByIndex(unsigned int index);
        int FindFieldIndexByName(System::String^ name);
        property VariableLayoutReflection^ ExplicitCounter { VariableLayoutReflection^ get(); }

        // Array properties
        property bool IsArray { bool get(); }
        TypeLayoutReflection^ UnwrapArray();
        property System::UIntPtr ElementCount { System::UIntPtr get(); }
        property System::UIntPtr TotalArrayElementCount { System::UIntPtr get(); }
        System::UIntPtr GetElementStride(ParameterCategory category);
        property TypeLayoutReflection^ ElementTypeLayout { TypeLayoutReflection^ get(); }
        property VariableLayoutReflection^ ElementVarLayout { VariableLayoutReflection^ get(); }
        property VariableLayoutReflection^ ContainerVarLayout { VariableLayoutReflection^ get(); }

        // Internal
        void* getNative();
        void* slangPtr();

    private:
        void* m_NativeTypeLayoutReflection;
        bool m_bOwnsNative;
    };
}
