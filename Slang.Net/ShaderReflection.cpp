// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "ShaderReflection.h"
#include "TypeReflection.h"
#include "TypeLayoutReflection.h"
#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "FunctionReflection.h"
#include "EntryPointReflection.h"
#include "GenericReflection.h"
#include "TypeParameterReflection.h"
#include "Program.h"
#include "StringUtils.h"
#include <msclr/marshal.h>
#include <vcclr.h>
#include <vector>
#include <TypeDef.h>

//Debug: delete these
#include <iostream>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace SlangNative;

namespace Slang
{

    // Constructor
    ShaderReflection::ShaderReflection(void* native)
    {
        m_NativeShaderReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }    // Destructor
    ShaderReflection::~ShaderReflection()
    {
        this->!ShaderReflection();
    }
    
    // Finalizer
    ShaderReflection::!ShaderReflection()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeShaderReflection = nullptr;
    }

    Program^ ShaderReflection::Parent::get()
    {
        if (!m_NativeShaderReflection) return nullptr;
        void* parent = SlangNative::ShaderReflection_GetParent(m_NativeShaderReflection);
        return parent ? gcnew Program(parent) : nullptr;
    }

    unsigned int ShaderReflection::ParameterCount::get()
    {
        if (!m_NativeShaderReflection) return 0;
        return ShaderReflection_GetParameterCount(m_NativeShaderReflection);
    }

    unsigned int ShaderReflection::TypeParameterCount::get()
    {
        if (!m_NativeShaderReflection) return 0;
        return ShaderReflection_GetTypeParameterCount(m_NativeShaderReflection);
    }    TypeParameterReflection^ ShaderReflection::GetTypeParameterByIndex(unsigned int index)
    {
        if (!m_NativeShaderReflection) return nullptr;
        void* param = ShaderReflection_GetTypeParameterByIndex(m_NativeShaderReflection, index);
        return param ? gcnew TypeParameterReflection(param) : nullptr;
    }      TypeParameterReflection^ ShaderReflection::FindTypeParameter(System::String^ name)
    {
        if (!m_NativeShaderReflection || !name) return nullptr;
        const char* nativeName = StringUtilities::FromString(name);
        void* param = ShaderReflection_FindTypeParameter(m_NativeShaderReflection, nativeName);
        Marshal::FreeHGlobal(System::IntPtr((void*)nativeName));
        return param ? gcnew TypeParameterReflection(param) : nullptr;
    }
    
    VariableLayoutReflection^ ShaderReflection::GetParameterByIndex(unsigned int index)
    {
        if (!m_NativeShaderReflection) return nullptr;
        void* param = ShaderReflection_GetParameterByIndex(m_NativeShaderReflection, index);
        return param ? gcnew VariableLayoutReflection(param) : nullptr;
    }

    unsigned int ShaderReflection::EntryPointCount::get()
    {
        if (!m_NativeShaderReflection) return 0;
        return ShaderReflection_GetEntryPointCount(m_NativeShaderReflection);
    }    EntryPointReflection^ ShaderReflection::GetEntryPointByIndex(unsigned int index)
    {
        if (!m_NativeShaderReflection) return nullptr;
        void* entryPoint = ShaderReflection_GetEntryPointByIndex(m_NativeShaderReflection, index);
        return entryPoint ? gcnew EntryPointReflection(entryPoint) : nullptr;
    }      EntryPointReflection^ ShaderReflection::FindEntryPointByName(System::String^ name)
    {
        if (!m_NativeShaderReflection || !name) return nullptr;
        const char* nativeName = StringUtilities::FromString(name);
        void* entryPoint = ShaderReflection_FindEntryPointByName(m_NativeShaderReflection, nativeName);
        Marshal::FreeHGlobal(System::IntPtr((void*)nativeName));
        return entryPoint ? gcnew EntryPointReflection(entryPoint) : nullptr;
    }
    
    unsigned int ShaderReflection::GlobalConstantBufferBinding::get()
    {
        if (!m_NativeShaderReflection) return 0;
        return ShaderReflection_GetGlobalConstantBufferBinding(m_NativeShaderReflection);
    }    
    unsigned long ShaderReflection::GlobalConstantBufferSize::get()
    {
        if (!m_NativeShaderReflection) return 0;
        return ShaderReflection_GetGlobalConstantBufferSize(m_NativeShaderReflection);
    }      
    TypeReflection^ ShaderReflection::FindTypeByName(System::String^ name)
    {
        if (!m_NativeShaderReflection || !name) return nullptr;
        const char* nativeName = StringUtilities::FromString(name);
        void* type = ShaderReflection_FindTypeByName(m_NativeShaderReflection, nativeName);
        Marshal::FreeHGlobal(System::IntPtr((void*)nativeName));
        return type ? gcnew TypeReflection(type) : nullptr;
    }      
    FunctionReflection^ ShaderReflection::FindFunctionByName(System::String^ name)
    {
        if (!m_NativeShaderReflection || !name) return nullptr;
        const char* nativeName = StringUtilities::FromString(name);
        void* function = ShaderReflection_FindFunctionByName(m_NativeShaderReflection, nativeName);
        Marshal::FreeHGlobal(System::IntPtr((void*)nativeName));
        return function ? gcnew FunctionReflection(function) : nullptr;
    }      
    FunctionReflection^ ShaderReflection::FindFunctionByNameInType(TypeReflection^ type, System::String^ name)
    {
        if (!m_NativeShaderReflection || !type || !name) return nullptr;
        void* nativeType = type->getNative();
        const char* nativeName = StringUtilities::FromString(name);
        void* function = ShaderReflection_FindFunctionByNameInType(m_NativeShaderReflection, nativeType, nativeName);
        Marshal::FreeHGlobal(System::IntPtr((void*)nativeName));
        return function ? gcnew FunctionReflection(function) : nullptr;
    }
    VariableReflection^ ShaderReflection::FindVarByNameInType(TypeReflection^ type, System::String^ name)
    {
        if (!m_NativeShaderReflection || !type || !name) return nullptr;
        void* nativeType = type->getNative();
        const char* nativeName = StringUtilities::FromString(name);
        void* variable = ShaderReflection_FindVarByNameInType(m_NativeShaderReflection, nativeType, nativeName);
        Marshal::FreeHGlobal(System::IntPtr((void*)nativeName));        return variable ? gcnew VariableReflection(variable) : nullptr;
    }

    TypeLayoutReflection^ ShaderReflection::GetTypeLayout(TypeReflection^ type, int layoutRules)
    {
        if (!m_NativeShaderReflection || !type) return nullptr;
        void* nativeType = type->getNative();
        void* layout = ShaderReflection_GetTypeLayout(m_NativeShaderReflection, nativeType, layoutRules);
        return layout ? gcnew TypeLayoutReflection(layout) : nullptr;
    }
    
    TypeReflection^ ShaderReflection::SpecializeType(TypeReflection^ type, array<TypeReflection^>^ specializationArgs)
    {
        if (!m_NativeShaderReflection || !type) return nullptr;
        void* nativeType = type->getNative();
        
        if (!specializationArgs || specializationArgs->Length == 0)
        {
            void* specialized = ShaderReflection_SpecializeType(m_NativeShaderReflection, nativeType, 0, nullptr);
            return specialized ? gcnew TypeReflection(specialized) : nullptr;
        }

        // Convert managed array to native array using simple array allocation
        void** nativeArgs = new void*[specializationArgs->Length];
        for (int i = 0; i < specializationArgs->Length; i++)
        {
            nativeArgs[i] = specializationArgs[i]->getNative();
        }
          void* specialized = ShaderReflection_SpecializeType(m_NativeShaderReflection, nativeType, specializationArgs->Length, nativeArgs);
        delete[] nativeArgs;
        return specialized ? gcnew TypeReflection(specialized) : nullptr;
    }
    
    bool ShaderReflection::IsSubType(TypeReflection^ subType, TypeReflection^ superType)
    {
        if (!m_NativeShaderReflection || !subType || !superType) return false;
        void* nativeSubType = subType->getNative();
        void* nativeSuperType = superType->getNative();        return ShaderReflection_IsSubType(m_NativeShaderReflection, nativeSubType, nativeSuperType);
    }
    
    unsigned int ShaderReflection::HashedStringCount::get()
    {
        if (!m_NativeShaderReflection) return 0;
        return ShaderReflection_GetHashedStringCount(m_NativeShaderReflection);
    }

    System::String^ ShaderReflection::GetHashedString(unsigned int index)
    {
        if (!m_NativeShaderReflection) return nullptr;
        const char* str = ShaderReflection_GetHashedString(m_NativeShaderReflection, index);
        return str ? StringUtilities::ToString(str) : nullptr;
    }
    
    TypeLayoutReflection^ ShaderReflection::GlobalParamsTypeLayout::get()
    {
        if (!m_NativeShaderReflection) return nullptr;
        void* layout = ShaderReflection_GetGlobalParamsTypeLayout(m_NativeShaderReflection);
        return layout ? gcnew TypeLayoutReflection(layout) : nullptr;
    }

    VariableLayoutReflection^ ShaderReflection::GlobalParamsVarLayout::get()
    {
        if (!m_NativeShaderReflection) return nullptr;
        void* layout = ShaderReflection_GetGlobalParamsVarLayout(m_NativeShaderReflection);
        return layout ? gcnew VariableLayoutReflection(layout) : nullptr;
    }

    System::String^ ShaderReflection::ToJson()
    {
        if (!m_NativeShaderReflection) return nullptr;

        const char* jsonStr = nullptr;
        SlangResult toJsonResult = ShaderReflection_ToJson(m_NativeShaderReflection, &jsonStr);
        

        if (toJsonResult < 0)
            return nullptr;//throw exception here

        String^ result = StringUtilities::ToString(jsonStr);
		//free((void*)jsonStr);

		return result;
    }

    void* ShaderReflection::getNative()
    {
        return m_NativeShaderReflection;
    }
}
