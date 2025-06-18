// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Attribute.h"

namespace Slang::Cpp
{

    // Constructor
    Attribute::Attribute(void* native)
    {
        m_NativeAttribute = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    Attribute::~Attribute()
    {
        this->!Attribute();
    }

    // Finalizer
    Attribute::!Attribute()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeAttribute = nullptr;
    }    System::String^ Attribute::Name::get()
    {
        if (!m_NativeAttribute) return nullptr;
        return Slang::Cpp::StringUtilities::ToString(SlangNative::Attribute_GetName(m_NativeAttribute));
    }

    unsigned int Attribute::ArgumentCount::get()
    {
        if (!m_NativeAttribute) return 0;
        return SlangNative::Attribute_GetArgumentCount(m_NativeAttribute);
    }    System::String^ Attribute::GetArgumentValueString(unsigned int index)
    {
        if (!m_NativeAttribute) return nullptr;
        const char* value = SlangNative::Attribute_GetArgumentValueString(m_NativeAttribute, index);
        return Slang::Cpp::StringUtilities::ToString(value);
    }

    System::Nullable<System::Int32> Attribute::GetArgumentValueInt(unsigned int index)
    {
        if (!m_NativeAttribute) return System::Nullable<System::Int32>();
        int32_t value;
        if (SLANG_SUCCEEDED(SlangNative::Attribute_GetArgumentValueInt(m_NativeAttribute, index, &value)))
        {
            return System::Nullable<System::Int32>(value);
        }
        return System::Nullable<System::Int32>();
    }

    System::Nullable<System::Single> Attribute::GetArgumentValueFloat(unsigned int index)
    {
        if (!m_NativeAttribute) return System::Nullable<System::Single>();
        float value;
        if (SLANG_SUCCEEDED(SlangNative::Attribute_GetArgumentValueFloat(m_NativeAttribute, index, &value)))
        {
            return System::Nullable<System::Single>(value);
        }
        return System::Nullable<System::Single>();
    }

    void* Attribute::getNative()
    {
        return m_NativeAttribute;
    }

    void* Attribute::slangPtr()
    {
        return SlangNative::Attribute_GetNative(m_NativeAttribute);
    }
}
