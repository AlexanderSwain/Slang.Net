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
    ref class TypeLayoutReflection;
    ref class VariableLayoutReflection;
    ref class VariableReflection;
    ref class FunctionReflection;
    ref class EntryPointReflection;
    ref class GenericReflection;
    ref class TypeParameterReflection;
	ref class LayoutRules;
	ref class Program;

    public ref class ShaderReflection : public System::IDisposable
    {
    public:
        // Constructor
        ShaderReflection(void* native);

        // Destructor
        ~ShaderReflection();

        // Finalizer
        !ShaderReflection();

        property Program^ Parent{ Program ^ get(); }

        // Properties and Methods
        property unsigned int ParameterCount { unsigned int get(); }
        property unsigned int TypeParameterCount { unsigned int get(); }
        
        TypeParameterReflection^ GetTypeParameterByIndex(unsigned int index);
        TypeParameterReflection^ FindTypeParameter(System::String^ name);
        VariableLayoutReflection^ GetParameterByIndex(unsigned int index);
        
        property unsigned int EntryPointCount { unsigned int get(); }
        EntryPointReflection^ GetEntryPointByIndex(unsigned int index);
        EntryPointReflection^ FindEntryPointByName(System::String^ name);
        
        property unsigned int GlobalConstantBufferBinding { unsigned int get(); }
        property unsigned long GlobalConstantBufferSize { unsigned long get(); }
        
        TypeReflection^ FindTypeByName(System::String^ name);
        FunctionReflection^ FindFunctionByName(System::String^ name);
        FunctionReflection^ FindFunctionByNameInType(TypeReflection^ type, System::String^ name);
        VariableReflection^ FindVarByNameInType(TypeReflection^ type, System::String^ name);
        
        TypeLayoutReflection^ GetTypeLayout(TypeReflection^ type, int layoutRules);
        
        TypeReflection^ SpecializeType(TypeReflection^ type, array<TypeReflection^>^ specializationArgs);
        
        bool IsSubType(TypeReflection^ subType, TypeReflection^ superType);
        
        property unsigned int HashedStringCount { unsigned int get(); }
        System::String^ GetHashedString(unsigned int index);
        
        property TypeLayoutReflection^ GlobalParamsTypeLayout { TypeLayoutReflection^ get(); }
        property VariableLayoutReflection^ GlobalParamsVarLayout { VariableLayoutReflection^ get(); }
        
        System::String^ ToJson();

        // Internal
        void* getNative();
        void* slangPtr();

    private:
        void* m_NativeShaderReflection;
        bool m_bOwnsNative;
    };
}
