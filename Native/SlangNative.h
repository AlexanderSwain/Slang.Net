#pragma once
#include "CompilerOptionCLI.h"
#include "PreprocessorMacroDescCLI.h"
#include "ShaderModelCLI.h"
#include "ParameterInfoCLI.h"
#include "TypeDef.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

using namespace Native;

namespace SlangNative
{
    extern "C" SLANGNATIVE_API void* CreateSession(Native::CompilerOptionCLI* options, int optionsLength,
        Native::PreprocessorMacroDescCLI* macros, int macrosLength,
        Native::ShaderModelCLI* models, int modelsLength,
        char* searchPaths[], int searchPathsLength,
        const char** error);    // Compilation API
    extern "C" SLANGNATIVE_API void* CreateModule(void* parentSession, const char* moduleName, const char* modulePath, const char* shaderSource, const char** error);
    extern "C" SLANGNATIVE_API void* FindEntryPoint(void* parentModule, const char* entryPointName, const char** error);
    extern "C" SLANGNATIVE_API void GetParameterInfo(void* parentEntryPoint, Native::ParameterInfoCLI** outParameterInfo, int* outParameterCount);
    extern "C" SLANGNATIVE_API void* CreateProgram(void* parentModule);
    extern "C" SLANGNATIVE_API int32_t Compile(void* program, unsigned int entryPointIndex, unsigned int targetIndex, const char** output);

    // Reflection API
    extern "C" SLANGNATIVE_API void* GetProgramReflection(void* program);
    
