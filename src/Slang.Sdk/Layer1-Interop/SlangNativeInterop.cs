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

    #endregion

    #region Module API

    [LibraryImport(LibraryName)]
    internal static partial nint CreateModule(
        nint parentSession, 
        char* moduleName,
        char* modulePath,
        char* shaderSource);

    [LibraryImport(LibraryName)]
    internal static partial void Module_Release(nint module);

    [LibraryImport(LibraryName)]
    internal static partial nint Module_FindEntryPoint(
        nint parentModule, 
        byte* entryPointName);

    [LibraryImport(LibraryName)]
    internal static partial void Module_GetParameterInfo(
        nint parentEntryPoint, 
        void** outParameterInfo, 
        int* outParameterCount);

    #endregion

    #region Program API

    [LibraryImport(LibraryName)]
    internal static partial nint Program_Create(nint parentModule);

    [LibraryImport(LibraryName)]
    internal static partial void Program_Release(nint program);

    [LibraryImport(LibraryName)]
    internal static partial SlangResult CompileProgram(
        nint program,
        uint entryPointIndex,
        uint targetIndex,
        char** output);

    [LibraryImport(LibraryName)]
    internal static partial nint GetProgramReflection(
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
    internal static partial nint ShaderReflection_FindTypeParameter(
        nint shaderReflection, 
        byte* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetParameterByIndex(
        nint shaderReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial uint ShaderReflection_GetEntryPointCount(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetEntryPointByIndex(
        nint shaderReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindEntryPointByName(
        nint shaderReflection, 
        byte* name);

    [LibraryImport(LibraryName)]
    internal static partial uint ShaderReflection_GetGlobalConstantBufferBinding(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nuint ShaderReflection_GetGlobalConstantBufferSize(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindTypeByName(
        nint shaderReflection, 
        byte* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindFunctionByName(
        nint shaderReflection, 
        byte* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindFunctionByNameInType(
        nint shaderReflection, 
        nint type, 
        byte* name);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_FindVarByNameInType(
        nint shaderReflection, 
        nint type, 
        byte* name);

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
    internal static partial byte* ShaderReflection_GetHashedString(
        nint shaderReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetGlobalParamsTypeLayout(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint ShaderReflection_GetGlobalParamsVarLayout(nint shaderReflection);

    [LibraryImport(LibraryName)]
    internal static partial int ShaderReflection_ToJson(
        nint shaderReflection, 
        byte** output);

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
    internal static partial byte* TypeReflection_GetName(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial uint TypeReflection_GetUserAttributeCount(nint typeReflection);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_GetUserAttributeByIndex(
        nint typeReflection, 
        uint index);

    [LibraryImport(LibraryName)]
    internal static partial nint TypeReflection_FindAttributeByName(
        nint typeReflection, 
        byte* name);

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
        byte* name);

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
    internal static partial byte* VariableReflection_GetName(nint variableReflection);

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
        byte* name);

    [LibraryImport(LibraryName)]
    internal static partial nint VariableReflection_FindUserAttributeByName(
        nint variableReflection, 
        byte* name);

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
    internal static partial byte* VariableLayoutReflection_GetName(nint variableLayoutReflection);

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
    internal static partial byte* VariableLayoutReflection_GetSemanticName(nint variableLayoutReflection);

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
    internal static partial byte* FunctionReflection_GetName(nint functionReflection);

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
        byte* name);

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
    internal static partial byte* EntryPointReflection_GetName(nint entryPointReflection);

    [LibraryImport(LibraryName)]
    internal static partial byte* EntryPointReflection_GetNameOverride(nint entryPointReflection);

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
    internal static partial byte* GenericReflection_GetName(nint genRefReflection);

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
    internal static partial byte* TypeParameterReflection_GetName(nint typeParameterReflection);

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
    internal static partial byte* Attribute_GetName(nint attributeReflection);

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
    internal static partial byte* Attribute_GetArgumentValueString(
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
    internal static partial byte* Modifier_GetName(nint modifier);

    #endregion
}

internal static unsafe partial class StrongTypeInterop
{

}