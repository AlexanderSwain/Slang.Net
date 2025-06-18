//#pragma once
//
//#define NOMINMAX
//#ifndef WIN32_LEAN_AND_MEAN
//#define WIN32_LEAN_AND_MEAN
//#endif
//
//#include <windows.h>
//#include "../Native/TypeDef.h"
//#include "../Native/CompilerOptionCLI.h"
//#include "../Native/PreprocessorMacroDescCLI.h"
//#include "../Native/ShaderModelCLI.h"
//
//using namespace System;
//using namespace System::Runtime::InteropServices;
//
//// Forward declare types that SlangNative uses
//typedef int SlangResult;
//
//// Define macros that were in SlangNative.h
//#define SLANG_SUCCEEDED(x) ((x) >= 0)
//
//namespace Slang::Cpp::PInvoke
//{
//    // P/Invoke declarations for SlangNative.dll functions
//    public ref class SlangNativeImports
//    {
//    public:
//        // Session API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* CreateSession(void* options, int optionsLength,
//            void* macros, int macrosLength,
//            void* models, int modelsLength,
//            char* searchPaths[], int searchPathsLength,
//            const char** error);
//
//        // Module API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* CreateModule(void* parentSession, const char* moduleName, const char* modulePath, const char* shaderSource, const char** error);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FindEntryPoint(void* parentModule, const char* entryPointName, const char** error);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void GetParameterInfo(void* parentEntryPoint, void** outParameterInfo, int* outParameterCount);
//
//        // Program API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* CreateProgram(void* parentModule);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int Compile(void* program, unsigned int entryPointIndex, unsigned int targetIndex, const char** output);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* GetProgramReflection(void* program);
//
//        // ShaderReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_GetParent(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_GetNative(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int ShaderReflection_GetParameterCount(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int ShaderReflection_GetTypeParameterCount(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_GetTypeParameterByIndex(void* shaderReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_FindTypeParameter(void* shaderReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_GetParameterByIndex(void* shaderReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int ShaderReflection_GetEntryPointCount(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_GetEntryPointByIndex(void* shaderReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_FindEntryPointByName(void* shaderReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int ShaderReflection_GetGlobalConstantBufferBinding(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t ShaderReflection_GetGlobalConstantBufferSize(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_FindTypeByName(void* shaderReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_FindFunctionByName(void* shaderReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_FindFunctionByNameInType(void* shaderReflection, void* type, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_FindVarByNameInType(void* shaderReflection, void* type, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_GetTypeLayout(void* shaderReflection, void* type, int layoutRules);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_SpecializeType(void* shaderReflection, void* type, int argCount, void** args);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static bool ShaderReflection_IsSubType(void* shaderReflection, void* subType, void* superType);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int ShaderReflection_GetHashedStringCount(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* ShaderReflection_GetHashedString(void* shaderReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_GetGlobalParamsTypeLayout(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* ShaderReflection_GetGlobalParamsVarLayout(void* shaderReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int ShaderReflection_ToJson(void* shaderReflection, const char** output);
//
//        // TypeReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_GetNative(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int TypeReflection_GetKind(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int TypeReflection_GetFieldCount(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_GetFieldByIndex(void* typeReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static bool TypeReflection_IsArray(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_UnwrapArray(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t TypeReflection_GetElementCount(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_GetElementType(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int TypeReflection_GetRowCount(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int TypeReflection_GetColumnCount(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int TypeReflection_GetScalarType(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_GetResourceResultType(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int TypeReflection_GetResourceShape(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int TypeReflection_GetResourceAccess(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* TypeReflection_GetName(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int TypeReflection_GetUserAttributeCount(void* typeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_GetUserAttributeByIndex(void* typeReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_FindAttributeByName(void* typeReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_ApplySpecializations(void* typeReflection, void* genRef);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeReflection_GetGenericContainer(void* typeReflection);
//
//        // FunctionReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_GetNative(void* functionReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* FunctionReflection_GetName(void* functionReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_GetReturnType(void* functionReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int FunctionReflection_GetParameterCount(void* functionReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_GetParameterByIndex(void* functionReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_FindModifier(void* functionReflection, int modifierId);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int FunctionReflection_GetUserAttributeCount(void* functionReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_GetUserAttributeByIndex(void* functionReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_FindAttributeByName(void* functionReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_GetGenericContainer(void* functionReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_ApplySpecializations(void* functionReflection, void* genRef);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_SpecializeWithArgTypes(void* functionReflection, unsigned int typeCount, void** types);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static bool FunctionReflection_IsOverloaded(void* functionReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int FunctionReflection_GetOverloadCount(void* functionReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* FunctionReflection_GetOverload(void* functionReflection, unsigned int index);
//
//        // Attribute API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* Attribute_GetNative(void* attributeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* Attribute_GetName(void* attributeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int Attribute_GetArgumentCount(void* attributeReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static SlangResult Attribute_GetArgumentValueInt(void* attributeReflection, unsigned int index, int* value);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static SlangResult Attribute_GetArgumentValueFloat(void* attributeReflection, unsigned int index, float* value);        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* Attribute_GetArgumentValueString(void* attributeReflection, unsigned int index);
//
//        // TypeLayoutReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeLayoutReflection_GetType(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int TypeLayoutReflection_GetKind(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t TypeLayoutReflection_GetSize(void* typeLayoutReflection, int category);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t TypeLayoutReflection_GetStride(void* typeLayoutReflection, int category);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int TypeLayoutReflection_GetAlignment(void* typeLayoutReflection, int category);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int TypeLayoutReflection_GetFieldCount(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeLayoutReflection_GetFieldByIndex(void* typeLayoutReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int TypeLayoutReflection_FindFieldIndexByName(void* typeLayoutReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeLayoutReflection_GetExplicitCounter(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static bool TypeLayoutReflection_IsArray(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeLayoutReflection_UnwrapArray(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t TypeLayoutReflection_GetElementCount(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t TypeLayoutReflection_GetTotalArrayElementCount(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t TypeLayoutReflection_GetElementStride(void* typeLayoutReflection, int category);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeLayoutReflection_GetElementTypeLayout(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeLayoutReflection_GetElementVarLayout(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeLayoutReflection_GetContainerVarLayout(void* typeLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeLayoutReflection_GetNative(void* typeLayoutReflection);
//
//        // GenericReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* GenericReflection_GetName(void* genericReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int GenericReflection_GetTypeParameterCount(void* genericReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* GenericReflection_GetTypeParameter(void* genericReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int GenericReflection_GetValueParameterCount(void* genericReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* GenericReflection_GetValueParameter(void* genericReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int GenericReflection_GetTypeParameterConstraintCount(void* genericReflection, void* typeParam);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* GenericReflection_GetTypeParameterConstraintType(void* genericReflection, void* typeParam, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int GenericReflection_GetInnerKind(void* genericReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* GenericReflection_GetOuterGenericContainer(void* genericReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* GenericReflection_GetConcreteType(void* genericReflection, void* typeParam);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static SlangResult GenericReflection_GetConcreteIntVal(void* genericReflection, void* valueParam, int64_t* value);        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* GenericReflection_ApplySpecializations(void* genericReflection, void* genRef);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* GenericReflection_GetNative(void* genericReflection);
//
//        // EntryPointReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* EntryPointReflection_GetParent(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* EntryPointReflection_GetName(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* EntryPointReflection_GetNameOverride(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int EntryPointReflection_GetParameterCount(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* EntryPointReflection_GetParameterByIndex(void* entryPointReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* EntryPointReflection_GetFunction(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int EntryPointReflection_GetStage(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void EntryPointReflection_GetComputeThreadGroupSize(void* entryPointReflection, unsigned int axisCount, unsigned int* outSizes);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void EntryPointReflection_GetComputeWaveSize(void* entryPointReflection, unsigned int* outWaveSize);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static bool EntryPointReflection_UsesAnySampleRateInput(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* EntryPointReflection_GetVarLayout(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* EntryPointReflection_GetTypeLayout(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* EntryPointReflection_GetResultVarLayout(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static bool EntryPointReflection_HasDefaultConstantBuffer(void* entryPointReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* EntryPointReflection_GetNative(void* entryPointReflection);
//
//        // TypeParameterReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* TypeParameterReflection_GetName(void* typeParameterReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int TypeParameterReflection_GetIndex(void* typeParameterReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int TypeParameterReflection_GetConstraintCount(void* typeParameterReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeParameterReflection_GetConstraintByIndex(void* typeParameterReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* TypeParameterReflection_GetNative(void* typeParameterReflection);
//
//        // VariableLayoutReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableLayoutReflection_GetVariable(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* VariableLayoutReflection_GetName(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableLayoutReflection_FindModifier(void* variableLayoutReflection, int modifierId);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableLayoutReflection_GetTypeLayout(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int VariableLayoutReflection_GetCategory(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int VariableLayoutReflection_GetCategoryCount(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int VariableLayoutReflection_GetCategoryByIndex(void* variableLayoutReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t VariableLayoutReflection_GetOffset(void* variableLayoutReflection, int category);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableLayoutReflection_GetType(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int VariableLayoutReflection_GetBindingIndex(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int VariableLayoutReflection_GetBindingSpace(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableLayoutReflection_GetSpace(void* variableLayoutReflection, int category);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static int VariableLayoutReflection_GetImageFormat(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* VariableLayoutReflection_GetSemanticName(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static size_t VariableLayoutReflection_GetSemanticIndex(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int VariableLayoutReflection_GetStage(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableLayoutReflection_GetPendingDataLayout(void* variableLayoutReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableLayoutReflection_GetNative(void* variableLayoutReflection);
//
//        // VariableReflection API
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static const char* VariableReflection_GetName(void* variableReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableReflection_GetType(void* variableReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableReflection_FindModifier(void* variableReflection, int modifierId);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static unsigned int VariableReflection_GetUserAttributeCount(void* variableReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableReflection_GetUserAttributeByIndex(void* variableReflection, unsigned int index);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableReflection_FindAttributeByName(void* variableReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableReflection_FindUserAttributeByName(void* variableReflection, const char* name);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static bool VariableReflection_HasDefaultValue(void* variableReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static SlangResult VariableReflection_GetDefaultValueInt(void* variableReflection, int* value);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableReflection_GetGenericContainer(void* variableReflection);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableReflection_ApplySpecializations(void* variableReflection, void* genRef);
//
//        [DllImport("SlangNative.dll", CallingConvention = CallingConvention::Cdecl)]
//        static void* VariableReflection_GetNative(void* variableReflection);
//
//        // Add more P/Invoke declarations as needed for other SlangNative functions
//    };
//}
