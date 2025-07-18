#include "SlangNative.h"
#include "SessionCLI.h"
#include "ModuleCLI.h"
#include "EntryPointCLI.h"
#include "ProgramCLI.h"
#include "Attribute.h"
#include "EntryPointReflection.h"
#include "FunctionReflection.h"
#include "GenericReflection.h"
#include "ShaderReflection.h"
#include "TypeLayoutReflection.h"
#include "TypeParameterReflection.h"
#include "TypeReflection.h"
#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "Modifier.h"
#include <string>

namespace SlangNative
{
    static thread_local std::string g_lastError;
    static void ThrowNotImplemented(const char* functionName)
    {
        throw std::runtime_error(std::string("Function not implemented: ") + functionName);
    }

    // Session API
    extern "C" SLANGNATIVE_API void* CreateSession(void* options, int optionsLength,
        void* macros, int macrosLength,
        void* models, int modelsLength,
        char* searchPaths[], int searchPathsLength,
        const char** error)
    {
        try
        {
            SessionCLI* result = new SessionCLI((CompilerOptionCLI*)options, optionsLength, (PreprocessorMacroDescCLI*)macros, macrosLength, (ShaderModelCLI*)models, modelsLength, searchPaths, searchPathsLength);
            *error = nullptr;
            return result;
        }
        catch (const std::exception& e)
        {
            g_lastError = e.what();
            *error = g_lastError.c_str();
            return nullptr;
        }
    }

    // Module API
    extern "C" SLANGNATIVE_API void* CreateModule(void* parentSession, const char* moduleName, const char* modulePath, const char* shaderSource, const char** error)
    {
        try
        {
            ModuleCLI* result = new ModuleCLI((SessionCLI*)parentSession, moduleName, modulePath, shaderSource);
            *error = nullptr;
            return result;
        }
        catch (const std::exception& e)
        {
            g_lastError = e.what();
            *error = g_lastError.c_str();
            return nullptr;
        }
    }

    extern "C" SLANGNATIVE_API void* FindEntryPoint(void* parentModule, const char* entryPointName, const char** error)
    {
        try 
        {
            EntryPointCLI* result = new EntryPointCLI((ModuleCLI*)parentModule, entryPointName);
            *error = nullptr;
            return result;
        }
        catch (const std::exception& e)
        {
            g_lastError = e.what();
            *error = g_lastError.c_str();
			return nullptr;
        }
    }

    extern "C" SLANGNATIVE_API void GetParameterInfo(void* parentEntryPoint, void** outParameterInfo, int* outParameterCount)
    {
        EntryPointCLI* entryPoint = (EntryPointCLI*)parentEntryPoint;
		*outParameterInfo = entryPoint->getParameterInfoArray();
        *outParameterCount = entryPoint->getParameterCount();
    }

    // Program API
    extern "C" SLANGNATIVE_API void* CreateProgram(void* parentModule)
    {
        ProgramCLI* result = new ProgramCLI((ModuleCLI*)parentModule);
        return result;
    }

    extern "C" SLANGNATIVE_API int32_t Compile(void* program, unsigned int entryPointIndex, unsigned int targetIndex, const char** output)
    {
        int32_t result = ((ProgramCLI*)program)->GetCompiled(entryPointIndex, targetIndex, output);
        return result;
    }

    // [Fix]
    extern "C" SLANGNATIVE_API void* GetProgramReflection(void* program)
    {
        if (!program) return nullptr;
        try 
        {
            ProgramCLI* programCLI = (ProgramCLI*)program;
            // Get the native slang reflection from the program
            void* nativeReflection = programCLI->GetReflection();
            if (nativeReflection)
            {
                return new Native::ShaderReflection(programCLI, nativeReflection);
            }
            return nullptr;
        }
        catch (...)
        {
            return nullptr;
        }
    }

