#pragma once

// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "ParameterCategory.h"
#include "../Native/SlangNative.h"

namespace Slang
{
    // Forward declarations
    ref class VariableReflection;
    ref class TypeLayoutReflection;
    ref class TypeReflection;
    ref class Modifier;

    public enum class ImageFormat
    {
        Unknown = 0,
        R32G32B32A32_TYPELESS = 1,
        R32G32B32A32_FLOAT = 2,
        R32G32B32A32_UINT = 3,
        R32G32B32A32_SINT = 4,
        R32G32B32_TYPELESS = 5,
        R32G32B32_FLOAT = 6,
        R32G32B32_UINT = 7,
        R32G32B32_SINT = 8,
        R16G16B16A16_TYPELESS = 9,
        R16G16B16A16_FLOAT = 10,
        R16G16B16A16_UNORM = 11,
        R16G16B16A16_UINT = 12,
        R16G16B16A16_SNORM = 13,
        R16G16B16A16_SINT = 14,
        R32G32_TYPELESS = 15,
        R32G32_FLOAT = 16,
        R32G32_UINT = 17,
        R32G32_SINT = 18,
        R10G10B10A2_TYPELESS = 19,
        R10G10B10A2_UNORM = 20,
        R10G10B10A2_UINT = 21,
        R11G11B10_FLOAT = 22,
        R8G8B8A8_TYPELESS = 23,
        R8G8B8A8_UNORM = 24,
        R8G8B8A8_UNORM_SRGB = 25,
        R8G8B8A8_UINT = 26,
        R8G8B8A8_SNORM = 27,
        R8G8B8A8_SINT = 28,
        R16G16_TYPELESS = 29,
        R16G16_FLOAT = 30,
        R16G16_UNORM = 31,
        R16G16_UINT = 32,
        R16G16_SNORM = 33,
        R16G16_SINT = 34,
        R32_TYPELESS = 35,
        D32_FLOAT = 36,
        R32_FLOAT = 37,
        R32_UINT = 38,
        R32_SINT = 39,
        R8G8_TYPELESS = 40,
        R8G8_UNORM = 41,
        R8G8_UINT = 42,
        R8G8_SNORM = 43,
        R8G8_SINT = 44,
        R16_TYPELESS = 45,
        R16_FLOAT = 46,
        D16_UNORM = 47,
        R16_UNORM = 48,
        R16_UINT = 49,
        R16_SNORM = 50,
        R16_SINT = 51,
        R8_TYPELESS = 52,
        R8_UNORM = 53,
        R8_UINT = 54,
        R8_SNORM = 55,
        R8_SINT = 56,
    };

    public ref class VariableLayoutReflection : public System::IDisposable
    {
    public:
        // Constructor
        VariableLayoutReflection(void* native);

        // Destructor
        ~VariableLayoutReflection();

        // Finalizer
        !VariableLayoutReflection();        // Properties and Methods
        property VariableReflection^ Variable { VariableReflection^ get(); }
        property System::String^ Name { System::String^ get(); }
        Modifier^ FindModifier(int id);
        property TypeLayoutReflection^ TypeLayout { TypeLayoutReflection^ get(); }

        // Categories
        property ParameterCategory Category { ParameterCategory get(); }
        property unsigned int CategoryCount { unsigned int get(); }
        ParameterCategory GetCategoryByIndex(unsigned int index);

        // Offsets and binding
        System::UIntPtr GetOffset(ParameterCategory category);
        property TypeReflection^ Type { TypeReflection^ get(); }
        property unsigned int BindingIndex { unsigned int get(); }
        property unsigned int BindingSpace { unsigned int get(); }
        System::UIntPtr GetBindingSpace(ParameterCategory category);

        // Image format
        property ImageFormat ImageFormat { Slang::ImageFormat get(); }

        // Semantics
        property System::String^ SemanticName { System::String^ get(); }
        property System::UIntPtr SemanticIndex { System::UIntPtr get(); }

        // Stage and pending data
        property unsigned int Stage { unsigned int get(); }
        property VariableLayoutReflection^ PendingDataLayout { VariableLayoutReflection^ get(); }

        // Internal
        void* getNative();
        void* slangPtr();

    private:
        void* m_NativeVariableLayoutReflection;
        bool m_bOwnsNative;
    };
}
