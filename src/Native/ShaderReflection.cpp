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
    if (!native) throw std::invalid_argument("Native pointer cannot be null");
    
    m_parent = parent;
    m_native = (slang::ShaderReflection*)native;

    // Use lazy initialization - only initialize when accessed
    m_typeParameters = nullptr;
	m_parameters = nullptr;
    m_entryPoints = nullptr;
	m_globalParamsTypeLayout = nullptr;
    m_globalParamsVarLayout = nullptr;
    json_blob = nullptr;
}

Native::ShaderReflection::~ShaderReflection()
{
    // Clean up the type parameters array
    if (m_typeParameters)
    {
        for (uint32_t index = 0; index < m_native->getTypeParameterCount(); index++)
        {
            delete m_typeParameters[index];
        }
        delete[] m_typeParameters;
        m_typeParameters = nullptr;
    }

    // Clean up the parameters array
    if (m_parameters)
    {
        for (uint32_t index = 0; index < m_native->getParameterCount(); index++)
        {
            delete m_parameters[index];
        }
        delete[] m_parameters;
        m_parameters = nullptr;
    }

    // Clean up the entry points array
    if (m_entryPoints)
    {
        for (SlangUInt index = 0; index < m_native->getEntryPointCount(); index++)
        {
            delete m_entryPoints[index];
        }
        delete[] m_entryPoints;
        m_entryPoints = nullptr;
    }

	// Clean up types map
    for (auto& pair : m_types)
    {
        delete pair.second; // Delete the TypeReflection object
	}
	m_types.clear(); // Clear the map

	// Clean up functions map
    for (auto& pair : m_functions)
    {
        delete pair.second; // Delete the FunctionReflection object
    }
	m_functions.clear(); // Clear the map

	// Clean up FunctionByName Results list
    for (auto& result : m_function_by_name_in_type_results_to_delete) {
        delete result;
    }
    m_function_by_name_in_type_results_to_delete.clear();

    // Clean up VariableByNameInType Results list
    for (auto& result : m_var_by_name_in_type_results_to_delete) {
        delete result;
    }
    m_var_by_name_in_type_results_to_delete.clear();

    // Clean up TypeLayout Results list
    for (auto& result : m_type_layouts_results_to_delete) {
        delete result;
    }
    m_type_layouts_results_to_delete.clear();

    // Clean up SpecializeType Results list
    for (auto& result : m_specialize_type_results_to_delete) {
        delete result;
    }
    m_specialize_type_results_to_delete.clear();

    // Clean up global params type layout
    delete m_globalParamsTypeLayout;
    m_globalParamsTypeLayout = nullptr;

    // Clean up global params variable layout
    delete m_globalParamsVarLayout;
    m_globalParamsVarLayout = nullptr;

    // m_parent is managed by ProgramCLI, so we don't delete it here
    //if (m_parent)
    //{
    //	delete m_parent;
    //	m_parent = nullptr;
    //}

    // No need to delete m_native here, as it is managed by Slang
    //if (m_native)
    //{
    //	delete m_native;
    //	m_native = nullptr;
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
    if (!m_typeParameters)
    {
        uint32_t typeParameterCount = m_native->getTypeParameterCount();
        m_typeParameters = new TypeParameterReflection*[typeParameterCount];
        for (uint32_t i = 0; i < typeParameterCount; i++)
        {
            slang::TypeParameterReflection* nativeTypeParameter = m_native->getTypeParameterByIndex(i);
            if (nativeTypeParameter)
                m_typeParameters[i] = new TypeParameterReflection(nativeTypeParameter);
            else
                m_typeParameters[i] = nullptr;
        }
    }
	return m_typeParameters[index];
}

Native::TypeParameterReflection* Native::ShaderReflection::findTypeParameter(char const* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    slang::TypeParameterReflection* nativeResult = m_native->findTypeParameter(name);
    if (nativeResult)
        return new TypeParameterReflection(nativeResult);
    return nullptr;
}

Native::VariableLayoutReflection* Native::ShaderReflection::getParameterByIndex(unsigned index)
{
    if (!m_parameters)
    {
        uint32_t parameterCount = m_native->getParameterCount();
        m_parameters = new VariableLayoutReflection*[parameterCount];
        for (uint32_t i = 0; i < parameterCount; i++)
        {
            slang::VariableLayoutReflection* nativeParameter = m_native->getParameterByIndex(i);
            if (nativeParameter)
                m_parameters[i] = new VariableLayoutReflection(nativeParameter);
            else
                m_parameters[i] = nullptr;
        }
    }
	return m_parameters[index];
}

SlangUInt Native::ShaderReflection::getEntryPointCount()
{
    return m_native->getEntryPointCount();
}

