// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "FunctionReflection.h"
#include "StringUtils.h"
#include "TypeReflection.h"
#include "VariableReflection.h"
#include "Attribute.h"
#include "GenericReflection.h"
#include "Modifier.h"
#include <msclr/marshal.h>

namespace Slang
{

    // Constructor
    FunctionReflection::FunctionReflection(void* native)
    {
        m_NativeFunctionReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    FunctionReflection::~FunctionReflection()
    {
        this->!FunctionReflection();
    }

    // Finalizer
    FunctionReflection::!FunctionReflection()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeFunctionReflection = nullptr;
    }    System::String^ FunctionReflection::Name::get()
    {
        if (!m_NativeFunctionReflection) return nullptr;
        return StringUtilities::ToString(SlangNative::FunctionReflection_GetName(m_NativeFunctionReflection));
    }

    TypeReflection^ FunctionReflection::ReturnType::get()
    {
        if (!m_NativeFunctionReflection) return nullptr;
        void* returnType = SlangNative::FunctionReflection_GetReturnType(m_NativeFunctionReflection);
        return returnType ? gcnew TypeReflection(returnType) : nullptr;
    }

    unsigned int FunctionReflection::ParameterCount::get()
    {
        if (!m_NativeFunctionReflection) return 0;
        return SlangNative::FunctionReflection_GetParameterCount(m_NativeFunctionReflection);
    }

    VariableReflection^ FunctionReflection::GetParameterByIndex(unsigned int index)
    {
        if (!m_NativeFunctionReflection) return nullptr;
        void* param = SlangNative::FunctionReflection_GetParameterByIndex(m_NativeFunctionReflection, index);
        return param ? gcnew VariableReflection(param) : nullptr;
    }

    unsigned int FunctionReflection::UserAttributeCount::get()
    {
        if (!m_NativeFunctionReflection) return 0;
        return SlangNative::FunctionReflection_GetUserAttributeCount(m_NativeFunctionReflection);
    }

    Attribute^ FunctionReflection::GetUserAttributeByIndex(unsigned int index)
    {
        if (!m_NativeFunctionReflection) return nullptr;
        void* attribute = SlangNative::FunctionReflection_GetUserAttributeByIndex(m_NativeFunctionReflection, index);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }    Attribute^ FunctionReflection::FindAttributeByName(System::String^ name)
    {
        if (!m_NativeFunctionReflection) return nullptr;
        const char* nativeName = StringUtilities::FromString(name);
        void* attribute = SlangNative::FunctionReflection_FindAttributeByName(m_NativeFunctionReflection, nativeName);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }    Attribute^ FunctionReflection::FindUserAttributeByName(System::String^ name)
    {
        if (!m_NativeFunctionReflection) return nullptr;
        const char* nativeName = StringUtilities::FromString(name);
        void* attribute = SlangNative::FunctionReflection_FindAttributeByName(m_NativeFunctionReflection, nativeName);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }

    Modifier^ FunctionReflection::FindModifier(int id)
    {
        if (!m_NativeFunctionReflection) return nullptr;
        void* modifier = SlangNative::FunctionReflection_FindModifier(m_NativeFunctionReflection, id);
        return modifier ? gcnew Modifier(modifier) : nullptr;
    }

    GenericReflection^ FunctionReflection::GenericContainer::get()
    {
        if (!m_NativeFunctionReflection) return nullptr;
        void* container = SlangNative::FunctionReflection_GetGenericContainer(m_NativeFunctionReflection);
        return container ? gcnew GenericReflection(container) : nullptr;
    }

    FunctionReflection^ FunctionReflection::ApplySpecializations(GenericReflection^ genRef)
    {
        if (!m_NativeFunctionReflection || !genRef) return nullptr;
        void* nativeGeneric = genRef->getNative();
        void* specialized = SlangNative::FunctionReflection_ApplySpecializations(m_NativeFunctionReflection, nativeGeneric);
        return specialized ? gcnew FunctionReflection(specialized) : nullptr;
    }    FunctionReflection^ FunctionReflection::SpecializeWithArgTypes(array<TypeReflection^>^ types)
    {
        if (!m_NativeFunctionReflection || !types) return nullptr;
        
        // Convert managed array to native array
        unsigned int typeCount = types->Length;
        void** nativeTypes = new void*[typeCount];
        for (unsigned int i = 0; i < typeCount; i++)
        {
            nativeTypes[i] = types[i]->getNative();
        }

        void* specialized = SlangNative::FunctionReflection_SpecializeWithArgTypes(m_NativeFunctionReflection, typeCount, nativeTypes);
        
        delete[] nativeTypes;
        return specialized ? gcnew FunctionReflection(specialized) : nullptr;
    }

    bool FunctionReflection::IsOverloaded::get()
    {
        if (!m_NativeFunctionReflection) return false;
        return SlangNative::FunctionReflection_IsOverloaded(m_NativeFunctionReflection);
    }

    unsigned int FunctionReflection::OverloadCount::get()
    {
        if (!m_NativeFunctionReflection) return 0;
        return SlangNative::FunctionReflection_GetOverloadCount(m_NativeFunctionReflection);
    }

    FunctionReflection^ FunctionReflection::GetOverload(unsigned int index)
    {
        if (!m_NativeFunctionReflection) return nullptr;
        void* overload = SlangNative::FunctionReflection_GetOverload(m_NativeFunctionReflection, index);
        return overload ? gcnew FunctionReflection(overload) : nullptr;
    }

    void* FunctionReflection::getNative()
    {
        return m_NativeFunctionReflection;
    }
    void* FunctionReflection::slangPtr()
    {
        return SlangNative::FunctionReflection_GetNative(m_NativeFunctionReflection);
    }
}
