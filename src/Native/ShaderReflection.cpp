#include "ShaderReflection.h"
#include "TypeReflection.h"
#include "TypeLayoutReflection.h"
#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "FunctionReflection.h"
#include "EntryPointReflection.h"
#include "GenericReflection.h"
#include "TypeParameterReflection.h"
#include "LayoutRules.h"
#include "ProgramCLI.h"

Native::ShaderReflection::ShaderReflection(ProgramCLI* parent, void* native)
{
    m_parent = parent;
    m_native = (slang::ShaderReflection*)native;
}

Native::ProgramCLI* Native::ShaderReflection::getParent()
{
    return m_parent;
}
slang::ShaderReflection* Native::ShaderReflection::getNative()
{
    return m_native;
}

unsigned Native::ShaderReflection::getParameterCount()
{
    return m_native->getParameterCount();
}

unsigned Native::ShaderReflection::getTypeParameterCount()
{
    return m_native->getTypeParameterCount();
}

Native::TypeParameterReflection* Native::ShaderReflection::getTypeParameterByIndex(unsigned index)
{
    return new TypeParameterReflection(m_native->getTypeParameterByIndex(index));
}

Native::TypeParameterReflection* Native::ShaderReflection::findTypeParameter(char const* name)
{
    return new TypeParameterReflection(m_native->findTypeParameter(name));
}

Native::VariableLayoutReflection* Native::ShaderReflection::getParameterByIndex(unsigned index)
{
    return new VariableLayoutReflection(m_native->getParameterByIndex(index));
}

SlangUInt Native::ShaderReflection::getEntryPointCount()
{
    return m_native->getEntryPointCount();
}

Native::EntryPointReflection* Native::ShaderReflection::getEntryPointByIndex(SlangUInt index)
{
    return new EntryPointReflection(this, m_native->getEntryPointByIndex(index));
}

Native::EntryPointReflection* Native::ShaderReflection::findEntryPointByName(const char* name)
{
    return new EntryPointReflection(this, m_native->findEntryPointByName(name));
}

SlangUInt Native::ShaderReflection::getGlobalConstantBufferBinding()
{
    return m_native->getGlobalConstantBufferBinding();
}

size_t Native::ShaderReflection::getGlobalConstantBufferSize()
{
    return m_native->getGlobalConstantBufferSize();
}

Native::TypeReflection* Native::ShaderReflection::findTypeByName(const char* name)
{
    return new TypeReflection(m_native->findTypeByName(name));
}

Native::FunctionReflection* Native::ShaderReflection::findFunctionByName(const char* name)
{
    return new FunctionReflection(m_native->findFunctionByName(name));
}

Native::FunctionReflection* Native::ShaderReflection::findFunctionByNameInType(TypeReflection* type, const char* name)
{
    return new FunctionReflection(m_native->findFunctionByNameInType((slang::TypeReflection*)type->getNative(), name));
}

Native::VariableReflection* Native::ShaderReflection::findVarByNameInType(TypeReflection* type, const char* name)
{
    return new VariableReflection(m_native->findVarByNameInType((slang::TypeReflection*)type->getNative(), name));
}

Native::TypeLayoutReflection* Native::ShaderReflection::getTypeLayout(TypeReflection* type, LayoutRules rules)
{
    return new TypeLayoutReflection(m_native->getTypeLayout((slang::TypeReflection*)type->getNative(), (slang::LayoutRules)rules));
}

Native::TypeReflection* Native::ShaderReflection::specializeType(
    TypeReflection* type,
    SlangInt specializationArgCount,
    TypeReflection* const* specializationArgs,
    ISlangBlob** outDiagnostics)
{
    // Convert Native::TypeReflection array to slang::TypeReflection array
    slang::TypeReflection** nativeArgs = new slang::TypeReflection*[specializationArgCount];
    for (SlangInt i = 0; i < specializationArgCount; i++)
    {
        nativeArgs[i] = (slang::TypeReflection*)specializationArgs[i]->getNative();
    }
    
    slang::TypeReflection* result = m_native->specializeType(
        (slang::TypeReflection*)type->getNative(),
        specializationArgCount,
        nativeArgs,
        outDiagnostics);
    
    delete[] nativeArgs;
    return result ? new TypeReflection(result) : nullptr;
}

bool Native::ShaderReflection::isSubType(TypeReflection* subType, TypeReflection* superType)
{
    return m_native->isSubType(
        (slang::TypeReflection*)subType->getNative(),
        (slang::TypeReflection*)superType->getNative());
}

SlangUInt Native::ShaderReflection::getHashedStringCount()
{
    return m_native->getHashedStringCount();
}

const char* Native::ShaderReflection::getHashedString(SlangUInt index, size_t* outCount)
{
    return m_native->getHashedString(index, outCount);
}

Native::TypeLayoutReflection* Native::ShaderReflection::getGlobalParamsTypeLayout()
{
    return new TypeLayoutReflection(m_native->getGlobalParamsTypeLayout());
}

Native::VariableLayoutReflection* Native::ShaderReflection::getGlobalParamsVarLayout()
{
    return new VariableLayoutReflection(m_native->getGlobalParamsVarLayout());
}

SlangResult Native::ShaderReflection::toJson(ISlangBlob** outBlob)
{
    return m_native->toJson(outBlob);
}