    // ShaderReflection API
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetParent(void* shaderReflection)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->getParent();
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_GetNative(void* shaderReflection)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetParameterCount(void* shaderReflection)
    {
        if (!shaderReflection) return 0;
        return ((Native::ShaderReflection*)shaderReflection)->getParameterCount();
    }

    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetTypeParameterCount(void* shaderReflection)
    {
        if (!shaderReflection) return 0;
        return ((Native::ShaderReflection*)shaderReflection)->getTypeParameterCount();
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_GetTypeParameterByIndex(void* shaderReflection, unsigned int index)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->getTypeParameterByIndex(index);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_FindTypeParameter(void* shaderReflection, const char* name)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->findTypeParameter(name);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_GetParameterByIndex(void* shaderReflection, unsigned int index)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->getParameterByIndex(index);
    }

    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetEntryPointCount(void* shaderReflection)
    {
        if (!shaderReflection) return 0;
        return ((Native::ShaderReflection*)shaderReflection)->getEntryPointCount();
    }
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetEntryPointByIndex(void* shaderReflection, unsigned int index)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->getEntryPointByIndex(index);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_FindEntryPointByName(void* shaderReflection, const char* name)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->findEntryPointByName(name);
    }

    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetGlobalConstantBufferBinding(void* shaderReflection)
    {
        if (!shaderReflection) return 0;
        return (unsigned int)((Native::ShaderReflection*)shaderReflection)->getGlobalConstantBufferBinding();
    }

    extern "C" SLANGNATIVE_API size_t ShaderReflection_GetGlobalConstantBufferSize(void* shaderReflection)
    {
        if (!shaderReflection) return 0;
        return ((Native::ShaderReflection*)shaderReflection)->getGlobalConstantBufferSize();
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_FindTypeByName(void* shaderReflection, const char* name)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->findTypeByName(name);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_FindFunctionByName(void* shaderReflection, const char* name)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->findFunctionByName(name);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_FindFunctionByNameInType(void* shaderReflection, void* type, const char* name)
    {
                if (!shaderReflection || !type) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->findFunctionByNameInType((Native::TypeReflection*)type, name);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_FindVarByNameInType(void* shaderReflection, void* type, const char* name)
    {
        if (!shaderReflection || !type) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->findVarByNameInType((Native::TypeReflection*)type, name);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_GetTypeLayout(void* shaderReflection, void* type, int layoutRules)
    {
        if (!shaderReflection || !type) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->getTypeLayout((Native::TypeReflection*)type, (Native::LayoutRules)layoutRules);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_SpecializeType(void* shaderReflection, void* type, int argCount, void** args)
    {
        if (!shaderReflection || !type) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->specializeType((Native::TypeReflection*)type, argCount, (Native::TypeReflection**)args, nullptr);
    }

    extern "C" SLANGNATIVE_API bool ShaderReflection_IsSubType(void* shaderReflection, void* subType, void* superType)
    {
        if (!shaderReflection || !subType || !superType) return false;
        return ((Native::ShaderReflection*)shaderReflection)->isSubType((Native::TypeReflection*)subType, (Native::TypeReflection*)superType);
    }

    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetHashedStringCount(void* shaderReflection)
    {
        if (!shaderReflection) return 0;
        return (unsigned int)((Native::ShaderReflection*)shaderReflection)->getHashedStringCount();
    }

    extern "C" SLANGNATIVE_API const char* ShaderReflection_GetHashedString(void* shaderReflection, unsigned int index)
    {
        if (!shaderReflection) return nullptr;
        size_t count;
        return ((Native::ShaderReflection*)shaderReflection)->getHashedString(index, &count);
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_GetGlobalParamsTypeLayout(void* shaderReflection)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->getGlobalParamsTypeLayout();
    }

    extern "C" SLANGNATIVE_API void* ShaderReflection_GetGlobalParamsVarLayout(void* shaderReflection)
    {
        if (!shaderReflection) return nullptr;
        return ((Native::ShaderReflection*)shaderReflection)->getGlobalParamsVarLayout();
    }

    extern "C" SLANGNATIVE_API int ShaderReflection_ToJson(void* shaderReflection, const char** output)
    {
        if (!shaderReflection || !output) return SLANG_FAIL;
		ISlangBlob* blob = nullptr;
        SlangResult result = ((Native::ShaderReflection*)shaderReflection)->toJson(&blob);
        if (blob != nullptr)
        {
			*output = (const char*)blob->getBufferPointer();
            blob->release();
        }
        return SLANG_OK;
    }

    // TypeReflection API
    extern "C" SLANGNATIVE_API void* TypeReflection_GetNative(void* typeReflection)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API int TypeReflection_GetKind(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return (int)((Native::TypeReflection*)typeReflection)->getKind();
    }

    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetFieldCount(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return ((Native::TypeReflection*)typeReflection)->getFieldCount();
    }

    extern "C" SLANGNATIVE_API void* TypeReflection_GetFieldByIndex(void* typeReflection, unsigned int index)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->getFieldByIndex(index);
    }

    extern "C" SLANGNATIVE_API bool TypeReflection_IsArray(void* typeReflection)
    {
        if (!typeReflection) return false;
        return ((Native::TypeReflection*)typeReflection)->isArray();
    }

    extern "C" SLANGNATIVE_API void* TypeReflection_UnwrapArray(void* typeReflection)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->unwrapArray();
    }

    extern "C" SLANGNATIVE_API size_t TypeReflection_GetElementCount(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return ((Native::TypeReflection*)typeReflection)->getElementCount();
    }

    extern "C" SLANGNATIVE_API void* TypeReflection_GetElementType(void* typeReflection)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->getElementType();
    }

    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetRowCount(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return ((Native::TypeReflection*)typeReflection)->getRowCount();
    }

    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetColumnCount(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return ((Native::TypeReflection*)typeReflection)->getColumnCount();
    }

    extern "C" SLANGNATIVE_API int TypeReflection_GetScalarType(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return (int)((Native::TypeReflection*)typeReflection)->getScalarType();
    }

    extern "C" SLANGNATIVE_API void* TypeReflection_GetResourceResultType(void* typeReflection)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->getResourceResultType();
    }

    extern "C" SLANGNATIVE_API int TypeReflection_GetResourceShape(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return (int)((Native::TypeReflection*)typeReflection)->getResourceAccess();
    }

    extern "C" SLANGNATIVE_API int TypeReflection_GetResourceAccess(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return (int)((Native::TypeReflection*)typeReflection)->getResourceShape();
    }

    extern "C" SLANGNATIVE_API const char* TypeReflection_GetName(void* typeReflection)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->getName();
    }

    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetUserAttributeCount(void* typeReflection)
    {
        if (!typeReflection) return 0;
        return ((Native::TypeReflection*)typeReflection)->getUserAttributeCount();
    }

    extern "C" SLANGNATIVE_API void* TypeReflection_GetUserAttributeByIndex(void* typeReflection, unsigned int index)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->getUserAttributeByIndex(index);
    }

    extern "C" SLANGNATIVE_API void* TypeReflection_FindAttributeByName(void* typeReflection, const char* name)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->findAttributeByName(name);
    }    
    extern "C" SLANGNATIVE_API void* TypeReflection_ApplySpecializations(void* typeReflection, void* genRef)
    {
        if (!typeReflection || !genRef) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->applySpecializations((Native::GenericReflection*)genRef);
    }    extern "C" SLANGNATIVE_API void* TypeReflection_GetGenericContainer(void* typeReflection)
    {
        if (!typeReflection) return nullptr;
        return ((Native::TypeReflection*)typeReflection)->getGenericContainer();
    }

    // TypeLayoutReflection API
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetNative(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return nullptr;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetType(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return nullptr;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getType();
    }

    extern "C" SLANGNATIVE_API int TypeLayoutReflection_GetKind(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return 0;
        return (int)((Native::TypeLayoutReflection*)typeLayoutReflection)->getKind();
    }

    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetSize(void* typeLayoutReflection, int category)
    {
        if (!typeLayoutReflection) return 0;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getSize((Native::ParameterCategory)category);
    }

    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetStride(void* typeLayoutReflection, int category)
    {
        if (!typeLayoutReflection) return 0;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getStride((Native::ParameterCategory)category);
    }

    extern "C" SLANGNATIVE_API int TypeLayoutReflection_GetAlignment(void* typeLayoutReflection, int category)
    {
        if (!typeLayoutReflection) return 0;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getAlignment((Native::ParameterCategory)category);
    }

    extern "C" SLANGNATIVE_API unsigned int TypeLayoutReflection_GetFieldCount(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return 0;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getFieldCount();
    }

    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetFieldByIndex(void* typeLayoutReflection, unsigned int index)
    {
        if (!typeLayoutReflection) return nullptr;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getFieldByIndex(index);
    }

    extern "C" SLANGNATIVE_API int TypeLayoutReflection_FindFieldIndexByName(void* typeLayoutReflection, const char* name)
    {
        if (!typeLayoutReflection) return -1;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->findFieldIndexByName(name);
    }

    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetExplicitCounter(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return nullptr;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getExplicitCounter();
    }

    extern "C" SLANGNATIVE_API bool TypeLayoutReflection_IsArray(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return false;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->isArray();
    }

    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_UnwrapArray(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return nullptr;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->unwrapArray();
    }

    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetElementCount(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return 0;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getElementCount();
    }

    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetTotalArrayElementCount(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return 0;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getTotalArrayElementCount();
    }

    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetElementStride(void* typeLayoutReflection, int category)
    {
        if (!typeLayoutReflection) return 0;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getElementStride((Native::ParameterCategory)category);
    }

    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetElementTypeLayout(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return nullptr;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getElementTypeLayout();
    }

    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetElementVarLayout(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return nullptr;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getElementVarLayout();
    }

    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetContainerVarLayout(void* typeLayoutReflection)
    {
        if (!typeLayoutReflection) return nullptr;
        return ((Native::TypeLayoutReflection*)typeLayoutReflection)->getContainerVarLayout();
    }

    // VariableReflection API
    extern "C" SLANGNATIVE_API void* VariableReflection_GetNative(void* variableReflection)
    {
        if (!variableReflection) return nullptr;
        return ((Native::VariableReflection*)variableReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API const char* VariableReflection_GetName(void* variableReflection)
    {
        if (!variableReflection) return nullptr;
        return ((Native::VariableReflection*)variableReflection)->getName();
    }

    extern "C" SLANGNATIVE_API void* VariableReflection_GetType(void* variableReflection)
    {
        if (!variableReflection) return nullptr;
        return ((Native::VariableReflection*)variableReflection)->getType();
    }

    extern "C" SLANGNATIVE_API void* VariableReflection_FindModifier(void* variableReflection, int modifierId)
    {
        if (!variableReflection) return nullptr;
        return ((Native::VariableReflection*)variableReflection)->findModifier((Native::Modifier::ID)modifierId);
    }

    extern "C" SLANGNATIVE_API unsigned int VariableReflection_GetUserAttributeCount(void* variableReflection)
    {
        if (!variableReflection) return 0;
        return ((Native::VariableReflection*)variableReflection)->getUserAttributeCount();
    }

    extern "C" SLANGNATIVE_API void* VariableReflection_GetUserAttributeByIndex(void* variableReflection, unsigned int index)
    {
        if (!variableReflection) return nullptr;
        return ((Native::VariableReflection*)variableReflection)->getUserAttributeByIndex(index);
    }

    extern "C" SLANGNATIVE_API void* VariableReflection_FindAttributeByName(void* variableReflection, const char* name)
    {
        if (!variableReflection) return nullptr;
        return ((Native::VariableReflection*)variableReflection)->findAttributeByName(name);
    }    extern "C" SLANGNATIVE_API void* VariableReflection_FindUserAttributeByName(void* variableReflection, const char* name)
    {
        if (!variableReflection) return nullptr;
        return ((Native::VariableReflection*)variableReflection)->findUserAttributeByName(name);
    }    extern "C" SLANGNATIVE_API bool VariableReflection_HasDefaultValue(void* variableReflection)
    {
        if (!variableReflection) return false;
        return ((Native::VariableReflection*)variableReflection)->hasDefaultValue();
    }    extern "C" SLANGNATIVE_API SlangResult VariableReflection_GetDefaultValueInt(void* variableReflection, int64_t* value)
    {
        if (!variableReflection || !value) return SLANG_FAIL;
        return ((Native::VariableReflection*)variableReflection)->getDefaultValueInt(value);
    }    extern "C" SLANGNATIVE_API void* VariableReflection_GetGenericContainer(void* variableReflection)
    {
        if (!variableReflection) return nullptr;
        return ((Native::VariableReflection*)variableReflection)->getGenericContainer();
    }    extern "C" SLANGNATIVE_API void* VariableReflection_ApplySpecializations(void* variableReflection, void** specializations, int count)
    {
        if (!variableReflection) return nullptr;
        // For simplicity, assume specializations[0] is a GenericReflection*
        if (count > 0 && specializations && specializations[0])
        {
            return ((Native::VariableReflection*)variableReflection)->applySpecializations((Native::GenericReflection*)specializations[0]);
        }
        return nullptr;
    }

    // VariableLayoutReflection API
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetNative(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return nullptr;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetVariable(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return nullptr;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getVariable();
    }

    extern "C" SLANGNATIVE_API const char* VariableLayoutReflection_GetName(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return nullptr;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getName();
    }

    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_FindModifier(void* variableLayoutReflection, int modifierId)
    {
        if (!variableLayoutReflection) return nullptr;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->findModifier((Native::Modifier::ID)modifierId);
    }

    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetTypeLayout(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return nullptr;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getTypeLayout();
    }

    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetCategory(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return 0;
        return (int)((Native::VariableLayoutReflection*)variableLayoutReflection)->getCategory();
    }

    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetCategoryCount(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return 0;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getCategoryCount();
    }

    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetCategoryByIndex(void* variableLayoutReflection, unsigned int index)
    {
        if (!variableLayoutReflection) return 0;
        return static_cast<int>(((Native::VariableLayoutReflection*)variableLayoutReflection)->getCategoryByIndex(index));
    }

    extern "C" SLANGNATIVE_API size_t VariableLayoutReflection_GetOffset(void* variableLayoutReflection, int category)
    {
        if (!variableLayoutReflection) return 0;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getOffset((SlangParameterCategory)category);
    }

    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetType(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return nullptr;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getType();
    }

    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetBindingIndex(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return 0;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getBindingIndex();
    }

    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetBindingSpace(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return 0;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getBindingSpace();
    }

    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetSpace(void* variableLayoutReflection, int category)
    {
        if (!variableLayoutReflection) return nullptr;
        size_t space = ((Native::VariableLayoutReflection*)variableLayoutReflection)->getBindingSpace((SlangParameterCategory)category);
        return (void*)space;
    }

    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetImageFormat(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return 0;
        return static_cast<int>(((Native::VariableLayoutReflection*)variableLayoutReflection)->getImageFormat());
    }

    extern "C" SLANGNATIVE_API const char* VariableLayoutReflection_GetSemanticName(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return nullptr;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getSemanticName();
    }

    extern "C" SLANGNATIVE_API size_t VariableLayoutReflection_GetSemanticIndex(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return 0;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getSemanticIndex();
    }

    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetStage(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return 0;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getStage();
    }

    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetPendingDataLayout(void* variableLayoutReflection)
    {
        if (!variableLayoutReflection) return nullptr;
        return ((Native::VariableLayoutReflection*)variableLayoutReflection)->getPendingDataLayout();
    }

    // FunctionReflection API
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetNative(void* functionReflection)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API const char* FunctionReflection_GetName(void* functionReflection)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->getName();
    }

    extern "C" SLANGNATIVE_API void* FunctionReflection_GetReturnType(void* functionReflection)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->getReturnType();
    }

    extern "C" SLANGNATIVE_API unsigned int FunctionReflection_GetParameterCount(void* functionReflection)
    {
        if (!functionReflection) return 0;
        return ((Native::FunctionReflection*)functionReflection)->getParameterCount();
    }

    extern "C" SLANGNATIVE_API void* FunctionReflection_GetParameterByIndex(void* functionReflection, unsigned int index)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->getParameterByIndex(index);
    }

    extern "C" SLANGNATIVE_API void* FunctionReflection_FindModifier(void* functionReflection, int modifierId)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->findModifier((Native::Modifier::ID)modifierId);
    }

    extern "C" SLANGNATIVE_API unsigned int FunctionReflection_GetUserAttributeCount(void* functionReflection)
    {
        if (!functionReflection) return 0;
        return ((Native::FunctionReflection*)functionReflection)->getUserAttributeCount();
    }

    extern "C" SLANGNATIVE_API void* FunctionReflection_GetUserAttributeByIndex(void* functionReflection, unsigned int index)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->getUserAttributeByIndex(index);
    }

    extern "C" SLANGNATIVE_API void* FunctionReflection_FindAttributeByName(void* functionReflection, const char* name)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->findAttributeByName(name);
    }    extern "C" SLANGNATIVE_API void* FunctionReflection_GetGenericContainer(void* functionReflection)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->getGenericContainer();
    }    extern "C" SLANGNATIVE_API void* FunctionReflection_ApplySpecializations(void* functionReflection, void* genRef)
    {
        if (!functionReflection || !genRef) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->applySpecializations((Native::GenericReflection*)genRef);
    }    extern "C" SLANGNATIVE_API void* FunctionReflection_SpecializeWithArgTypes(void* functionReflection, unsigned int typeCount, void** types)
    {
        if (!functionReflection || !types) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->specializeWithArgTypes(typeCount, (Native::TypeReflection**)types);
    }    extern "C" SLANGNATIVE_API bool FunctionReflection_IsOverloaded(void* functionReflection)
    {
        if (!functionReflection) return false;
        return ((Native::FunctionReflection*)functionReflection)->isOverloaded();
    }    extern "C" SLANGNATIVE_API unsigned int FunctionReflection_GetOverloadCount(void* functionReflection)
    {
        if (!functionReflection) return 0;
        return ((Native::FunctionReflection*)functionReflection)->getOverloadCount();
    }    extern "C" SLANGNATIVE_API void* FunctionReflection_GetOverload(void* functionReflection, unsigned int index)
    {
        if (!functionReflection) return nullptr;
        return ((Native::FunctionReflection*)functionReflection)->getOverload(index);
    }



    // EntryPointReflection API
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetParent(void* entryPointReflection)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getParent();
    }

    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetNative(void* entryPointReflection)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API const char* EntryPointReflection_GetName(void* entryPointReflection)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getName();
    }

    extern "C" SLANGNATIVE_API const char* EntryPointReflection_GetNameOverride(void* entryPointReflection)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getNameOverride();
    }

    extern "C" SLANGNATIVE_API unsigned int EntryPointReflection_GetParameterCount(void* entryPointReflection)
    {
        if (!entryPointReflection) return 0;
        return ((Native::EntryPointReflection*)entryPointReflection)->getParameterCount();
    }

    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetParameterByIndex(void* entryPointReflection, unsigned int index)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getParameterByIndex(index);
    }   
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetFunction(void* entryPointReflection)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getFunction();
    }

    extern "C" SLANGNATIVE_API int EntryPointReflection_GetStage(void* entryPointReflection)
    {
        if (!entryPointReflection) return 0;
        return (int)((Native::EntryPointReflection*)entryPointReflection)->getStage();
    }

    extern "C" SLANGNATIVE_API void EntryPointReflection_GetComputeThreadGroupSize(void* entryPointReflection, unsigned int axisCount, SlangUInt* outSizeAlongAxis)
    {
        if (!entryPointReflection) return;
        ((Native::EntryPointReflection*)entryPointReflection)->getComputeThreadGroupSize(3, (Native::SlangUInt*)outSizeAlongAxis);
    }    extern "C" SLANGNATIVE_API SlangResult EntryPointReflection_GetComputeWaveSize(void* entryPointReflection, SlangUInt* outWaveSize)
    {
        if (!entryPointReflection || !outWaveSize) return SLANG_FAIL;
        try
        {
            ((Native::EntryPointReflection*)entryPointReflection)->getComputeWaveSize(outWaveSize);
            return SLANG_OK;
        }
        catch (...)
        {
            return SLANG_FAIL;
        }
    }

    extern "C" SLANGNATIVE_API bool EntryPointReflection_UsesAnySampleRateInput(void* entryPointReflection)
    {
        if (!entryPointReflection) return false;
        return ((Native::EntryPointReflection*)entryPointReflection)->usesAnySampleRateInput();
    }

    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetVarLayout(void* entryPointReflection)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getVarLayout();
    }    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetTypeLayout(void* entryPointReflection)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getTypeLayout();
    }    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetResultVarLayout(void* entryPointReflection)
    {
        if (!entryPointReflection) return nullptr;
        return ((Native::EntryPointReflection*)entryPointReflection)->getResultVarLayout();    }    extern "C" SLANGNATIVE_API bool EntryPointReflection_HasDefaultConstantBuffer(void* entryPointReflection)
    {
        if (!entryPointReflection) return false;
        return ((Native::EntryPointReflection*)entryPointReflection)->hasDefaultConstantBuffer();
    }

    // GenericReflection API
    extern "C" SLANGNATIVE_API void* GenericReflection_GetNative(void* genRefReflection)
    {
        if (!genRefReflection) return nullptr;
        return ((Native::GenericReflection*)genRefReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API const char* GenericReflection_GetName(void* genRefReflection)
    {
        if (!genRefReflection) return nullptr;
        return ((Native::GenericReflection*)genRefReflection)->getName();
    }

    extern "C" SLANGNATIVE_API unsigned int GenericReflection_GetTypeParameterCount(void* genRefReflection)
    {
        if (!genRefReflection) return 0;
        return ((Native::GenericReflection*)genRefReflection)->getTypeParameterCount();
    }

    extern "C" SLANGNATIVE_API void* GenericReflection_GetTypeParameter(void* genRefReflection, unsigned int index)
    {
        if (!genRefReflection) return nullptr;
        return ((Native::GenericReflection*)genRefReflection)->getTypeParameter(index);
    }    extern "C" SLANGNATIVE_API unsigned int GenericReflection_GetValueParameterCount(void* genRefReflection)
    {
        if (!genRefReflection) return 0;
        return ((Native::GenericReflection*)genRefReflection)->getValueParameterCount();
    }    extern "C" SLANGNATIVE_API void* GenericReflection_GetValueParameter(void* genRefReflection, unsigned int index)
    {
        if (!genRefReflection) return nullptr;
        return ((Native::GenericReflection*)genRefReflection)->getValueParameter(index);
    }    extern "C" SLANGNATIVE_API unsigned int GenericReflection_GetTypeParameterConstraintCount(void* genRefReflection, void* typeParam)
    {
        if (!genRefReflection || !typeParam) return 0;
        return ((Native::GenericReflection*)genRefReflection)->getTypeParameterConstraintCount((Native::VariableReflection*)typeParam);
    }    extern "C" SLANGNATIVE_API void* GenericReflection_GetTypeParameterConstraintType(void* genRefReflection, void* typeParam, unsigned int index)
    {
        if (!genRefReflection || !typeParam) return nullptr;
        return ((Native::GenericReflection*)genRefReflection)->getTypeParameterConstraintType((Native::VariableReflection*)typeParam, index);
    }    extern "C" SLANGNATIVE_API int GenericReflection_GetInnerKind(void* genRefReflection)
    {
        if (!genRefReflection) return 0;
        return (int)((Native::GenericReflection*)genRefReflection)->getInnerKind();
    }    extern "C" SLANGNATIVE_API void* GenericReflection_GetOuterGenericContainer(void* genRefReflection)
    {
        if (!genRefReflection) return nullptr;
        return ((Native::GenericReflection*)genRefReflection)->getOuterGenericContainer();
    }    extern "C" SLANGNATIVE_API void* GenericReflection_GetConcreteType(void* genRefReflection, void* typeParam)
    {
        if (!genRefReflection || !typeParam) return nullptr;
        return ((Native::GenericReflection*)genRefReflection)->getConcreteType((Native::VariableReflection*)typeParam);
    }    extern "C" SLANGNATIVE_API SlangResult GenericReflection_GetConcreteIntVal(void* genRefReflection, void* valueParam, int64_t* value)
    {
        if (!genRefReflection || !valueParam || !value) return SLANG_FAIL;
        try
        {
            *value = ((Native::GenericReflection*)genRefReflection)->getConcreteIntVal((Native::VariableReflection*)valueParam);
            return SLANG_OK;
        }
        catch (...)
        {
            return SLANG_FAIL;
        }
    }    extern "C" SLANGNATIVE_API void* GenericReflection_ApplySpecializations(void* genRefReflection, void* genRef)
    {
        if (!genRefReflection || !genRef) return nullptr;
        return ((Native::GenericReflection*)genRefReflection)->applySpecializations((Native::GenericReflection*)genRef);
    }

    // TypeParameterReflection API
    extern "C" SLANGNATIVE_API void* TypeParameterReflection_GetNative(void* typeParameterReflection)
    {
        if (!typeParameterReflection) return nullptr;
        return ((Native::TypeParameterReflection*)typeParameterReflection)->getNative();
    }

    extern "C" SLANGNATIVE_API const char* TypeParameterReflection_GetName(void* typeParameterReflection)
    {
        if (!typeParameterReflection) return nullptr;
        return ((Native::TypeParameterReflection*)typeParameterReflection)->getName();
    }

    extern "C" SLANGNATIVE_API unsigned int TypeParameterReflection_GetIndex(void* typeParameterReflection)
    {
        if (!typeParameterReflection) return 0;
        return ((Native::TypeParameterReflection*)typeParameterReflection)->getIndex();
    }

    extern "C" SLANGNATIVE_API unsigned int TypeParameterReflection_GetConstraintCount(void* typeParameterReflection)
    {
        if (!typeParameterReflection) return 0;
        return ((Native::TypeParameterReflection*)typeParameterReflection)->getConstraintCount();
    }

    extern "C" SLANGNATIVE_API void* TypeParameterReflection_GetConstraintByIndex(void* typeParameterReflection, int index)
    {
        if (!typeParameterReflection) return nullptr;
        return ((Native::TypeParameterReflection*)typeParameterReflection)->getConstraintByIndex(index);
    }

    // Attribute API
    extern "C" SLANGNATIVE_API void* Attribute_GetNative(void* attributeReflection)
    {
        if (!attributeReflection) return nullptr;
        return ((Native::Attribute*)attributeReflection)->getNative();
	}

    extern "C" SLANGNATIVE_API const char* Attribute_GetName(void* attributeReflection)
    {
        if (!attributeReflection) return nullptr;
        return ((Native::Attribute*)attributeReflection)->getName();
    }

    extern "C" SLANGNATIVE_API unsigned int Attribute_GetArgumentCount(void* attributeReflection)
    {
        if (!attributeReflection) return 0;
        return ((Native::Attribute*)attributeReflection)->getArgumentCount();
    }

    extern "C" SLANGNATIVE_API SlangResult Attribute_GetArgumentValueInt(void* attributeReflection, unsigned int index, int* value)
    {
        if (!attributeReflection || !value) return SLANG_FAIL;
        return ((Native::Attribute*)attributeReflection)->getArgumentValueInt(index, value);
    }

    extern "C" SLANGNATIVE_API SlangResult Attribute_GetArgumentValueFloat(void* attributeReflection, unsigned int index, float* value)
    {
        if (!attributeReflection || !value) return SLANG_FAIL;
        return ((Native::Attribute*)attributeReflection)->getArgumentValueFloat(index, value);
    }

    extern "C" SLANGNATIVE_API const char* Attribute_GetArgumentValueString(void* attributeReflection, unsigned int index)
    {
        if (!attributeReflection) return nullptr;
        size_t outSize;
        return ((Native::Attribute*)attributeReflection)->getArgumentValueString(index, &outSize);
    }    
    // Modifier API

    extern "C" SLANGNATIVE_API void* Modifier_GetNative(void* modifier)
    {
        if (!modifier) return nullptr;
        return ((Native::Modifier*)modifier)->getNative();
	}

    extern "C" SLANGNATIVE_API int Modifier_GetID(void* modifier)
    {
        if (!modifier) return 0;
        return (int)((Native::Modifier*)modifier)->getID();
    }

    extern "C" SLANGNATIVE_API const char* Modifier_GetName(void* modifier)
    {
        if (!modifier) return nullptr;
        return ((Native::Modifier*)modifier)->getName();
    }
}