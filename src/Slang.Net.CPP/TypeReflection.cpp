// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "TypeReflection.h"
#include "StringUtils.h"
#include "VariableReflection.h"
#include "Attribute.h"
#include "GenericReflection.h"
#include <msclr/marshal.h>

using namespace SlangNative;

namespace Slang::Cpp
{
    // Constructor
    TypeReflection::TypeReflection(void* native)
    {
        m_NativeTypeReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    TypeReflection::~TypeReflection()
    {
        this->!TypeReflection();
    }

    // Finalizer
    TypeReflection::!TypeReflection()
    {
		SlangNative::TypeReflection_Release(m_NativeTypeReflection);
        m_NativeTypeReflection = nullptr;
    }    
    
    // Properties
    TypeKind TypeReflection::Kind::get()
    {
        if (!m_NativeTypeReflection) return TypeKind::None;
        return static_cast<TypeKind>(TypeReflection_GetKind(m_NativeTypeReflection));
    }

    unsigned int TypeReflection::FieldCount::get()
    {
        if (!m_NativeTypeReflection) return 0;
        return TypeReflection_GetFieldCount(m_NativeTypeReflection);
    }

    VariableReflection^ TypeReflection::GetFieldByIndex(unsigned int index)
    {
        if (!m_NativeTypeReflection) return nullptr;
        void* field = TypeReflection_GetFieldByIndex(m_NativeTypeReflection, index);
        return field ? gcnew VariableReflection(field) : nullptr;
    }

    bool TypeReflection::IsArray::get()
    {
        if (!m_NativeTypeReflection) return false;
        return TypeReflection_IsArray(m_NativeTypeReflection);
    }

    TypeReflection^ TypeReflection::UnwrapArray()
    {
        if (!m_NativeTypeReflection) return nullptr;
        void* unwrapped = TypeReflection_UnwrapArray(m_NativeTypeReflection);
        return unwrapped ? gcnew TypeReflection(unwrapped) : nullptr;
    }    
    System::UIntPtr TypeReflection::ElementCount::get()
    {
        if (!m_NativeTypeReflection) return System::UIntPtr::Zero;
        return System::UIntPtr(TypeReflection_GetElementCount(m_NativeTypeReflection));
    }    
    System::UIntPtr TypeReflection::TotalArrayElementCount::get()
    {
        if (!m_NativeTypeReflection) return System::UIntPtr::Zero;
        // Note: This function may not be available in native interface, using ElementCount for now
        return System::UIntPtr(TypeReflection_GetElementCount(m_NativeTypeReflection));
    }
    TypeReflection^ TypeReflection::ElementType::get()
    {
        if (!m_NativeTypeReflection) return nullptr;
        void* elementType = SlangNative::TypeReflection_GetElementType(m_NativeTypeReflection);
        return elementType ? gcnew TypeReflection(elementType) : nullptr;
    }

    unsigned int TypeReflection::RowCount::get()
    {
        if (!m_NativeTypeReflection) return 0;
        return SlangNative::TypeReflection_GetRowCount(m_NativeTypeReflection);
    }

    unsigned int TypeReflection::ColumnCount::get()
    {
        if (!m_NativeTypeReflection) return 0;
        return SlangNative::TypeReflection_GetColumnCount(m_NativeTypeReflection);
    }

    Slang::Cpp::ScalarType TypeReflection::ScalarType::get()
    {
        if (!m_NativeTypeReflection) return Slang::Cpp::ScalarType::None;
        return static_cast<Slang::Cpp::ScalarType>(SlangNative::TypeReflection_GetScalarType(m_NativeTypeReflection));
    }    
    TypeReflection^ TypeReflection::ResourceResultType::get()
    {
        if (!m_NativeTypeReflection) return nullptr;
        void* resultType = SlangNative::TypeReflection_GetResourceResultType(m_NativeTypeReflection);
        return resultType ? gcnew TypeReflection(resultType) : nullptr;
    }

    Slang::Cpp::ResourceShape TypeReflection::ResourceShape::get()
    {
        if (!m_NativeTypeReflection) return Slang::Cpp::ResourceShape::None;
        return static_cast<Slang::Cpp::ResourceShape>(SlangNative::TypeReflection_GetResourceShape(m_NativeTypeReflection));
    }

    Slang::Cpp::ResourceAccess TypeReflection::ResourceAccess::get()
    {
        if (!m_NativeTypeReflection) return Slang::Cpp::ResourceAccess::None;
        return static_cast<Slang::Cpp::ResourceAccess>(SlangNative::TypeReflection_GetResourceAccess(m_NativeTypeReflection));
    }    
    System::String^ TypeReflection::Name::get()
    {
        if (!m_NativeTypeReflection) return nullptr;
        return Slang::Cpp::StringUtilities::ToString(SlangNative::TypeReflection_GetName(m_NativeTypeReflection));
    }    
    System::String^ TypeReflection::FullName::get()
    {
        if (!m_NativeTypeReflection) return nullptr;
        // TODO: Add FullName support to DLL exports, for now use Name
        return Slang::Cpp::StringUtilities::ToString(SlangNative::TypeReflection_GetName(m_NativeTypeReflection));
    }
    unsigned int TypeReflection::UserAttributeCount::get()
    {
        if (!m_NativeTypeReflection) return 0;
        return SlangNative::TypeReflection_GetUserAttributeCount(m_NativeTypeReflection);
    }

    Attribute^ TypeReflection::GetUserAttributeByIndex(unsigned int index)
    {
        if (!m_NativeTypeReflection) return nullptr;
        void* attribute = SlangNative::TypeReflection_GetUserAttributeByIndex(m_NativeTypeReflection, index);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }    Attribute^ TypeReflection::FindAttributeByName(System::String^ name)
    {
        if (!m_NativeTypeReflection) return nullptr;
        const char* nativeName = Slang::Cpp::StringUtilities::FromString(name);
        void* attribute = SlangNative::TypeReflection_FindAttributeByName(m_NativeTypeReflection, nativeName);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }    Attribute^ TypeReflection::FindUserAttributeByName(System::String^ name)
    {
        if (!m_NativeTypeReflection) return nullptr;
        const char* nativeName = Slang::Cpp::StringUtilities::FromString(name);
        void* attribute = SlangNative::TypeReflection_FindAttributeByName(m_NativeTypeReflection, nativeName);
        return attribute ? gcnew Attribute(attribute) : nullptr;
    }

    TypeReflection^ TypeReflection::ApplySpecializations(GenericReflection^ genRef)
    {
        // TODO: Add ApplySpecializations support to DLL exports
        // For now, return nullptr as this is an advanced feature
        return nullptr;
    }

    GenericReflection^ TypeReflection::GenericContainer::get()
    {
        // TODO: Add GenericContainer support to DLL exports
        // For now, return nullptr as this is an advanced feature
        return nullptr;
    }

    void* TypeReflection::getNative()
    {
        return m_NativeTypeReflection;
    }
    void* TypeReflection::slangPtr()
    {
        return SlangNative::TypeReflection_GetNative(m_NativeTypeReflection);
    }
}
