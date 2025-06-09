// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "TypeParameterReflection.h"
#include "TypeReflection.h"
#include "StringUtils.h"
#include <msclr/marshal.h>

using namespace SlangNative;

namespace Slang
{

    // Constructor
    TypeParameterReflection::TypeParameterReflection(void* native)
    {
        m_NativeTypeParameterReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    TypeParameterReflection::~TypeParameterReflection()
    {
        this->!TypeParameterReflection();
    }

    // Finalizer
    TypeParameterReflection::!TypeParameterReflection()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeTypeParameterReflection = nullptr;
    }    System::String^ TypeParameterReflection::Name::get()
    {
        if (!m_NativeTypeParameterReflection) return nullptr;        return StringUtilities::ToString(TypeParameterReflection_GetName(m_NativeTypeParameterReflection));
    }
    
    unsigned int TypeParameterReflection::Index::get()
    {
        if (!m_NativeTypeParameterReflection) return 0;        return TypeParameterReflection_GetIndex(m_NativeTypeParameterReflection);
    }
    
    unsigned int TypeParameterReflection::ConstraintCount::get()
    {
        if (!m_NativeTypeParameterReflection) return 0;        return TypeParameterReflection_GetConstraintCount(m_NativeTypeParameterReflection);
    }
    
    TypeReflection^ TypeParameterReflection::GetConstraintByIndex(int index)
    {
        if (!m_NativeTypeParameterReflection) return nullptr;
        void* constraint = TypeParameterReflection_GetConstraintByIndex(m_NativeTypeParameterReflection, index);
        return constraint ? gcnew TypeReflection(constraint) : nullptr;
    }

    void* TypeParameterReflection::getNative()
    {
        return m_NativeTypeParameterReflection;
    }
}