    // ShaderReflection API
    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetParameterCount(void* shaderReflection);
    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetTypeParameterCount(void* shaderReflection);
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetTypeParameterByIndex(void* shaderReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* ShaderReflection_FindTypeParameter(void* shaderReflection, const char* name);
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetParameterByIndex(void* shaderReflection, unsigned int index);
    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetEntryPointCount(void* shaderReflection);
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetEntryPointByIndex(void* shaderReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* ShaderReflection_FindEntryPointByName(void* shaderReflection, const char* name);
    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetGlobalConstantBufferBinding(void* shaderReflection);
    extern "C" SLANGNATIVE_API size_t ShaderReflection_GetGlobalConstantBufferSize(void* shaderReflection);
    extern "C" SLANGNATIVE_API void* ShaderReflection_FindTypeByName(void* shaderReflection, const char* name);
    extern "C" SLANGNATIVE_API void* ShaderReflection_FindFunctionByName(void* shaderReflection, const char* name);
    extern "C" SLANGNATIVE_API void* ShaderReflection_FindFunctionByNameInType(void* shaderReflection, void* type, const char* name);
    extern "C" SLANGNATIVE_API void* ShaderReflection_FindVarByNameInType(void* shaderReflection, void* type, const char* name);
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetTypeLayout(void* shaderReflection, void* type, int layoutRules);
    extern "C" SLANGNATIVE_API void* ShaderReflection_SpecializeType(void* shaderReflection, void* type, int argCount, void** args);
    extern "C" SLANGNATIVE_API bool ShaderReflection_IsSubType(void* shaderReflection, void* subType, void* superType);
    extern "C" SLANGNATIVE_API unsigned int ShaderReflection_GetHashedStringCount(void* shaderReflection);
    extern "C" SLANGNATIVE_API const char* ShaderReflection_GetHashedString(void* shaderReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetGlobalParamsTypeLayout(void* shaderReflection);
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetGlobalParamsVarLayout(void* shaderReflection);
    extern "C" SLANGNATIVE_API int ShaderReflection_ToJson(void* shaderReflection, const char** output);

    // TypeReflection API
    extern "C" SLANGNATIVE_API int TypeReflection_GetKind(void* typeReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetFieldCount(void* typeReflection);
    extern "C" SLANGNATIVE_API void* TypeReflection_GetFieldByIndex(void* typeReflection, unsigned int index);
    extern "C" SLANGNATIVE_API bool TypeReflection_IsArray(void* typeReflection);
    extern "C" SLANGNATIVE_API void* TypeReflection_UnwrapArray(void* typeReflection);
    extern "C" SLANGNATIVE_API size_t TypeReflection_GetElementCount(void* typeReflection);
    extern "C" SLANGNATIVE_API void* TypeReflection_GetElementType(void* typeReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetRowCount(void* typeReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetColumnCount(void* typeReflection);
    extern "C" SLANGNATIVE_API int TypeReflection_GetScalarType(void* typeReflection);
    extern "C" SLANGNATIVE_API int TypeReflection_GetResourceShape(void* typeReflection);
    extern "C" SLANGNATIVE_API int TypeReflection_GetResourceAccess(void* typeReflection);
    extern "C" SLANGNATIVE_API void* TypeReflection_GetResourceResultType(void* typeReflection);
    extern "C" SLANGNATIVE_API const char* TypeReflection_GetName(void* typeReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetUserAttributeCount(void* typeReflection);
    extern "C" SLANGNATIVE_API void* TypeReflection_GetUserAttributeByIndex(void* typeReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* TypeReflection_FindAttributeByName(void* typeReflection, const char* name);

    // TypeLayoutReflection API
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetType(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API int TypeLayoutReflection_GetKind(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetSize(void* typeLayoutReflection, int category);
    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetStride(void* typeLayoutReflection, int category);
    extern "C" SLANGNATIVE_API int TypeLayoutReflection_GetAlignment(void* typeLayoutReflection, int category);
    extern "C" SLANGNATIVE_API unsigned int TypeLayoutReflection_GetFieldCount(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetFieldByIndex(void* typeLayoutReflection, unsigned int index);
    extern "C" SLANGNATIVE_API int TypeLayoutReflection_FindFieldIndexByName(void* typeLayoutReflection, const char* name);
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetExplicitCounter(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API bool TypeLayoutReflection_IsArray(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_UnwrapArray(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetElementCount(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetTotalArrayElementCount(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API size_t TypeLayoutReflection_GetElementStride(void* typeLayoutReflection, int category);
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetElementTypeLayout(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetElementVarLayout(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetContainerVarLayout(void* typeLayoutReflection);

    // VariableReflection API
    extern "C" SLANGNATIVE_API const char* VariableReflection_GetName(void* variableReflection);
    extern "C" SLANGNATIVE_API void* VariableReflection_GetType(void* variableReflection);
    extern "C" SLANGNATIVE_API void* VariableReflection_FindModifier(void* variableReflection, int modifierId);
    extern "C" SLANGNATIVE_API unsigned int VariableReflection_GetUserAttributeCount(void* variableReflection);
    extern "C" SLANGNATIVE_API void* VariableReflection_GetUserAttributeByIndex(void* variableReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* VariableReflection_FindAttributeByName(void* variableReflection, const char* name);

    // VariableLayoutReflection API
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetVariable(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API const char* VariableLayoutReflection_GetName(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_FindModifier(void* variableLayoutReflection, int modifierId);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetTypeLayout(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetCategory(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API size_t VariableLayoutReflection_GetOffset(void* variableLayoutReflection, int category);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetSpace(void* variableLayoutReflection, int category);
    extern "C" SLANGNATIVE_API const char* VariableLayoutReflection_GetSemanticName(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API size_t VariableLayoutReflection_GetSemanticIndex(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetCategoryCount(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetCategoryByIndex(void* variableLayoutReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetType(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetBindingIndex(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetBindingSpace(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetImageFormat(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetStage(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetPendingDataLayout(void* variableLayoutReflection);

    // FunctionReflection API
    extern "C" SLANGNATIVE_API const char* FunctionReflection_GetName(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetReturnType(void* functionReflection);
    extern "C" SLANGNATIVE_API unsigned int FunctionReflection_GetParameterCount(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetParameterByIndex(void* functionReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* FunctionReflection_FindModifier(void* functionReflection, int modifierId);
    extern "C" SLANGNATIVE_API unsigned int FunctionReflection_GetUserAttributeCount(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetUserAttributeByIndex(void* functionReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* FunctionReflection_FindAttributeByName(void* functionReflection, const char* name);

    // EntryPointReflection API
    extern "C" SLANGNATIVE_API void* EntryPointReflection_AsFunction(void* entryPointReflection);
    extern "C" SLANGNATIVE_API const char* EntryPointReflection_GetName(void* entryPointReflection);
    extern "C" SLANGNATIVE_API const char* EntryPointReflection_GetNameOverride(void* entryPointReflection);
    extern "C" SLANGNATIVE_API unsigned int EntryPointReflection_GetParameterCount(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetParameterByIndex(void* entryPointReflection, unsigned int index);
    extern "C" SLANGNATIVE_API int EntryPointReflection_GetStage(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void EntryPointReflection_GetComputeThreadGroupSize(void* entryPointReflection, unsigned int outSizeAlongAxis[3]);
    extern "C" SLANGNATIVE_API bool EntryPointReflection_UsesAnySampleRateInput(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetVarLayout(void* entryPointReflection);

    // GenericReflection API
    extern "C" SLANGNATIVE_API const char* GenericReflection_GetName(void* genRefReflection);
    extern "C" SLANGNATIVE_API unsigned int GenericReflection_GetTypeParameterCount(void* genRefReflection);
    extern "C" SLANGNATIVE_API void* GenericReflection_GetTypeParameter(void* genRefReflection, unsigned int index);

    // TypeParameterReflection API
    extern "C" SLANGNATIVE_API const char* TypeParameterReflection_GetName(void* typeParameterReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeParameterReflection_GetIndex(void* typeParameterReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeParameterReflection_GetConstraintCount(void* typeParameterReflection);
    extern "C" SLANGNATIVE_API void* TypeParameterReflection_GetConstraintByIndex(void* typeParameterReflection, int index);

    // Attribute API
    extern "C" SLANGNATIVE_API const char* Attribute_GetName(void* attributeReflection);
    extern "C" SLANGNATIVE_API unsigned int Attribute_GetArgumentCount(void* attributeReflection);
    extern "C" SLANGNATIVE_API SlangResult Attribute_GetArgumentValueInt(void* attributeReflection, unsigned int index, int* value);
    extern "C" SLANGNATIVE_API SlangResult Attribute_GetArgumentValueFloat(void* attributeReflection, unsigned int index, float* value);
    extern "C" SLANGNATIVE_API const char* Attribute_GetArgumentValueString(void* attributeReflection, unsigned int index);

}
