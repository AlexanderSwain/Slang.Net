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

Native::ShaderReflection::ShaderReflection(ProgramCLI* parent, unsigned int targetIndex)
{
    if (!parent) throw std::invalid_argument("Parent cannot be null");
    
    m_parent = parent;

    // Get the native slang reflection from the program
    slang::ProgramLayout* slangProgramLayout = m_parent->GetLayout(targetIndex);

    m_native = (slang::ShaderReflection*)slangProgramLayout;
}

Native::ShaderReflection::~ShaderReflection()
{
    // m_parent is managed by ProgramCLI, so we don't delete it here
    //if (m_parent)
    //{
    //    delete m_parent;
    //    m_parent = nullptr;
    //}

    // No need to delete m_native here, as it is managed by Slang
    //if (m_native)
    //{
    //    delete m_native;
    //    m_native = nullptr;
    //}
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
    return new Native::TypeParameterReflection(m_native->getTypeParameterByIndex(index));
}

Native::TypeParameterReflection* Native::ShaderReflection::findTypeParameter(char const* name)
{
    return new Native::TypeParameterReflection(m_native->findTypeParameter(name));
}

Native::VariableLayoutReflection* Native::ShaderReflection::getParameterByIndex(unsigned index)
{
    return new Native::VariableLayoutReflection(m_native->getParameterByIndex(index));
}

SlangUInt Native::ShaderReflection::getEntryPointCount()
{
    return m_native->getEntryPointCount();
}

Native::EntryPointReflection* Native::ShaderReflection::getEntryPointByIndex(SlangUInt index)
{
    return new Native::EntryPointReflection(this, m_native->getEntryPointByIndex(index));
}

Native::EntryPointReflection* Native::ShaderReflection::findEntryPointByName(const char* name)
{
    return new Native::EntryPointReflection(this, m_native->findEntryPointByName(name));
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
    return new Native::TypeReflection(m_native->findTypeByName(name));
}

Native::FunctionReflection* Native::ShaderReflection::findFunctionByName(const char* name)
{
    return new Native::FunctionReflection(m_native->findFunctionByName(name));
}

Native::FunctionReflection* Native::ShaderReflection::findFunctionByNameInType(TypeReflection* type, const char* name)
{
    return new Native::FunctionReflection(m_native->findFunctionByNameInType(type->getNative(), name));
}

Native::VariableReflection* Native::ShaderReflection::findVarByNameInType(TypeReflection* type, const char* name)
{
    return new Native::VariableReflection(m_native->findVarByNameInType(type->getNative(), name));
}

Native::TypeLayoutReflection* Native::ShaderReflection::getTypeLayout(TypeReflection* type, LayoutRules rules)
{
    return new Native::TypeLayoutReflection(m_native->getTypeLayout(type->getNative(), (slang::LayoutRules)rules));
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

    auto callResult = new Native::TypeReflection(m_native->specializeType(
        (slang::TypeReflection*)type->getNative(),
        specializationArgCount,
        nativeArgs,
        outDiagnostics));

    delete[] nativeArgs;
    return callResult;
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
    return new Native::TypeLayoutReflection(m_native->getGlobalParamsTypeLayout());
}

Native::VariableLayoutReflection* Native::ShaderReflection::getGlobalParamsVarLayout()
{
    return new Native::VariableLayoutReflection(m_native->getGlobalParamsVarLayout());
}

SlangResult Native::ShaderReflection::toJson(const char** outBlob)
{
    Slang::ComPtr<ISlangBlob> json_blob;
    SlangResult result = m_native->toJson(json_blob.writeRef());
    if (SLANG_FAILED(result))
        throw std::runtime_error("Failed to convert ShaderReflection to JSON");

    // TODO: verify that there is no memory leak here, because it looks like there is
    *outBlob = _strdup((const char*)json_blob->getBufferPointer());

    return SLANG_OK;
}
