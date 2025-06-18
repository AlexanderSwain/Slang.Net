// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "TypeLayoutReflection.h"
#include "StringUtils.h"
#include "TypeReflection.h"
#include "VariableLayoutReflection.h"
#include <msclr/marshal.h>

namespace Slang::Cpp
{
    // Constructor
    TypeLayoutReflection::TypeLayoutReflection(void* native)
    {
        m_NativeTypeLayoutReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    TypeLayoutReflection::~TypeLayoutReflection()
    {
        this->!TypeLayoutReflection();
    }

    // Finalizer
    TypeLayoutReflection::!TypeLayoutReflection()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeTypeLayoutReflection = nullptr;
    }

    // Properties    
    TypeReflection^ TypeLayoutReflection::Type::get()
    {
        if (!m_NativeTypeLayoutReflection) return nullptr;
        void* type = SlangNative::TypeLayoutReflection_GetType(m_NativeTypeLayoutReflection);
        return type ? gcnew TypeReflection(type) : nullptr;
    }    
    TypeKind TypeLayoutReflection::Kind::get()
    {
        if (!m_NativeTypeLayoutReflection) return TypeKind::None;
        return static_cast<TypeKind>(SlangNative::TypeLayoutReflection_GetKind(m_NativeTypeLayoutReflection));
    }    
    System::UIntPtr TypeLayoutReflection::GetSize(ParameterCategory category)
    {
        if (!m_NativeTypeLayoutReflection) return System::UIntPtr::Zero;
        return System::UIntPtr(SlangNative::TypeLayoutReflection_GetSize(m_NativeTypeLayoutReflection, static_cast<int>(category)));
    }    
    System::UIntPtr TypeLayoutReflection::GetStride(ParameterCategory category)
    {
        if (!m_NativeTypeLayoutReflection) return System::UIntPtr::Zero;
        return System::UIntPtr(SlangNative::TypeLayoutReflection_GetStride(m_NativeTypeLayoutReflection, static_cast<int>(category)));
    }    
    int TypeLayoutReflection::GetAlignment(ParameterCategory category)
    {
        if (!m_NativeTypeLayoutReflection) return 0;
        return SlangNative::TypeLayoutReflection_GetAlignment(m_NativeTypeLayoutReflection, static_cast<int>(category));
    }    
    unsigned int TypeLayoutReflection::FieldCount::get()
    {
        if (!m_NativeTypeLayoutReflection) return 0;
        return SlangNative::TypeLayoutReflection_GetFieldCount(m_NativeTypeLayoutReflection);
    }

    VariableLayoutReflection^ TypeLayoutReflection::GetFieldByIndex(unsigned int index)
    {
        if (!m_NativeTypeLayoutReflection) return nullptr;
        void* field = SlangNative::TypeLayoutReflection_GetFieldByIndex(m_NativeTypeLayoutReflection, index);
        return field ? gcnew VariableLayoutReflection(field) : nullptr;
    }    
    int TypeLayoutReflection::FindFieldIndexByName(System::String^ name)
    {
        if (!m_NativeTypeLayoutReflection) return -1;
        const char* nativeName = Slang::Cpp::StringUtilities::FromString(name);
        return SlangNative::TypeLayoutReflection_FindFieldIndexByName(m_NativeTypeLayoutReflection, nativeName);
    }
    VariableLayoutReflection^ TypeLayoutReflection::ExplicitCounter::get()
    {
        if (!m_NativeTypeLayoutReflection) return nullptr;
        void* counter = SlangNative::TypeLayoutReflection_GetExplicitCounter(m_NativeTypeLayoutReflection);
        return counter ? gcnew VariableLayoutReflection(counter) : nullptr;
    }    
    bool TypeLayoutReflection::IsArray::get()
    {
        if (!m_NativeTypeLayoutReflection) return false;
        return SlangNative::TypeLayoutReflection_IsArray(m_NativeTypeLayoutReflection);
    }    
    TypeLayoutReflection^ TypeLayoutReflection::UnwrapArray()
    {
        if (!m_NativeTypeLayoutReflection) return nullptr;
        void* unwrapped = SlangNative::TypeLayoutReflection_UnwrapArray(m_NativeTypeLayoutReflection);
        return unwrapped ? gcnew TypeLayoutReflection(unwrapped) : nullptr;
    }    
    System::UIntPtr TypeLayoutReflection::ElementCount::get()
    {
        if (!m_NativeTypeLayoutReflection) return System::UIntPtr::Zero;
        return System::UIntPtr(SlangNative::TypeLayoutReflection_GetElementCount(m_NativeTypeLayoutReflection));
    }    
    System::UIntPtr TypeLayoutReflection::TotalArrayElementCount::get()
    {
        if (!m_NativeTypeLayoutReflection) return System::UIntPtr::Zero;
        return System::UIntPtr(SlangNative::TypeLayoutReflection_GetTotalArrayElementCount(m_NativeTypeLayoutReflection));
    }    
    System::UIntPtr TypeLayoutReflection::GetElementStride(ParameterCategory category)
    {
        if (!m_NativeTypeLayoutReflection) return System::UIntPtr::Zero;
        return System::UIntPtr(SlangNative::TypeLayoutReflection_GetElementStride(m_NativeTypeLayoutReflection, static_cast<int>(category)));
    }    
    TypeLayoutReflection^ TypeLayoutReflection::ElementTypeLayout::get()
    {
        if (!m_NativeTypeLayoutReflection) return nullptr;
        void* elementType = SlangNative::TypeLayoutReflection_GetElementTypeLayout(m_NativeTypeLayoutReflection);
        return elementType ? gcnew TypeLayoutReflection(elementType) : nullptr;
    }    
    VariableLayoutReflection^ TypeLayoutReflection::ElementVarLayout::get()
    {
        if (!m_NativeTypeLayoutReflection) return nullptr;
        void* elementVar = SlangNative::TypeLayoutReflection_GetElementVarLayout(m_NativeTypeLayoutReflection);
        return elementVar ? gcnew VariableLayoutReflection(elementVar) : nullptr;
    }    
    VariableLayoutReflection^ TypeLayoutReflection::ContainerVarLayout::get()
    {
        if (!m_NativeTypeLayoutReflection) return nullptr;
        void* containerVar = SlangNative::TypeLayoutReflection_GetContainerVarLayout(m_NativeTypeLayoutReflection);
        return containerVar ? gcnew VariableLayoutReflection(containerVar) : nullptr;
    }

    void* TypeLayoutReflection::getNative()
    {
        return m_NativeTypeLayoutReflection;
    }
    void* TypeLayoutReflection::slangPtr()
    {
        return SlangNative::TypeLayoutReflection_GetNative(m_NativeTypeLayoutReflection);
    }
}
