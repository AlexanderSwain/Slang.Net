#pragma once
#include "CompilerOptionCLI.h"
#include "PreprocessorMacroDescCLI.h"
#include "TargetCLI.h"
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
    //Diagnostics
    extern "C" SLANGNATIVE_API const char* SlangNative_GetLastError();

    // Session API
    extern "C" SLANGNATIVE_API void* Session_Create(
        void* options, int optionsLength,
        void* macros, int macrosLength,
        void* models, int modelsLength,
        char* searchPaths[], int searchPathsLength);
    extern "C" SLANGNATIVE_API void Session_Release(void* session);

    // Module API
    extern "C" SLANGNATIVE_API void* Module_Create(void* parentSession, const char* moduleName, const char* modulePath, const char* shaderSource);
    extern "C" SLANGNATIVE_API void* Module_Import(void* parentSession, const char* moduleName);
    extern "C" SLANGNATIVE_API void Module_Release(void* module);
	extern "C" SLANGNATIVE_API unsigned int Module_GetEntryPointCount(void* parentModule);
    extern "C" SLANGNATIVE_API void* Module_GetEntryPointByIndex(void* parentModule, unsigned int index);
	extern "C" SLANGNATIVE_API void* Module_FindEntryPointByName(void* parentModule, const char* entryPointName);

	// EntryPoint API
	extern "C" SLANGNATIVE_API void* EntryPoint_Create(void* parentModule, unsigned int entryPointIndex);
    extern "C" SLANGNATIVE_API void* EntryPoint_CreateByName(void* parentModule, const char* entryPointName);
    extern "C" SLANGNATIVE_API void EntryPoint_Release(void* entryPoint);
	extern "C" SLANGNATIVE_API int EntryPoint_GetIndex(void* entryPoint);
	extern "C" SLANGNATIVE_API const char* EntryPoint_GetName(void* entryPoint);

    // Program API
    extern "C" SLANGNATIVE_API void* Program_Create(void* parentModule);
    extern "C" SLANGNATIVE_API void Program_Release(void* program);
    extern "C" SLANGNATIVE_API int32_t Program_CompileProgram(void* program, unsigned int entryPointIndex, unsigned int targetIndex, const char** output);
    extern "C" SLANGNATIVE_API void* Program_GetProgramReflection(void* program, unsigned int targetIndex);

    // ShaderReflection API
    extern "C" SLANGNATIVE_API void ShaderReflection_Release(void* shaderReflection);
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetParent(void* shaderReflection);
    extern "C" SLANGNATIVE_API void* ShaderReflection_GetNative(void* shaderReflection);
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
    extern "C" SLANGNATIVE_API void TypeReflection_Release(void* typeReflection);
    extern "C" SLANGNATIVE_API void* TypeReflection_GetNative(void* typeReflection);
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
    extern "C" SLANGNATIVE_API void* TypeReflection_GetResourceResultType(void* typeReflection);
    extern "C" SLANGNATIVE_API int TypeReflection_GetResourceShape(void* typeReflection);
    extern "C" SLANGNATIVE_API int TypeReflection_GetResourceAccess(void* typeReflection);
    extern "C" SLANGNATIVE_API const char* TypeReflection_GetName(void* typeReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeReflection_GetUserAttributeCount(void* typeReflection);
    extern "C" SLANGNATIVE_API void* TypeReflection_GetUserAttributeByIndex(void* typeReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* TypeReflection_FindAttributeByName(void* typeReflection, const char* name);
    extern "C" SLANGNATIVE_API void* TypeReflection_ApplySpecializations(void* typeReflection, void* genRef);
    extern "C" SLANGNATIVE_API void* TypeReflection_GetGenericContainer(void* typeReflection);

    // TypeLayoutReflection API
    extern "C" SLANGNATIVE_API void TypeLayoutReflection_Release(void* typeLayoutReflection);
    extern "C" SLANGNATIVE_API void* TypeLayoutReflection_GetNative(void* typeLayoutReflection);
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
    extern "C" SLANGNATIVE_API void VariableReflection_Release(void* variableReflection);
    extern "C" SLANGNATIVE_API void* VariableReflection_GetNative(void* variableReflection);
    extern "C" SLANGNATIVE_API const char* VariableReflection_GetName(void* variableReflection);
    extern "C" SLANGNATIVE_API void* VariableReflection_GetType(void* variableReflection);
    extern "C" SLANGNATIVE_API void* VariableReflection_FindModifier(void* variableReflection, int modifierId);
    extern "C" SLANGNATIVE_API unsigned int VariableReflection_GetUserAttributeCount(void* variableReflection);
    extern "C" SLANGNATIVE_API void* VariableReflection_GetUserAttributeByIndex(void* variableReflection, unsigned int index);    
    extern "C" SLANGNATIVE_API void* VariableReflection_FindAttributeByName(void* variableReflection, const char* name);
    extern "C" SLANGNATIVE_API void* VariableReflection_FindUserAttributeByName(void* variableReflection, const char* name);
    extern "C" SLANGNATIVE_API bool VariableReflection_HasDefaultValue(void* variableReflection);
    extern "C" SLANGNATIVE_API SlangResult VariableReflection_GetDefaultValueInt(void* variableReflection, int64_t* value);
    extern "C" SLANGNATIVE_API void* VariableReflection_GetGenericContainer(void* variableReflection);
    extern "C" SLANGNATIVE_API void* VariableReflection_ApplySpecializations(void* variableReflection, void** specializations, int count);

    // VariableLayoutReflection API
    extern "C" SLANGNATIVE_API void VariableLayoutReflection_Release(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetNative(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetVariable(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API const char* VariableLayoutReflection_GetName(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_FindModifier(void* variableLayoutReflection, int modifierId);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetTypeLayout(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetCategory(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetCategoryCount(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetCategoryByIndex(void* variableLayoutReflection, unsigned int index);
    extern "C" SLANGNATIVE_API size_t VariableLayoutReflection_GetOffset(void* variableLayoutReflection, int category);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetType(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetBindingIndex(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetBindingSpace(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetSpace(void* variableLayoutReflection, int category);
    extern "C" SLANGNATIVE_API int VariableLayoutReflection_GetImageFormat(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API const char* VariableLayoutReflection_GetSemanticName(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API size_t VariableLayoutReflection_GetSemanticIndex(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API unsigned int VariableLayoutReflection_GetStage(void* variableLayoutReflection);
    extern "C" SLANGNATIVE_API void* VariableLayoutReflection_GetPendingDataLayout(void* variableLayoutReflection);

    // FunctionReflection API
    extern "C" SLANGNATIVE_API void FunctionReflection_Release(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetNative(void* functionReflection);
    extern "C" SLANGNATIVE_API const char* FunctionReflection_GetName(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetReturnType(void* functionReflection);
    extern "C" SLANGNATIVE_API unsigned int FunctionReflection_GetParameterCount(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetParameterByIndex(void* functionReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* FunctionReflection_FindModifier(void* functionReflection, int modifierId);
    extern "C" SLANGNATIVE_API unsigned int FunctionReflection_GetUserAttributeCount(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetUserAttributeByIndex(void* functionReflection, unsigned int index);    
    extern "C" SLANGNATIVE_API void* FunctionReflection_FindAttributeByName(void* functionReflection, const char* name);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetGenericContainer(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_ApplySpecializations(void* functionReflection, void* genRef);
    extern "C" SLANGNATIVE_API void* FunctionReflection_SpecializeWithArgTypes(void* functionReflection, unsigned int typeCount, void** types);
    extern "C" SLANGNATIVE_API bool FunctionReflection_IsOverloaded(void* functionReflection);
    extern "C" SLANGNATIVE_API unsigned int FunctionReflection_GetOverloadCount(void* functionReflection);
    extern "C" SLANGNATIVE_API void* FunctionReflection_GetOverload(void* functionReflection, unsigned int index);    

    // EntryPointReflection API
    extern "C" SLANGNATIVE_API void EntryPointReflection_Release(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetNative(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetParent(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetNative(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_AsFunction(void* entryPointReflection);
    extern "C" SLANGNATIVE_API const char* EntryPointReflection_GetName(void* entryPointReflection);
    extern "C" SLANGNATIVE_API const char* EntryPointReflection_GetNameOverride(void* entryPointReflection);
    extern "C" SLANGNATIVE_API unsigned int EntryPointReflection_GetParameterCount(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetParameterByIndex(void* entryPointReflection, unsigned int index);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetFunction(void* entryPointReflection);
    extern "C" SLANGNATIVE_API int EntryPointReflection_GetStage(void* entryPointReflection);    
    extern "C" SLANGNATIVE_API void EntryPointReflection_GetComputeThreadGroupSize(void* entryPointReflection, unsigned int axisCount, SlangUInt* outSizeAlongAxis);
    extern "C" SLANGNATIVE_API SlangResult EntryPointReflection_GetComputeWaveSize(void* entryPointReflection, SlangUInt* outWaveSize);
    extern "C" SLANGNATIVE_API bool EntryPointReflection_UsesAnySampleRateInput(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetVarLayout(void* entryPointReflection);
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetTypeLayout(void* entryPointReflection);    
    extern "C" SLANGNATIVE_API void* EntryPointReflection_GetResultVarLayout(void* entryPointReflection);
    extern "C" SLANGNATIVE_API bool EntryPointReflection_HasDefaultConstantBuffer(void* entryPointReflection);
    
    
    // GenericReflection API
    extern "C" SLANGNATIVE_API void GenericReflection_Release(void* genRefReflection);
    extern "C" SLANGNATIVE_API void* GenericReflection_GetNative(void* genRefReflection);
    extern "C" SLANGNATIVE_API const char* GenericReflection_GetName(void* genRefReflection);
    extern "C" SLANGNATIVE_API unsigned int GenericReflection_GetTypeParameterCount(void* genRefReflection);
    extern "C" SLANGNATIVE_API void* GenericReflection_GetTypeParameter(void* genRefReflection, unsigned int index);
    extern "C" SLANGNATIVE_API unsigned int GenericReflection_GetValueParameterCount(void* genRefReflection);
    extern "C" SLANGNATIVE_API void* GenericReflection_GetValueParameter(void* genRefReflection, unsigned int index);
    extern "C" SLANGNATIVE_API unsigned int GenericReflection_GetTypeParameterConstraintCount(void* genRefReflection, void* typeParam);
    extern "C" SLANGNATIVE_API void* GenericReflection_GetTypeParameterConstraintType(void* genRefReflection, void* typeParam, unsigned int index);
    extern "C" SLANGNATIVE_API int GenericReflection_GetInnerKind(void* genRefReflection);
    extern "C" SLANGNATIVE_API void* GenericReflection_GetOuterGenericContainer(void* genRefReflection);
    extern "C" SLANGNATIVE_API void* GenericReflection_GetConcreteType(void* genRefReflection, void* typeParam);
    extern "C" SLANGNATIVE_API SlangResult GenericReflection_GetConcreteIntVal(void* genRefReflection, void* valueParam, int64_t* value);
    extern "C" SLANGNATIVE_API void* GenericReflection_ApplySpecializations(void* genRefReflection, void* genRef);

    // TypeParameterReflection API
    extern "C" SLANGNATIVE_API void TypeParameterReflection_Release(void* typeParameterReflection);
    extern "C" SLANGNATIVE_API void* TypeParameterReflection_GetNative(void* typeParameterReflection);
    extern "C" SLANGNATIVE_API const char* TypeParameterReflection_GetName(void* typeParameterReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeParameterReflection_GetIndex(void* typeParameterReflection);
    extern "C" SLANGNATIVE_API unsigned int TypeParameterReflection_GetConstraintCount(void* typeParameterReflection);
    extern "C" SLANGNATIVE_API void* TypeParameterReflection_GetConstraintByIndex(void* typeParameterReflection, int index);

    // Attribute API
    extern "C" SLANGNATIVE_API void Attribute_Release(void* attributeReflection);
    extern "C" SLANGNATIVE_API void* Attribute_GetNative(void* attributeReflection);
    extern "C" SLANGNATIVE_API const char* Attribute_GetName(void* attributeReflection);
    extern "C" SLANGNATIVE_API unsigned int Attribute_GetArgumentCount(void* attributeReflection);
	extern "C" SLANGNATIVE_API void* Attribute_GetArgumentType(void* attributeReflection, unsigned int index);
    extern "C" SLANGNATIVE_API SlangResult Attribute_GetArgumentValueInt(void* attributeReflection, unsigned int index, int* value);
    extern "C" SLANGNATIVE_API SlangResult Attribute_GetArgumentValueFloat(void* attributeReflection, unsigned int index, float* value);
    extern "C" SLANGNATIVE_API const char* Attribute_GetArgumentValueString(void* attributeReflection, unsigned int index);    
    
    // Modifier API
    extern "C" SLANGNATIVE_API void Modifier_Release(void* modifier);
    extern "C" SLANGNATIVE_API void* Modifier_GetNative(void* modifier);
    extern "C" SLANGNATIVE_API int Modifier_GetID(void* modifier);
    extern "C" SLANGNATIVE_API const char* Modifier_GetName(void* modifier);
}
