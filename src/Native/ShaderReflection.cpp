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

    // Initialize the typeParameter array
    uint32_t typeParameterCount = m_native->getTypeParameterCount();
    m_typeParameters = new TypeParameterReflection*[typeParameterCount];
    for (uint32_t index = 0; index < typeParameterCount; index++)
    {
        m_typeParameters[index] = new TypeParameterReflection(m_native->getTypeParameterByIndex(index));
    }

	// Initialize the parameters array
    uint32_t parameterCount = m_native->getParameterCount();
    m_parameters = new VariableLayoutReflection*[parameterCount];
    for (uint32_t index = 0; index < parameterCount; index++)
    {
        m_parameters[index] = new VariableLayoutReflection(m_native->getParameterByIndex(index));
    }

    // Initialize the entry points array
    SlangUInt entryPointCount = m_native->getEntryPointCount();
    m_entryPoints = new EntryPointReflection*[entryPointCount];
    for (SlangUInt index = 0; index < entryPointCount; index++)
    {
        m_entryPoints[index] = new EntryPointReflection(this, m_native->getEntryPointByIndex(index));
	}

	// Initialize global params type layout
    m_globalParamsTypeLayout = new TypeLayoutReflection(m_native->getGlobalParamsTypeLayout());
    
    // Initialize global params variable layout
    m_globalParamsVarLayout = new VariableLayoutReflection(m_native->getGlobalParamsVarLayout());
}

Native::ShaderReflection::~ShaderReflection()
{
    // Clean up the type parameters array
    for (uint32_t index = 0; index < m_native->getTypeParameterCount(); index++)
    {
        delete m_typeParameters[index];
    }
    delete[] m_typeParameters;
    m_typeParameters = nullptr;

    // Clean up the parameters array
    for (uint32_t index = 0; index < m_native->getParameterCount(); index++)
    {
        delete m_parameters[index];
    }
    delete[] m_parameters;
    m_parameters = nullptr;

    // Clean up the entry points array
    for (SlangUInt index = 0; index < m_native->getEntryPointCount(); index++)
    {
        delete m_entryPoints[index];
    }
    delete[] m_entryPoints;
    m_entryPoints = nullptr;

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
	return m_typeParameters[index];
}

Native::TypeParameterReflection* Native::ShaderReflection::findTypeParameter(char const* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
    return new TypeParameterReflection(m_native->findTypeParameter(name));
}

Native::VariableLayoutReflection* Native::ShaderReflection::getParameterByIndex(unsigned index)
{
	return m_parameters[index];
}

SlangUInt Native::ShaderReflection::getEntryPointCount()
{
    return m_native->getEntryPointCount();
}

Native::EntryPointReflection* Native::ShaderReflection::getEntryPointByIndex(SlangUInt index)
{
	return m_entryPoints[index];
}

Native::EntryPointReflection* Native::ShaderReflection::findEntryPointByName(const char* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
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
    // Check if the modifier is already cached
    auto it = m_types.find(name);

    // If the modifier is already cached, return it
    if (it != m_types.end())
        return it->second;

    // If not cached, create a new Modifier and cache it
    Native::TypeReflection* result = new TypeReflection(m_native->findTypeByName(name));
    m_types[std::string(name)] = result;
    return result;
}

Native::FunctionReflection* Native::ShaderReflection::findFunctionByName(const char* name)
{
    // Check if the modifier is already cached
    auto it = m_functions.find(name);

    // If the modifier is already cached, return it
    if (it != m_functions.end())
        return it->second;

    // If not cached, create a new Modifier and cache it
    Native::FunctionReflection* result = new FunctionReflection(m_native->findFunctionByName(name));
    m_functions[std::string(name)] = result;
    return result;
}

Native::FunctionReflection* Native::ShaderReflection::findFunctionByNameInType(TypeReflection* type, const char* name)
{
    Native::FunctionReflection* result = new Native::FunctionReflection(m_native->findFunctionByNameInType((slang::TypeReflection*)type->getNative(), name));
    m_function_by_name_in_type_results_to_delete.push_back(result);
    return result;
}

Native::VariableReflection* Native::ShaderReflection::findVarByNameInType(TypeReflection* type, const char* name)
{
    Native::VariableReflection* result = new Native::VariableReflection(m_native->findVarByNameInType((slang::TypeReflection*)type->getNative(), name));
    m_var_by_name_in_type_results_to_delete.push_back(result);
    return result;
}

Native::TypeLayoutReflection* Native::ShaderReflection::getTypeLayout(TypeReflection* type, LayoutRules rules)
{
    Native::TypeLayoutReflection* result = new Native::TypeLayoutReflection(m_native->getTypeLayout((slang::TypeReflection*)type->getNative(), (slang::LayoutRules)rules));
    m_type_layouts_results_to_delete.push_back(result);
    return result;
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

    if (!result)
		return nullptr;

	TypeReflection* resToRet = new TypeReflection(result);
	m_specialize_type_results_to_delete.push_back(resToRet);
    return resToRet;
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
    return m_globalParamsTypeLayout;
}

Native::VariableLayoutReflection* Native::ShaderReflection::getGlobalParamsVarLayout()
{
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
