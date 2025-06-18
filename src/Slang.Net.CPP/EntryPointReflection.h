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
    ref class FunctionReflection;
    ref class VariableLayoutReflection;
    ref class TypeLayoutReflection;
    ref class ShaderReflection;

    public enum class SlangStage
    {
        None = 0,
        Vertex = 1,
        Hull = 2,
        Domain = 3,
        Geometry = 4,
        Fragment = 5,
        Compute = 6,
        RayGeneration = 7,
        Intersection = 8,
        AnyHit = 9,
        ClosestHit = 10,
        Miss = 11,
        Callable = 12,
        Mesh = 13,
        Amplification = 14,
        Pixel = 5,  // Alias for Fragment
    };

    public ref class EntryPointReflection : public System::IDisposable
    {
    public:
        // Constructor
        EntryPointReflection(void* native);

        // Destructor
        ~EntryPointReflection();

        // Finalizer
        !EntryPointReflection();

        // Properties and Methods
        property ShaderReflection^ Parent { ShaderReflection^ get(); }

        property System::String^ Name { System::String^ get(); }
        property System::String^ NameOverride { System::String^ get(); }
        
        // Parameters
        property unsigned int ParameterCount { unsigned int get(); }
        VariableLayoutReflection^ GetParameterByIndex(unsigned int index);

        // Function information
        property FunctionReflection^ Function { FunctionReflection^ get(); }
        property SlangStage Stage { SlangStage get(); }

        // Compute-specific properties
        array<System::UInt32>^ GetComputeThreadGroupSize();
        property System::Nullable<System::UInt32> ComputeWaveSize { System::Nullable<System::UInt32> get(); }
        property bool UsesAnySampleRateInput { bool get(); }

        // Layout information
        property VariableLayoutReflection^ VarLayout { VariableLayoutReflection^ get(); }
        property TypeLayoutReflection^ TypeLayout { TypeLayoutReflection^ get(); }
        property VariableLayoutReflection^ ResultVarLayout { VariableLayoutReflection^ get(); }        // Buffer information
        property bool HasDefaultConstantBuffer { bool get(); }

        // Equality operators
        virtual bool Equals(System::Object^ obj) override;
        virtual bool Equals(EntryPointReflection^ other);
        static bool operator==(EntryPointReflection^ left, EntryPointReflection^ right);
        static bool operator!=(EntryPointReflection^ left, EntryPointReflection^ right);
        virtual int GetHashCode() override;

        // Internal
        void* getNative();
        void* slangPtr();

    private:
        void* m_NativeEntryPointReflection;
        bool m_bOwnsNative;
    };
}
