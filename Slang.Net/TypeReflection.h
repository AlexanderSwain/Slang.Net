#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "TypeKind.h"
#include "ScalarType.h"
#include "ResourceShape.h"
#include "ResourceAccess.h"
#include "../Native/SlangNative.h"

namespace Slang
{
    // Forward declarations
    ref class VariableReflection;
    ref class Attribute;
    ref class GenericReflection;
    ref class FunctionReflection;

    public ref class TypeReflection : public System::IDisposable
    {
    public:
        // Constructor
        TypeReflection(void* native);

        // Destructor
        ~TypeReflection();        // Finalizer
        !TypeReflection();

        // Properties and Methods
        property TypeKind Kind { TypeKind get(); }
        
        // Only useful if Kind == TypeKind::Struct
        property unsigned int FieldCount { unsigned int get(); }
        VariableReflection^ GetFieldByIndex(unsigned int index);

        // Array properties
        property bool IsArray { bool get(); }
        TypeReflection^ UnwrapArray();
        
        // Only useful if Kind == TypeKind::Array
        property System::UIntPtr ElementCount { System::UIntPtr get(); }
        property System::UIntPtr TotalArrayElementCount { System::UIntPtr get(); }
        property TypeReflection^ ElementType { TypeReflection^ get(); }

        // Matrix/Vector properties
        property unsigned int RowCount { unsigned int get(); }
        property unsigned int ColumnCount { unsigned int get(); }

        // Scalar properties
        property ScalarType ScalarType { Slang::ScalarType get(); }

        // Resource properties
        property TypeReflection^ ResourceResultType { TypeReflection^ get(); }
        property ResourceShape ResourceShape { Slang::ResourceShape get(); }
        property ResourceAccess ResourceAccess { Slang::ResourceAccess get(); }

        // Name properties
        property System::String^ Name { System::String^ get(); }
        property System::String^ FullName { System::String^ get(); }

        // Attributes
        property unsigned int UserAttributeCount { unsigned int get(); }
        Attribute^ GetUserAttributeByIndex(unsigned int index);
        Attribute^ FindAttributeByName(System::String^ name);
        Attribute^ FindUserAttributeByName(System::String^ name);

        // Generic operations
        TypeReflection^ ApplySpecializations(GenericReflection^ genRef);
        property GenericReflection^ GenericContainer { GenericReflection^ get(); }

        // Internal
        void* getNative();
        void* slangPtr();

    private:
        void* m_NativeTypeReflection;
        bool m_bOwnsNative;
    };
}
