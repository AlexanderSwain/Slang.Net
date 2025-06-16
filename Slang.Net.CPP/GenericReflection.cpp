// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "GenericReflection.h"
#include "StringUtils.h"
#include "VariableReflection.h"
#include "TypeReflection.h"
#include <msclr/marshal.h>

namespace Slang
{
    // Constructor
    GenericReflection::GenericReflection(void* native)
    {
        m_NativeGenericReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    GenericReflection::~GenericReflection()
    {
        this->!GenericReflection();
    }

    // Finalizer
    GenericReflection::!GenericReflection()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeGenericReflection = nullptr;
    }

    System::String^ GenericReflection::Name::get()
    {
        if (!m_NativeGenericReflection) return nullptr;
        return StringUtilities::ToString(SlangNative::GenericReflection_GetName(m_NativeGenericReflection));
    }

    unsigned int GenericReflection::TypeParameterCount::get()
    {
        if (!m_NativeGenericReflection) return 0;
        return SlangNative::GenericReflection_GetTypeParameterCount(m_NativeGenericReflection);
    }

    VariableReflection^ GenericReflection::GetTypeParameter(unsigned int index)
    {
        if (!m_NativeGenericReflection) return nullptr;
        void* param = SlangNative::GenericReflection_GetTypeParameter(m_NativeGenericReflection, index);
        return param ? gcnew VariableReflection(param) : nullptr;
    }

    unsigned int GenericReflection::ValueParameterCount::get()
    {
        if (!m_NativeGenericReflection) return 0;
        return SlangNative::GenericReflection_GetValueParameterCount(m_NativeGenericReflection);
    }

    VariableReflection^ GenericReflection::GetValueParameter(unsigned int index)
    {
        if (!m_NativeGenericReflection) return nullptr;
        void* param = SlangNative::GenericReflection_GetValueParameter(m_NativeGenericReflection, index);
        return param ? gcnew VariableReflection(param) : nullptr;
    }

    unsigned int GenericReflection::GetTypeParameterConstraintCount(VariableReflection^ typeParam)
    {
        if (!m_NativeGenericReflection || !typeParam) return 0;
        void* nativeTypeParam = typeParam->getNative();
        return SlangNative::GenericReflection_GetTypeParameterConstraintCount(m_NativeGenericReflection, nativeTypeParam);
    }

    TypeReflection^ GenericReflection::GetTypeParameterConstraintType(VariableReflection^ typeParam, unsigned int index)
    {
        if (!m_NativeGenericReflection || !typeParam) return nullptr;
        void* nativeTypeParam = typeParam->getNative();
        void* constraintType = SlangNative::GenericReflection_GetTypeParameterConstraintType(m_NativeGenericReflection, nativeTypeParam, index);
        return constraintType ? gcnew TypeReflection(constraintType) : nullptr;
    }

    DeclKind GenericReflection::InnerKind::get()
    {
        if (!m_NativeGenericReflection) return DeclKind::Unsupported;
        return static_cast<DeclKind>(SlangNative::GenericReflection_GetInnerKind(m_NativeGenericReflection));
    }

    GenericReflection^ GenericReflection::OuterGenericContainer::get()
    {
        if (!m_NativeGenericReflection) return nullptr;
        void* outer = SlangNative::GenericReflection_GetOuterGenericContainer(m_NativeGenericReflection);
        return outer ? gcnew GenericReflection(outer) : nullptr;
    }

    TypeReflection^ GenericReflection::GetConcreteType(VariableReflection^ typeParam)
    {
        if (!m_NativeGenericReflection || !typeParam) return nullptr;
        void* nativeTypeParam = typeParam->getNative();
        void* concreteType = SlangNative::GenericReflection_GetConcreteType(m_NativeGenericReflection, nativeTypeParam);
        return concreteType ? gcnew TypeReflection(concreteType) : nullptr;
    }

    System::Nullable<System::Int64> GenericReflection::GetConcreteIntVal(VariableReflection^ valueParam)
    {
        if (!m_NativeGenericReflection || !valueParam) return System::Nullable<System::Int64>();
        void* nativeValueParam = valueParam->getNative();
        int64_t value;
        SlangResult result = SlangNative::GenericReflection_GetConcreteIntVal(m_NativeGenericReflection, nativeValueParam, &value);
        if (SLANG_SUCCEEDED(result))
            return System::Nullable<System::Int64>(value);
        return System::Nullable<System::Int64>();
    }

    GenericReflection^ GenericReflection::ApplySpecializations(GenericReflection^ genRef)
    {
        if (!m_NativeGenericReflection || !genRef) return nullptr;
        void* nativeGeneric = genRef->getNative();
        void* specialized = SlangNative::GenericReflection_ApplySpecializations(m_NativeGenericReflection, nativeGeneric);
        return specialized ? gcnew GenericReflection(specialized) : nullptr;
    }

    void* GenericReflection::getNative()
    {
        return m_NativeGenericReflection;
    }
    void* GenericReflection::slangPtr()
    {
        return SlangNative::GenericReflection_GetNative(m_NativeGenericReflection);
    }
}
