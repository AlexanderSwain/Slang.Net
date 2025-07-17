// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "VariableReflection.h"

namespace Slang::Cpp
{
    // Constructor
    VariableReflection::VariableReflection(void* native)
    {
        m_NativeVariableReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    VariableReflection::~VariableReflection()
    {
        this->!VariableReflection();
    }

    // Finalizer
    VariableReflection::!VariableReflection()
    {
		SlangNative::VariableReflection_Release(m_NativeVariableReflection);
        m_NativeVariableReflection = nullptr;
    }

    // Properties
    System::String^ VariableReflection::Name::get()
    {
        if (!m_NativeVariableReflection) return nullptr;
        return Slang::Cpp::StringUtilities::ToString(SlangNative::VariableReflection_GetName(m_NativeVariableReflection));
    }

    TypeReflection^ VariableReflection::Type::get()
    {
        if (!m_NativeVariableReflection) return nullptr;
        void* type = SlangNative::VariableReflection_GetType(m_NativeVariableReflection);
        return type ? gcnew TypeReflection(type) : nullptr;
    }

    Modifier^ VariableReflection::FindModifier(int id)
    {
        if (!m_NativeVariableReflection) return nullptr;
        void* modifier = SlangNative::VariableReflection_FindModifier(m_NativeVariableReflection, id);
        return modifier ? gcnew Modifier(modifier) : nullptr;
    }

    unsigned int VariableReflection::UserAttributeCount::get()
    {
        if (!m_NativeVariableReflection) return 0;
        return SlangNative::VariableReflection_GetUserAttributeCount(m_NativeVariableReflection);
    }

    Attribute^ VariableReflection::GetUserAttributeByIndex(unsigned int index)
    {
        if (!m_NativeVariableReflection) return nullptr;
        void* attribute = SlangNative::VariableReflection_GetUserAttributeByIndex(m_NativeVariableReflection, index);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }

    Attribute^ VariableReflection::FindAttributeByName(System::String^ name)
    {
        if (!m_NativeVariableReflection || !name) return nullptr;
        const char* nameStr = Slang::Cpp::StringUtilities::FromString(name);
        void* attribute = SlangNative::VariableReflection_FindAttributeByName(m_NativeVariableReflection, nameStr);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }

    Attribute^ VariableReflection::FindUserAttributeByName(System::String^ name)
    {
        if (!m_NativeVariableReflection || !name) return nullptr;
        msclr::interop::marshal_context ctx;
        const char* nameStr = ctx.marshal_as<const char*>(name);
        void* attribute = SlangNative::VariableReflection_FindUserAttributeByName(m_NativeVariableReflection, nameStr);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }

    bool VariableReflection::HasDefaultValue::get()
    {
        if (!m_NativeVariableReflection) return false;
        return SlangNative::VariableReflection_HasDefaultValue(m_NativeVariableReflection);
    }

    System::Nullable<System::Int64> VariableReflection::GetDefaultValueInt()
    {
        if (!m_NativeVariableReflection) return System::Nullable<System::Int64>();
        int64_t value;
        SlangResult result = SlangNative::VariableReflection_GetDefaultValueInt(m_NativeVariableReflection, &value);
        if (SLANG_SUCCEEDED(result))
            return System::Nullable<System::Int64>(value);
        return System::Nullable<System::Int64>();
    }

    GenericReflection^ VariableReflection::GenericContainer::get()
    {
        if (!m_NativeVariableReflection) return nullptr;
        void* container = SlangNative::VariableReflection_GetGenericContainer(m_NativeVariableReflection);
        return container ? gcnew GenericReflection(container) : nullptr;
    }

    VariableReflection^ VariableReflection::ApplySpecializations(GenericReflection^ genRef)
    {
        if (!m_NativeVariableReflection || !genRef) return nullptr;
        void* nativeGeneric = genRef->getNative();
        void* specializations[] = { nativeGeneric };
        void* specialized = SlangNative::VariableReflection_ApplySpecializations(m_NativeVariableReflection, specializations, 1);
        return specialized ? gcnew VariableReflection(specialized) : nullptr;
    }

    void* VariableReflection::getNative()
    {
        return m_NativeVariableReflection;
    }
    void* VariableReflection::slangPtr()
    {
        return SlangNative::VariableReflection_GetNative(m_NativeVariableReflection);
    }
}
