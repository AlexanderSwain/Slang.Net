// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "EntryPointReflection.h"
#include "StringUtils.h"
#include "FunctionReflection.h"
#include "VariableLayoutReflection.h"
#include "TypeLayoutReflection.h"
#include <msclr/marshal.h>

namespace Slang
{

    // Constructor
    EntryPointReflection::EntryPointReflection(void* native)
    {
        m_NativeEntryPointReflection = native;
        m_bOwnsNative = false; // We don't own the native pointer in this case
    }

    // Destructor
    EntryPointReflection::~EntryPointReflection()
    {
        this->!EntryPointReflection();
    }

    // Finalizer
    EntryPointReflection::!EntryPointReflection()
    {
        // Note: We typically don't delete the native pointer as it's managed by Slang
        m_NativeEntryPointReflection = nullptr;
    }    System::String^ EntryPointReflection::Name::get()
    {
        if (!m_NativeEntryPointReflection) return nullptr;
        return StringUtilities::ToString(SlangNative::EntryPointReflection_GetName(m_NativeEntryPointReflection));
    }

    System::String^ EntryPointReflection::NameOverride::get()
    {
        if (!m_NativeEntryPointReflection) return nullptr;
        return StringUtilities::ToString(SlangNative::EntryPointReflection_GetNameOverride(m_NativeEntryPointReflection));
    }

    unsigned int EntryPointReflection::ParameterCount::get()
    {
        if (!m_NativeEntryPointReflection) return 0;
        return SlangNative::EntryPointReflection_GetParameterCount(m_NativeEntryPointReflection);
    }

    VariableLayoutReflection^ EntryPointReflection::GetParameterByIndex(unsigned int index)
    {
        if (!m_NativeEntryPointReflection) return nullptr;
        void* param = SlangNative::EntryPointReflection_GetParameterByIndex(m_NativeEntryPointReflection, index);
        return param ? gcnew VariableLayoutReflection(param) : nullptr;
    }

    FunctionReflection^ EntryPointReflection::Function::get()
    {
        if (!m_NativeEntryPointReflection) return nullptr;
        void* function = SlangNative::EntryPointReflection_GetFunction(m_NativeEntryPointReflection);
        return function ? gcnew FunctionReflection(function) : nullptr;
    }

    SlangStage EntryPointReflection::Stage::get()
    {
        if (!m_NativeEntryPointReflection) return SlangStage::None;
        return static_cast<SlangStage>(SlangNative::EntryPointReflection_GetStage(m_NativeEntryPointReflection));
    }    array<System::UInt32>^ EntryPointReflection::GetComputeThreadGroupSize()
    {
        if (!m_NativeEntryPointReflection) return nullptr;
        
        SlangUInt sizes[3];
        SlangNative::EntryPointReflection_GetComputeThreadGroupSize(m_NativeEntryPointReflection, 3, sizes);
        
        array<System::UInt32>^ result = gcnew array<System::UInt32>(3);
        result[0] = static_cast<System::UInt32>(sizes[0]);
        result[1] = static_cast<System::UInt32>(sizes[1]);
        result[2] = static_cast<System::UInt32>(sizes[2]);
        
        return result;
    }

    System::Nullable<System::UInt32> EntryPointReflection::ComputeWaveSize::get()
    {
        if (!m_NativeEntryPointReflection) return System::Nullable<System::UInt32>();
        
        SlangUInt waveSize;
        SlangNative::EntryPointReflection_GetComputeWaveSize(m_NativeEntryPointReflection, &waveSize);
        
        if (waveSize > 0)
            return System::Nullable<System::UInt32>(static_cast<System::UInt32>(waveSize));
        
        return System::Nullable<System::UInt32>();
    }

    bool EntryPointReflection::UsesAnySampleRateInput::get()
    {
        if (!m_NativeEntryPointReflection) return false;
        return SlangNative::EntryPointReflection_UsesAnySampleRateInput(m_NativeEntryPointReflection);
    }

    VariableLayoutReflection^ EntryPointReflection::VarLayout::get()
    {
        if (!m_NativeEntryPointReflection) return nullptr;
        void* varLayout = SlangNative::EntryPointReflection_GetVarLayout(m_NativeEntryPointReflection);
        return varLayout ? gcnew VariableLayoutReflection(varLayout) : nullptr;
    }

    TypeLayoutReflection^ EntryPointReflection::TypeLayout::get()
    {
        if (!m_NativeEntryPointReflection) return nullptr;
        void* typeLayout = SlangNative::EntryPointReflection_GetTypeLayout(m_NativeEntryPointReflection);
        return typeLayout ? gcnew TypeLayoutReflection(typeLayout) : nullptr;
    }

    VariableLayoutReflection^ EntryPointReflection::ResultVarLayout::get()
    {
        if (!m_NativeEntryPointReflection) return nullptr;
        void* resultVarLayout = SlangNative::EntryPointReflection_GetResultVarLayout(m_NativeEntryPointReflection);
        return resultVarLayout ? gcnew VariableLayoutReflection(resultVarLayout) : nullptr;
    }    bool EntryPointReflection::HasDefaultConstantBuffer::get()
    {
        if (!m_NativeEntryPointReflection) return false;
        return SlangNative::EntryPointReflection_HasDefaultConstantBuffer(m_NativeEntryPointReflection);
    }

    void* EntryPointReflection::getNative()
    {
        return m_NativeEntryPointReflection;
    }
}
