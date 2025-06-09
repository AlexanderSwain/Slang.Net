// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "VariableLayoutReflection.h"
#include "StringUtils.h"
#include "VariableReflection.h"
#include "TypeLayoutReflection.h"
#include "TypeReflection.h"
#include "Modifier.h"
#include <msclr/marshal.h>

namespace Slang
{
    // Constructor
    VariableLayoutReflection::VariableLayoutReflection(void* native)
    {
        m_NativeVariableLayoutReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    VariableLayoutReflection::~VariableLayoutReflection()
    {
        this->!VariableLayoutReflection();
    }

    // Finalizer
    VariableLayoutReflection::!VariableLayoutReflection()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeVariableLayoutReflection = nullptr;
    }

    // Properties    
    VariableReflection^ VariableLayoutReflection::Variable::get()
    {
        if (!m_NativeVariableLayoutReflection) return nullptr;
        void* variable = SlangNative::VariableLayoutReflection_GetVariable(m_NativeVariableLayoutReflection);
        return variable ? gcnew VariableReflection(variable) : nullptr;
    }    
    System::String^ VariableLayoutReflection::Name::get()
    {
        if (!m_NativeVariableLayoutReflection) return nullptr;
        return StringUtilities::ToString(SlangNative::VariableLayoutReflection_GetName(m_NativeVariableLayoutReflection));
    }Modifier^ VariableLayoutReflection::FindModifier(int id)
    {
        if (!m_NativeVariableLayoutReflection) return nullptr;
        void* modifier = SlangNative::VariableLayoutReflection_FindModifier(m_NativeVariableLayoutReflection, id);
        return modifier ? gcnew Modifier(modifier) : nullptr;
    }    
    TypeLayoutReflection^ VariableLayoutReflection::TypeLayout::get()
    {
        if (!m_NativeVariableLayoutReflection) return nullptr;
        void* typeLayout = SlangNative::VariableLayoutReflection_GetTypeLayout(m_NativeVariableLayoutReflection);
        return typeLayout ? gcnew TypeLayoutReflection(typeLayout) : nullptr;
    }    
    ParameterCategory VariableLayoutReflection::Category::get()
    {
        if (!m_NativeVariableLayoutReflection) return ParameterCategory::None;
        return static_cast<ParameterCategory>(SlangNative::VariableLayoutReflection_GetCategory(m_NativeVariableLayoutReflection));
    }    
    unsigned int VariableLayoutReflection::CategoryCount::get()
    {
        if (!m_NativeVariableLayoutReflection) return 0;
        return SlangNative::VariableLayoutReflection_GetCategoryCount(m_NativeVariableLayoutReflection);
    }   
    ParameterCategory VariableLayoutReflection::GetCategoryByIndex(unsigned int index)
    {
        if (!m_NativeVariableLayoutReflection) return ParameterCategory::None;
        return static_cast<ParameterCategory>(SlangNative::VariableLayoutReflection_GetCategoryByIndex(m_NativeVariableLayoutReflection, index));
    }
    System::UIntPtr VariableLayoutReflection::GetOffset(ParameterCategory category)
    {
        if (!m_NativeVariableLayoutReflection) return System::UIntPtr::Zero;
        return System::UIntPtr(SlangNative::VariableLayoutReflection_GetOffset(m_NativeVariableLayoutReflection, static_cast<int>(category)));
    }    
    TypeReflection^ VariableLayoutReflection::Type::get()
    {
        if (!m_NativeVariableLayoutReflection) return nullptr;
        void* type = SlangNative::VariableLayoutReflection_GetType(m_NativeVariableLayoutReflection);
        return type ? gcnew TypeReflection(type) : nullptr;
    }    
    unsigned int VariableLayoutReflection::BindingIndex::get()
    {
        if (!m_NativeVariableLayoutReflection) return 0;
        return SlangNative::VariableLayoutReflection_GetBindingIndex(m_NativeVariableLayoutReflection);
    }    
    unsigned int VariableLayoutReflection::BindingSpace::get()
    {
        if (!m_NativeVariableLayoutReflection) return 0;
        return SlangNative::VariableLayoutReflection_GetBindingSpace(m_NativeVariableLayoutReflection);
    }
    System::UIntPtr VariableLayoutReflection::GetBindingSpace(ParameterCategory category)
    {
        if (!m_NativeVariableLayoutReflection) return System::UIntPtr::Zero;
        void* space = SlangNative::VariableLayoutReflection_GetSpace(m_NativeVariableLayoutReflection, static_cast<int>(category));
        return System::UIntPtr(space);
    }    
    Slang::ImageFormat VariableLayoutReflection::ImageFormat::get()
    {
        if (!m_NativeVariableLayoutReflection) return Slang::ImageFormat::Unknown;
        return static_cast<Slang::ImageFormat>(SlangNative::VariableLayoutReflection_GetImageFormat(m_NativeVariableLayoutReflection));
    }
    System::String^ VariableLayoutReflection::SemanticName::get()
    {
        if (!m_NativeVariableLayoutReflection) return nullptr;
        return StringUtilities::ToString(SlangNative::VariableLayoutReflection_GetSemanticName(m_NativeVariableLayoutReflection));
    }
    System::UIntPtr VariableLayoutReflection::SemanticIndex::get()
    {
        if (!m_NativeVariableLayoutReflection) return System::UIntPtr::Zero;
        return System::UIntPtr(SlangNative::VariableLayoutReflection_GetSemanticIndex(m_NativeVariableLayoutReflection));
    }
    unsigned int VariableLayoutReflection::Stage::get()
    {
        if (!m_NativeVariableLayoutReflection) return 0;
        return SlangNative::VariableLayoutReflection_GetStage(m_NativeVariableLayoutReflection);
    }
    VariableLayoutReflection^ VariableLayoutReflection::PendingDataLayout::get()
    {
        if (!m_NativeVariableLayoutReflection) return nullptr;
        void* pendingData = SlangNative::VariableLayoutReflection_GetPendingDataLayout(m_NativeVariableLayoutReflection);
        return pendingData ? gcnew VariableLayoutReflection(pendingData) : nullptr;
    }

    void* VariableLayoutReflection::getNative()
    {
        return m_NativeVariableLayoutReflection;
    }
}
