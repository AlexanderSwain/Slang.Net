using System.Runtime.InteropServices;

namespace Slang.Sdk.Interop;

/// <summary>
/// P/Invoke declarations for SlangNative.dll functions.
/// These provide direct access to the native Slang functionality.
/// </summary>
internal static unsafe partial class SlangNativeInterop
{
    private const string LibraryName = "SlangNative";

    #region Diagnostics

    [LibraryImport(LibraryName)]
    internal static partial char* SlangNative_GetLastError();

    #endregion

    #region Session API

    [LibraryImport(LibraryName)]
    internal static partial nint Session_Create(
        void* options, int optionsLength,
        void* macros, int macrosLength,
        void* models, int modelsLength,
        char*[] searchPaths, int searchPathsLength);

    [LibraryImport(LibraryName)]
    internal static partial void Session_Release(nint session);

    [LibraryImport(LibraryName)]
    internal static partial uint Session_GetModuleCount(nint module);

    [LibraryImport(LibraryName)]
    internal static partial nint Session_GetModuleByIndex(nint module, uint index);

    #endregion

    #region Module API

    [LibraryImport(LibraryName)]
    internal static partial nint Module_Create(
        nint parentSession, 
        char* moduleName,
        char* modulePath,
        char* shaderSource);

    [LibraryImport(LibraryName)]
    internal static partial nint Module_Import(
    nint parentSession,
    char* moduleName);

    [LibraryImport(LibraryName)]
    internal static partial void Module_Release(nint module);

    [LibraryImport(LibraryName)]
    internal static partial uint Module_GetEntryPointCount(nint parentModule);