Native::EntryPointReflection* Native::ShaderReflection::getEntryPointByIndex(SlangUInt index)
{
    if (!m_entryPoints)
    {
        SlangUInt entryPointCount = m_native->getEntryPointCount();
        m_entryPoints = new EntryPointReflection*[entryPointCount];
        for (SlangUInt i = 0; i < entryPointCount; i++)
        {
            slang::EntryPointReflection* nativeEntryPoint = m_native->getEntryPointByIndex(i);
            if (nativeEntryPoint)
                m_entryPoints[i] = new EntryPointReflection(this, nativeEntryPoint);
            else
                m_entryPoints[i] = nullptr;
        }
    }
	return m_entryPoints[index];
}

Native::EntryPointReflection* Native::ShaderReflection::findEntryPointByName(const char* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    slang::EntryPointReflection* nativeResult = m_native->findEntryPointByName(name);
    if (nativeResult)
        return new EntryPointReflection(this, nativeResult);
    return nullptr;
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
    // Check if the modifier is already cached
    auto it = m_types.find(name);

    // If the modifier is already cached, return it
    if (it != m_types.end())
        return it->second;

    // If not cached, create a new Modifier and cache it
    slang::TypeReflection* nativeResult = m_native->findTypeByName(name);
    if (nativeResult)
    {
        Native::TypeReflection* result = new TypeReflection(nativeResult);
        m_types[std::string(name)] = result;
        return result;
    }
    return nullptr;
}

Native::FunctionReflection* Native::ShaderReflection::findFunctionByName(const char* name)
{
    // Check if the modifier is already cached
    auto it = m_functions.find(name);

    // If the modifier is already cached, return it
    if (it != m_functions.end())
        return it->second;

    // If not cached, create a new Modifier and cache it
    slang::FunctionReflection* nativeResult = m_native->findFunctionByName(name);
    if (nativeResult)
    {
        Native::FunctionReflection* result = new FunctionReflection(nativeResult);
        m_functions[std::string(name)] = result;
        return result;
    }
    return nullptr;
}

Native::FunctionReflection* Native::ShaderReflection::findFunctionByNameInType(TypeReflection* type, const char* name)
{
    slang::FunctionReflection* nativeResult = m_native->findFunctionByNameInType((slang::TypeReflection*)type->getNative(), name);
    if (nativeResult)
    {
        Native::FunctionReflection* result = new Native::FunctionReflection(nativeResult);
        m_function_by_name_in_type_results_to_delete.push_back(result);
        return result;
    }
    return nullptr;
}

Native::VariableReflection* Native::ShaderReflection::findVarByNameInType(TypeReflection* type, const char* name)
{
    slang::VariableReflection* nativeResult = m_native->findVarByNameInType((slang::TypeReflection*)type->getNative(), name);
    if (nativeResult)
    {
        Native::VariableReflection* result = new Native::VariableReflection(nativeResult);
        m_var_by_name_in_type_results_to_delete.push_back(result);
        return result;
    }
    return nullptr;
}

Native::TypeLayoutReflection* Native::ShaderReflection::getTypeLayout(TypeReflection* type, LayoutRules rules)
{
    slang::TypeLayoutReflection* nativeResult = m_native->getTypeLayout((slang::TypeReflection*)type->getNative(), (slang::LayoutRules)rules);
    if (nativeResult)
    {
        Native::TypeLayoutReflection* result = new Native::TypeLayoutReflection(nativeResult);
        m_type_layouts_results_to_delete.push_back(result);
        return result;
    }
    return nullptr;
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

    if (result)
    {
        TypeReflection* resToRet = new TypeReflection(result);
        m_specialize_type_results_to_delete.push_back(resToRet);
        return resToRet;
    }
    return nullptr;
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
    if (!m_globalParamsTypeLayout)
    {
        slang::TypeLayoutReflection* globalParamsTypeLayoutPtr = m_native->getGlobalParamsTypeLayout();
        if (globalParamsTypeLayoutPtr) 
            m_globalParamsTypeLayout = new TypeLayoutReflection(globalParamsTypeLayoutPtr);
        else
            m_globalParamsTypeLayout = nullptr;
    }
    return m_globalParamsTypeLayout;
}

Native::VariableLayoutReflection* Native::ShaderReflection::getGlobalParamsVarLayout()
{
    if (!m_globalParamsVarLayout)
    {
        slang::VariableLayoutReflection* globalParamsVarLayoutPtr = m_native->getGlobalParamsVarLayout();
        if (globalParamsVarLayoutPtr) 
            m_globalParamsVarLayout = new VariableLayoutReflection(globalParamsVarLayoutPtr);
        else
            m_globalParamsVarLayout = nullptr;
    }
    return m_globalParamsVarLayout;
}

SlangResult Native::ShaderReflection::toJson(ISlangBlob** outBlob)
{
    if (!json_blob)
    {
        SlangResult result = m_native->toJson(&json_blob);
        if (SLANG_FAILED(result))
            return result;
    }

    (*outBlob) = json_blob;
    return SLANG_OK;
}