    [LibraryImport(LibraryName)]
    internal static partial nint Module_GetEntryPointByIndex(
        nint parentModule, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint Module_FindEntryPointByName(
        nint parentModule, 
        char* entryPointName);

    #endregion

    #region EntryPoint API

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPoint_Create(
        nint parentModule, 
        uint entryPointIndex);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPoint_CreateByName(
        nint parentModule, 
        char* entryPointName);

    [LibraryImport(LibraryName)]
    internal static partial void EntryPoint_Release(nint entryPoint);

    [LibraryImport(LibraryName)]
    internal static partial int EntryPoint_GetIndex(nint entryPoint);

    [LibraryImport(LibraryName)]
    internal static partial char* EntryPoint_GetName(nint entryPoint);

    #endregion

    #region Program API

    [LibraryImport(LibraryName)]
    internal static partial nint Program_Create(nint parentModule);

    [LibraryImport(LibraryName)]
    internal static partial void Program_Release(nint program);

    [LibraryImport(LibraryName)]
    internal static partial SlangResult Program_CompileProgram(
        nint program,
        uint entryPointIndex,
        uint targetIndex,
        char** output);

    [LibraryImport(LibraryName)]
    internal static partial nint Program_GetProgramReflection(
        nint program, 
        uint targetIndex);

    #endregion

    #region ShaderReflection API

    [LibraryImport(LibraryName)]
    internal static partial void ShaderReflection_Release(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetParent(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetNative(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint ShaderReflection_GetParameterCount(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint ShaderReflection_GetTypeParameterCount(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetTypeParameterByIndex(
        nint shaderReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindTypeParameter(nint shaderReflection, char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetParameterByIndex(nint shaderReflection, uint index);

    [LibraryImport(LibraryName)]
    internal static partial uint ShaderReflection_GetEntryPointCount(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetEntryPointByIndex(
        nint shaderReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindEntryPointByName(
        nint shaderReflection, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial uint ShaderReflection_GetGlobalConstantBufferBinding(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nuint ShaderReflection_GetGlobalConstantBufferSize(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindTypeByName(
        nint shaderReflection, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindFunctionByName(
        nint shaderReflection, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindFunctionByNameInType(
        nint shaderReflection, 
        nint type, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindVarByNameInType(
        nint shaderReflection, 
        nint type, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetTypeLayout(
        nint shaderReflection, 
        nint type, 
        int layoutRules);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_SpecializeType(
        nint shaderReflection, 
        nint type, 
        int argCount, 
        void** args);

    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool ShaderReflection_IsSubType(
        nint shaderReflection, 
        nint subType, 
        nint superType);

    [LibraryImport(LibraryName)]
    internal static partial uint ShaderReflection_GetHashedStringCount(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* ShaderReflection_GetHashedString(
        nint shaderReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetGlobalParamsTypeLayout(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetGlobalParamsVarLayout(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial int ShaderReflection_ToJson(
        nint shaderReflection, 
        char** output);

    #endregion

    #region TypeReflection API

    [LibraryImport(LibraryName)]
    internal static partial void TypeReflection_Release(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_GetNative(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial int TypeReflection_GetKind(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint TypeReflection_GetFieldCount(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_GetFieldByIndex(
        nint typeReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool TypeReflection_IsArray(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_UnwrapArray(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nuint TypeReflection_GetElementCount(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_GetElementType(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint TypeReflection_GetRowCount(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint TypeReflection_GetColumnCount(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial int TypeReflection_GetScalarType(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_GetResourceResultType(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial int TypeReflection_GetResourceShape(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial int TypeReflection_GetResourceAccess(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* TypeReflection_GetName(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint TypeReflection_GetUserAttributeCount(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_GetUserAttributeByIndex(
        nint typeReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_FindAttributeByName(
        nint typeReflection, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_ApplySpecializations(
        nint typeReflection, 
        nint genRef);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_GetGenericContainer(nint typeReflection);

    #endregion

    #region TypeLayoutReflection API

    [LibraryImport(LibraryName)]
    internal static partial void TypeLayoutReflection_Release(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeLayoutReflection_GetNative(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeLayoutReflection_GetType(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial int TypeLayoutReflection_GetKind(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nuint TypeLayoutReflection_GetSize(
        nint typeLayoutReflection, 
        int category);

    [LibraryImport(LibraryName)]
    internal static partial nuint TypeLayoutReflection_GetStride(
        nint typeLayoutReflection, 
        int category);

    [LibraryImport(LibraryName)]
    internal static partial int TypeLayoutReflection_GetAlignment(
        nint typeLayoutReflection, 
        int category);

    [LibraryImport(LibraryName)]
    internal static partial uint TypeLayoutReflection_GetFieldCount(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeLayoutReflection_GetFieldByIndex(
        nint typeLayoutReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial int TypeLayoutReflection_FindFieldIndexByName(
        nint typeLayoutReflection, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeLayoutReflection_GetExplicitCounter(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool TypeLayoutReflection_IsArray(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeLayoutReflection_UnwrapArray(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nuint TypeLayoutReflection_GetElementCount(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nuint TypeLayoutReflection_GetTotalArrayElementCount(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nuint TypeLayoutReflection_GetElementStride(
        nint typeLayoutReflection, 
        int category);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeLayoutReflection_GetElementTypeLayout(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeLayoutReflection_GetElementVarLayout(nint typeLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeLayoutReflection_GetContainerVarLayout(nint typeLayoutReflection);

    #endregion

    #region VariableReflection API

    [LibraryImport(LibraryName)]
    internal static partial void VariableReflection_Release(nint variableReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_GetNative(nint variableReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* VariableReflection_GetName(nint variableReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_GetType(nint variableReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_FindModifier(
        nint variableReflection, 
        int modifierId);

    [LibraryImport(LibraryName)]
    internal static partial uint VariableReflection_GetUserAttributeCount(nint variableReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_GetUserAttributeByIndex(
        nint variableReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_FindAttributeByName(
        nint variableReflection, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_FindUserAttributeByName(
        nint variableReflection, 
        char* name);

    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool VariableReflection_HasDefaultValue(nint variableReflection);

    [LibraryImport(LibraryName)]
    internal static partial int VariableReflection_GetDefaultValueInt(
        nint variableReflection, 
        long* value);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_GetGenericContainer(nint variableReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_ApplySpecializations(
        nint variableReflection, 
        void** specializations, 
        int count);

    #endregion

    #region VariableLayoutReflection API

    [LibraryImport(LibraryName)]
    internal static partial void VariableLayoutReflection_Release(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableLayoutReflection_GetNative(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableLayoutReflection_GetVariable(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* VariableLayoutReflection_GetName(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableLayoutReflection_FindModifier(
        nint variableLayoutReflection, 
        int modifierId);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableLayoutReflection_GetTypeLayout(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial int VariableLayoutReflection_GetCategory(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint VariableLayoutReflection_GetCategoryCount(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial int VariableLayoutReflection_GetCategoryByIndex(
        nint variableLayoutReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nuint VariableLayoutReflection_GetOffset(
        nint variableLayoutReflection, 
        int category);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableLayoutReflection_GetType(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint VariableLayoutReflection_GetBindingIndex(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint VariableLayoutReflection_GetBindingSpace(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableLayoutReflection_GetSpace(
        nint variableLayoutReflection, 
        int category);

    [LibraryImport(LibraryName)]
    internal static partial int VariableLayoutReflection_GetImageFormat(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* VariableLayoutReflection_GetSemanticName(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nuint VariableLayoutReflection_GetSemanticIndex(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint VariableLayoutReflection_GetStage(nint variableLayoutReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableLayoutReflection_GetPendingDataLayout(nint variableLayoutReflection);

    #endregion

    #region FunctionReflection API

    [LibraryImport(LibraryName)]
    internal static partial void FunctionReflection_Release(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_GetNative(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* FunctionReflection_GetName(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_GetReturnType(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint FunctionReflection_GetParameterCount(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_GetParameterByIndex(
        nint functionReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_FindModifier(
        nint functionReflection, 
        int modifierId);

    [LibraryImport(LibraryName)]
    internal static partial uint FunctionReflection_GetUserAttributeCount(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_GetUserAttributeByIndex(
        nint functionReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_FindAttributeByName(
        nint functionReflection, 
        char* name);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_GetGenericContainer(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_ApplySpecializations(
        nint functionReflection, 
        nint genRef);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_SpecializeWithArgTypes(
        nint functionReflection, 
        uint typeCount, 
        void** types);

    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool FunctionReflection_IsOverloaded(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint FunctionReflection_GetOverloadCount(nint functionReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint FunctionReflection_GetOverload(
        nint functionReflection, 
        uint index);

    #endregion

    #region EntryPointReflection API

    [LibraryImport(LibraryName)]
    internal static partial void EntryPointReflection_Release(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPointReflection_GetNative(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPointReflection_GetParent(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPointReflection_AsFunction(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* EntryPointReflection_GetName(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* EntryPointReflection_GetNameOverride(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint EntryPointReflection_GetParameterCount(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPointReflection_GetParameterByIndex(
        nint entryPointReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPointReflection_GetFunction(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial int EntryPointReflection_GetStage(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial void EntryPointReflection_GetComputeThreadGroupSize(
        nint entryPointReflection, 
        uint axisCount, 
        ulong* outSizeAlongAxis);

    [LibraryImport(LibraryName)]
    internal static partial int EntryPointReflection_GetComputeWaveSize(
        nint entryPointReflection, 
        ulong* outWaveSize);

    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool EntryPointReflection_UsesAnySampleRateInput(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPointReflection_GetVarLayout(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPointReflection_GetTypeLayout(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint EntryPointReflection_GetResultVarLayout(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool EntryPointReflection_HasDefaultConstantBuffer(nint entryPointReflection);

    #endregion

    #region GenericReflection API

    [LibraryImport(LibraryName)]
    internal static partial void GenericReflection_Release(nint genRefReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint GenericReflection_GetNative(nint genRefReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* GenericReflection_GetName(nint genRefReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint GenericReflection_GetTypeParameterCount(nint genRefReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint GenericReflection_GetTypeParameter(
        nint genRefReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial uint GenericReflection_GetValueParameterCount(nint genRefReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint GenericReflection_GetValueParameter(
        nint genRefReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial uint GenericReflection_GetTypeParameterConstraintCount(
        nint genRefReflection, 
        nint typeParam);

    [LibraryImport(LibraryName)]
    internal static partial nint GenericReflection_GetTypeParameterConstraintType(
        nint genRefReflection, 
        nint typeParam, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial int GenericReflection_GetInnerKind(nint genRefReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint GenericReflection_GetOuterGenericContainer(nint genRefReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint GenericReflection_GetConcreteType(
        nint genRefReflection, 
        nint typeParam);

    [LibraryImport(LibraryName)]
    internal static partial int GenericReflection_GetConcreteIntVal(
        nint genRefReflection, 
        nint valueParam, 
        long* value);

    [LibraryImport(LibraryName)]
    internal static partial nint GenericReflection_ApplySpecializations(
        nint genRefReflection, 
        nint genRef);

    #endregion

    #region TypeParameterReflection API

    [LibraryImport(LibraryName)]
    internal static partial void TypeParameterReflection_Release(nint typeParameterReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeParameterReflection_GetNative(nint typeParameterReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* TypeParameterReflection_GetName(nint typeParameterReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint TypeParameterReflection_GetIndex(nint typeParameterReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint TypeParameterReflection_GetConstraintCount(nint typeParameterReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeParameterReflection_GetConstraintByIndex(
        nint typeParameterReflection, 
        int index);

    #endregion

    #region Attribute API

    [LibraryImport(LibraryName)]
    internal static partial void Attribute_Release(nint attributeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint Attribute_GetNative(nint attributeReflection);

    [LibraryImport(LibraryName)]
    internal static partial char* Attribute_GetName(nint attributeReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint Attribute_GetArgumentCount(nint attributeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint Attribute_GetArgumentType(
        nint attributeReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial int Attribute_GetArgumentValueInt(
        nint attributeReflection, 
        uint index, 
        int* value);

    [LibraryImport(LibraryName)]
    internal static partial int Attribute_GetArgumentValueFloat(
        nint attributeReflection, 
        uint index, 
        float* value);

    [LibraryImport(LibraryName)]
    internal static partial char* Attribute_GetArgumentValueString(
        nint attributeReflection, 
        uint index);

    #endregion

    #region Modifier API

    [LibraryImport(LibraryName)]
    internal static partial void Modifier_Release(nint modifier);

    [LibraryImport(LibraryName)]
    internal static partial nint Modifier_GetNative(nint modifier);

    [LibraryImport(LibraryName)]
    internal static partial int Modifier_GetID(nint modifier);

    [LibraryImport(LibraryName)]
    internal static partial char* Modifier_GetName(nint modifier);

    #endregion
}

internal static unsafe partial class StrongTypeInterop
{
    #region Session API

    /// <summary>
    /// Creates a Slang session with strongly-typed handle.
    /// </summary>
    internal static SessionHandle Session_Create(
        void* options, int optionsLength,
        void* macros, int macrosLength,
        void* models, int modelsLength,
        char*[] searchPaths, int searchPathsLength)
    {
        var handle = SlangNativeInterop.Session_Create(options, optionsLength, macros, macrosLength, models, modelsLength, searchPaths, searchPathsLength);
        return new SessionHandle(handle);
    }

    /// <summary>
    /// Finds a previously loaded module by its index
    /// </summary>
    /// <param name="module"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    internal static ModuleHandle Session_GetModuleByIndex(nint module, uint index)
    {
        var handle = SlangNativeInterop.Session_GetModuleByIndex(module, index);
        return new ModuleHandle(handle);
    }

    #endregion

    #region Module API

    /// <summary>
    /// Creates a module with strongly-typed handle.
    /// </summary>
    internal static ModuleHandle Module_Create(
        nint parentSession,
        char* moduleName,
        char* modulePath,
        char* shaderSource)
    {
        var handle = SlangNativeInterop.Module_Create(parentSession, moduleName, modulePath, shaderSource);
        return new ModuleHandle(handle);
    }

    /// <summary>
    /// Creates a module with strongly-typed handle.
    /// </summary>
    internal static ModuleHandle Module_Import(
        nint parentSession,
        char* moduleName)
    {
        var handle = SlangNativeInterop.Module_Import(parentSession, moduleName);
        return new ModuleHandle(handle);
    }

    /// <summary>
    /// Gets entry point by index with strongly-typed handle.
    /// </summary>
    internal static EntryPointHandle Module_GetEntryPointByIndex(
        nint parentModule,
        uint index)
    {
        var handle = SlangNativeInterop.Module_GetEntryPointByIndex(parentModule, index);
        return new EntryPointHandle(handle);
    }

    /// <summary>
    /// Finds entry point by name with strongly-typed handle.
    /// </summary>
    internal static EntryPointHandle Module_FindEntryPointByName(
        nint parentModule,
        char* entryPointName)
    {
        var handle = SlangNativeInterop.Module_FindEntryPointByName(parentModule, entryPointName);
        return new EntryPointHandle(handle);
    }

    #endregion

    #region EntryPoint API

    /// <summary>
    /// Creates an entry point with strongly-typed handle.
    /// </summary>
    internal static EntryPointHandle EntryPoint_Create(
        nint parentModule,
        uint entryPointIndex)
    {
        var handle = SlangNativeInterop.EntryPoint_Create(parentModule, entryPointIndex);
        return new EntryPointHandle(handle);
    }

    /// <summary>
    /// Creates an entry point by name with strongly-typed handle.
    /// </summary>
    internal static EntryPointHandle EntryPoint_CreateByName(
        nint parentModule,
        char* entryPointName)
    {
        var handle = SlangNativeInterop.EntryPoint_CreateByName(parentModule, entryPointName);
        return new EntryPointHandle(handle);
    }

    #endregion

    #region Program API

    /// <summary>
    /// Creates a program with strongly-typed handle.
    /// </summary>
    internal static ProgramHandle Program_Create(nint parentModule)
    {
        var handle = SlangNativeInterop.Program_Create(parentModule);
        return new ProgramHandle(handle);
    }

    /// <summary>
    /// Gets program reflection with strongly-typed handle.
    /// </summary>
    internal static ShaderReflectionHandle Program_GetProgramReflection(
        nint program,
        uint targetIndex)
    {
        var handle = SlangNativeInterop.Program_GetProgramReflection(program, targetIndex);
        return new ShaderReflectionHandle(handle);
    }

    #endregion

    #region ShaderReflection API

    /// <summary>
    /// Gets shader reflection parent with strongly-typed handle.
    /// </summary>
    internal static ShaderReflectionHandle ShaderReflection_GetParent(nint shaderReflection)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetParent(shaderReflection);
        return new ShaderReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type parameter by index with strongly-typed handle.
    /// </summary>
    internal static TypeParameterReflectionHandle ShaderReflection_GetTypeParameterByIndex(
        nint shaderReflection,
        uint index)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetTypeParameterByIndex(shaderReflection, index);
        return new TypeParameterReflectionHandle(handle);
    }

    /// <summary>
    /// Finds type parameter with strongly-typed handle.
    /// </summary>
    internal static TypeParameterReflectionHandle ShaderReflection_FindTypeParameter(nint shaderReflection, char* name)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindTypeParameter(shaderReflection, name);
        return new TypeParameterReflectionHandle(handle);
    }

    /// <summary>
    /// Gets parameter by index with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle ShaderReflection_GetParameterByIndex(
        nint shaderReflection,
        uint index)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetParameterByIndex(shaderReflection, index);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point by index with strongly-typed handle.
    /// </summary>
    internal static EntryPointReflectionHandle ShaderReflection_GetEntryPointByIndex(
        nint shaderReflection,
        uint index)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetEntryPointByIndex(shaderReflection, index);
        return new EntryPointReflectionHandle(handle);
    }

    /// <summary>
    /// Finds entry point by name with strongly-typed handle.
    /// </summary>
    internal static EntryPointReflectionHandle ShaderReflection_FindEntryPointByName(
        nint shaderReflection,
        char* name)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindEntryPointByName(shaderReflection, name);
        return new EntryPointReflectionHandle(handle);
    }

    /// <summary>
    /// Finds type by name with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle ShaderReflection_FindTypeByName(
        nint shaderReflection,
        char* name)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindTypeByName(shaderReflection, name);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds function by name with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle ShaderReflection_FindFunctionByName(
        nint shaderReflection,
        char* name)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindFunctionByName(shaderReflection, name);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Finds function by name in type with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle ShaderReflection_FindFunctionByNameInType(
        nint shaderReflection,
        nint type,
        char* name)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindFunctionByNameInType(shaderReflection, type, name);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Finds variable by name in type with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle ShaderReflection_FindVarByNameInType(
        nint shaderReflection,
        nint type,
        char* name)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindVarByNameInType(shaderReflection, type, name);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle ShaderReflection_GetTypeLayout(
        nint shaderReflection,
        nint type,
        int layoutRules)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetTypeLayout(shaderReflection, type, layoutRules);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Specializes type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle ShaderReflection_SpecializeType(
        nint shaderReflection,
        nint type,
        int argCount,
        void** args)
    {
        var handle = SlangNativeInterop.ShaderReflection_SpecializeType(shaderReflection, type, argCount, args);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets global params type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle ShaderReflection_GetGlobalParamsTypeLayout(nint shaderReflection)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetGlobalParamsTypeLayout(shaderReflection);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets global params var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle ShaderReflection_GetGlobalParamsVarLayout(nint shaderReflection)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetGlobalParamsVarLayout(shaderReflection);
        return new VariableLayoutReflectionHandle(handle);
    }

    #endregion

    #region TypeReflection API

    /// <summary>
    /// Gets type reflection field by index with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle TypeReflection_GetFieldByIndex(
        nint typeReflection,
        uint index)
    {
        var handle = SlangNativeInterop.TypeReflection_GetFieldByIndex(typeReflection, index);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Unwraps array type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeReflection_UnwrapArray(nint typeReflection)
    {
        var handle = SlangNativeInterop.TypeReflection_UnwrapArray(typeReflection);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets element type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeReflection_GetElementType(nint typeReflection)
    {
        var handle = SlangNativeInterop.TypeReflection_GetElementType(typeReflection);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets resource result type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeReflection_GetResourceResultType(nint typeReflection)
    {
        var handle = SlangNativeInterop.TypeReflection_GetResourceResultType(typeReflection);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets user attribute by index with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle TypeReflection_GetUserAttributeByIndex(
        nint typeReflection,
        uint index)
    {
        var handle = SlangNativeInterop.TypeReflection_GetUserAttributeByIndex(typeReflection, index);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds attribute by name with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle TypeReflection_FindAttributeByName(
        nint typeReflection,
        char* name)
    {
        var handle = SlangNativeInterop.TypeReflection_FindAttributeByName(typeReflection, name);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Applies specializations with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeReflection_ApplySpecializations(
        nint typeReflection,
        nint genRef)
    {
        var handle = SlangNativeInterop.TypeReflection_ApplySpecializations(typeReflection, genRef);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets generic container with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle TypeReflection_GetGenericContainer(nint typeReflection)
    {
        var handle = SlangNativeInterop.TypeReflection_GetGenericContainer(typeReflection);
        return new GenericReflectionHandle(handle);
    }

    #endregion

    #region TypeLayoutReflection API

    /// <summary>
    /// Gets type layout type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeLayoutReflection_GetType(nint typeLayoutReflection)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetType(typeLayoutReflection);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type layout field by index with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle TypeLayoutReflection_GetFieldByIndex(
        nint typeLayoutReflection,
        uint index)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetFieldByIndex(typeLayoutReflection, index);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets explicit counter with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle TypeLayoutReflection_GetExplicitCounter(nint typeLayoutReflection)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetExplicitCounter(typeLayoutReflection);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Unwraps array type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle TypeLayoutReflection_UnwrapArray(nint typeLayoutReflection)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_UnwrapArray(typeLayoutReflection);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets element type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle TypeLayoutReflection_GetElementTypeLayout(nint typeLayoutReflection)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetElementTypeLayout(typeLayoutReflection);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets element var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle TypeLayoutReflection_GetElementVarLayout(nint typeLayoutReflection)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetElementVarLayout(typeLayoutReflection);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets container var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle TypeLayoutReflection_GetContainerVarLayout(nint typeLayoutReflection)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetContainerVarLayout(typeLayoutReflection);
        return new VariableLayoutReflectionHandle(handle);
    }

    #endregion

    #region VariableReflection API

    /// <summary>
    /// Gets variable type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle VariableReflection_GetType(nint variableReflection)
    {
        var handle = SlangNativeInterop.VariableReflection_GetType(variableReflection);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds modifier with strongly-typed handle.
    /// </summary>
    internal static ModifierReflectionHandle VariableReflection_FindModifier(
        nint variableReflection,
        int modifierId)
    {
        var handle = SlangNativeInterop.VariableReflection_FindModifier(variableReflection, modifierId);
        return new ModifierReflectionHandle(handle);
    }

    /// <summary>
    /// Gets user attribute by index with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle VariableReflection_GetUserAttributeByIndex(
        nint variableReflection,
        uint index)
    {
        var handle = SlangNativeInterop.VariableReflection_GetUserAttributeByIndex(variableReflection, index);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds attribute by name with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle VariableReflection_FindAttributeByName(
        nint variableReflection,
        char* name)
    {
        var handle = SlangNativeInterop.VariableReflection_FindAttributeByName(variableReflection, name);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds user attribute by name with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle VariableReflection_FindUserAttributeByName(
        nint variableReflection,
        char* name)
    {
        var handle = SlangNativeInterop.VariableReflection_FindUserAttributeByName(variableReflection, name);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets generic container with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle VariableReflection_GetGenericContainer(nint variableReflection)
    {
        var handle = SlangNativeInterop.VariableReflection_GetGenericContainer(variableReflection);
        return new GenericReflectionHandle(handle);
    }

    /// <summary>
    /// Applies specializations with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle VariableReflection_ApplySpecializations(
        nint variableReflection,
        void** specializations,
        int count)
    {
        var handle = SlangNativeInterop.VariableReflection_ApplySpecializations(variableReflection, specializations, count);
        return new VariableReflectionHandle(handle);
    }

    #endregion

    #region VariableLayoutReflection API

    /// <summary>
    /// Gets variable layout variable with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle VariableLayoutReflection_GetVariable(nint variableLayoutReflection)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetVariable(variableLayoutReflection);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Finds modifier with strongly-typed handle.
    /// </summary>
    internal static ModifierReflectionHandle VariableLayoutReflection_FindModifier(
        nint variableLayoutReflection,
        int modifierId)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_FindModifier(variableLayoutReflection, modifierId);
        return new ModifierReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle VariableLayoutReflection_GetTypeLayout(nint variableLayoutReflection)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetTypeLayout(variableLayoutReflection);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle VariableLayoutReflection_GetType(nint variableLayoutReflection)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetType(variableLayoutReflection);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets space with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle VariableLayoutReflection_GetSpace(
        nint variableLayoutReflection,
        int category)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetSpace(variableLayoutReflection, category);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets pending data layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle VariableLayoutReflection_GetPendingDataLayout(nint variableLayoutReflection)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetPendingDataLayout(variableLayoutReflection);
        return new VariableLayoutReflectionHandle(handle);
    }

    #endregion

    #region FunctionReflection API

    /// <summary>
    /// Gets function return type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle FunctionReflection_GetReturnType(nint functionReflection)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetReturnType(functionReflection);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets function parameter by index with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle FunctionReflection_GetParameterByIndex(
        nint functionReflection,
        uint index)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetParameterByIndex(functionReflection, index);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Finds modifier with strongly-typed handle.
    /// </summary>
    internal static ModifierReflectionHandle FunctionReflection_FindModifier(
        nint functionReflection,
        int modifierId)
    {
        var handle = SlangNativeInterop.FunctionReflection_FindModifier(functionReflection, modifierId);
        return new ModifierReflectionHandle(handle);
    }

    /// <summary>
    /// Gets user attribute by index with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle FunctionReflection_GetUserAttributeByIndex(
        nint functionReflection,
        uint index)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetUserAttributeByIndex(functionReflection, index);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds attribute by name with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle FunctionReflection_FindAttributeByName(
        nint functionReflection,
        char* name)
    {
        var handle = SlangNativeInterop.FunctionReflection_FindAttributeByName(functionReflection, name);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets generic container with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle FunctionReflection_GetGenericContainer(nint functionReflection)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetGenericContainer(functionReflection);
        return new GenericReflectionHandle(handle);
    }

    /// <summary>
    /// Applies specializations with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle FunctionReflection_ApplySpecializations(
        nint functionReflection,
        nint genRef)
    {
        var handle = SlangNativeInterop.FunctionReflection_ApplySpecializations(functionReflection, genRef);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Specializes with arg types with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle FunctionReflection_SpecializeWithArgTypes(
        nint functionReflection,
        uint typeCount,
        void** types)
    {
        var handle = SlangNativeInterop.FunctionReflection_SpecializeWithArgTypes(functionReflection, typeCount, types);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Gets function overload with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle FunctionReflection_GetOverload(
        nint functionReflection,
        uint index)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetOverload(functionReflection, index);
        return new FunctionReflectionHandle(handle);
    }

    #endregion

    #region EntryPointReflection API

    /// <summary>
    /// Gets entry point parent with strongly-typed handle.
    /// </summary>
    internal static ShaderReflectionHandle EntryPointReflection_GetParent(nint entryPointReflection)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetParent(entryPointReflection);
        return new ShaderReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point as function with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle EntryPointReflection_AsFunction(nint entryPointReflection)
    {
        var handle = SlangNativeInterop.EntryPointReflection_AsFunction(entryPointReflection);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point parameter by index with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle EntryPointReflection_GetParameterByIndex(
        nint entryPointReflection,
        uint index)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetParameterByIndex(entryPointReflection, index);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point function with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle EntryPointReflection_GetFunction(nint entryPointReflection)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetFunction(entryPointReflection);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle EntryPointReflection_GetVarLayout(nint entryPointReflection)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetVarLayout(entryPointReflection);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle EntryPointReflection_GetTypeLayout(nint entryPointReflection)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetTypeLayout(entryPointReflection);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point result var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle EntryPointReflection_GetResultVarLayout(nint entryPointReflection)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetResultVarLayout(entryPointReflection);
        return new VariableLayoutReflectionHandle(handle);
    }

    #endregion

    #region GenericReflection API

    /// <summary>
    /// Gets generic type parameter with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle GenericReflection_GetTypeParameter(
        nint genRefReflection,
        uint index)
    {
        var handle = SlangNativeInterop.GenericReflection_GetTypeParameter(genRefReflection, index);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Gets generic value parameter with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle GenericReflection_GetValueParameter(
        nint genRefReflection,
        uint index)
    {
        var handle = SlangNativeInterop.GenericReflection_GetValueParameter(genRefReflection, index);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type parameter constraint type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle GenericReflection_GetTypeParameterConstraintType(
        nint genRefReflection,
        nint typeParam,
        uint index)
    {
        var handle = SlangNativeInterop.GenericReflection_GetTypeParameterConstraintType(genRefReflection, typeParam, index);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets outer generic container with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle GenericReflection_GetOuterGenericContainer(nint genRefReflection)
    {
        var handle = SlangNativeInterop.GenericReflection_GetOuterGenericContainer(genRefReflection);
        return new GenericReflectionHandle(handle);
    }

    /// <summary>
    /// Gets concrete type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle GenericReflection_GetConcreteType(
        nint genRefReflection,
        nint typeParam)
    {
        var handle = SlangNativeInterop.GenericReflection_GetConcreteType(genRefReflection, typeParam);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Applies specializations with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle GenericReflection_ApplySpecializations(
        nint genRefReflection,
        nint genRef)
    {
        var handle = SlangNativeInterop.GenericReflection_ApplySpecializations(genRefReflection, genRef);
        return new GenericReflectionHandle(handle);
    }

    #endregion

    #region TypeParameterReflection API

    /// <summary>
    /// Gets constraint by index with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeParameterReflection_GetConstraintByIndex(
        nint typeParameterReflection,
        int index)
    {
        var handle = SlangNativeInterop.TypeParameterReflection_GetConstraintByIndex(typeParameterReflection, index);
        return new TypeReflectionHandle(handle);
    }

    #endregion

    #region Attribute API

    /// <summary>
    /// Gets argument type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle Attribute_GetArgumentType(
        nint attributeReflection,
        uint index)
    {
        var handle = SlangNativeInterop.Attribute_GetArgumentType(attributeReflection, index);
        return new TypeReflectionHandle(handle);
    }

    #endregion
}