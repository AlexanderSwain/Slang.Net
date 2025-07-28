using System.Runtime.InteropServices;

namespace Slang.Sdk.Interop;

/// <summary>
/// P/Invoke declarations for SlangNative.dll functions.
/// These provide direct access to the native Slang functionality.
/// </summary>
internal static unsafe partial class SlangNativeInterop
{
    private const string LibraryName = "SlangNative";

    static SlangNativeInterop()
    {
        IntPtr DllImportResolver(string libraryName, System.Reflection.Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == LibraryName)
            {
                // Use the Runtime.Directory to get the correct runtime path
                var runtimeDirectory = Slang.Runtime.SlangNative_Directory;
                var libraryPath = Path.Combine(runtimeDirectory, "SlangNative.dll");

                if (File.Exists(libraryPath))
                {
                    return NativeLibrary.Load(libraryPath);
                }
            }

            // Fall back to default resolution
            return IntPtr.Zero;
        }

        // Register a custom DLL resolver to look in the runtime-specific directory
        // This is required for testing Slang.Sdk as an executable
        NativeLibrary.SetDllImportResolver(typeof(SlangNativeInterop).Assembly, DllImportResolver);
    }

    #region Session API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Session_Create(
        void* options, int optionsLength,
        void* macros, int macrosLength,
        void* models, int modelsLength,
        string[] searchPaths, int searchPathsLength, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void Session_Release(nint session, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint Session_GetModuleCount(nint module, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Session_GetModuleByIndex(nint module, uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Session_GetNative(nint session, out string error);

    #endregion

    #region Module API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Module_Create(
        nint parentSession, 
        string moduleName,
        string modulePath,
        string shaderSource, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Module_Import(
    nint parentSession,
    string moduleName, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void Module_Release(nint mod, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string Module_GetName(nint mod, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint Module_GetEntryPointCount(nint mod, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Module_GetEntryPointByIndex(nint mod, uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Module_FindEntryPointByName(nint mod, string entryPointName, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Module_GetNative(nint mod, out string error);

    #endregion

    #region EntryPoint API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPoint_Create(
        nint parentModule, 
        uint entryPointIndex, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPoint_CreateByName(
        nint parentModule, 
        string entryPointName, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void EntryPoint_Release(nint entryPoint, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int EntryPoint_GetIndex(nint entryPoint, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string EntryPoint_GetName(nint entryPoint, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial SlangResult EntryPoint_Compile(nint entryPoint, int targetIndex, out string output, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPoint_GetNative(nint entryPoint, out string error);
    #endregion

    #region Program API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Program_Create(nint parentModule, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void Program_Release(nint program, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial SlangResult Program_CompileProgram(
        nint program,
        uint entryPointIndex,
        uint targetIndex,
        out string output, 
        out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Program_GetProgramReflection(
        nint program, 
        uint targetIndex, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Program_GetNative(nint program, out string error);   
    #endregion

    #region ShaderReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void ShaderReflection_Release(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_GetParent(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint ShaderReflection_GetParameterCount(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint ShaderReflection_GetTypeParameterCount(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_GetTypeParameterByIndex(
        nint shaderReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_FindTypeParameter(nint shaderReflection, string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_GetParameterByIndex(nint shaderReflection, uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint ShaderReflection_GetEntryPointCount(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_GetEntryPointByIndex(
        nint shaderReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_FindEntryPointByName(
        nint shaderReflection, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint ShaderReflection_GetGlobalConstantBufferBinding(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint ShaderReflection_GetGlobalConstantBufferSize(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_FindTypeByName(
        nint shaderReflection, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_FindFunctionByName(
        nint shaderReflection, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_FindFunctionByNameInType(
        nint shaderReflection, 
        nint type, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_FindVarByNameInType(
        nint shaderReflection, 
        nint type, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_GetTypeLayout(
        nint shaderReflection, 
        nint type, 
        int layoutRules, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_SpecializeType(
        nint shaderReflection, 
        nint type, 
        int argCount, 
        void** args, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool ShaderReflection_IsSubType(
        nint shaderReflection, 
        nint subType, 
        nint superType, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint ShaderReflection_GetHashedStringCount(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string? ShaderReflection_GetHashedString(
        nint shaderReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_GetGlobalParamsTypeLayout(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_GetGlobalParamsVarLayout(nint shaderReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int ShaderReflection_ToJson(
        nint shaderReflection, 
        out string output, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint ShaderReflection_GetNative(nint shaderReflection, out string error);

    #endregion

    #region TypeReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void TypeReflection_Release(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int TypeReflection_GetKind(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint TypeReflection_GetFieldCount(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_GetFieldByIndex(
        nint typeReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool TypeReflection_IsArray(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_UnwrapArray(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint TypeReflection_GetElementCount(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_GetElementType(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint TypeReflection_GetRowCount(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint TypeReflection_GetColumnCount(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int TypeReflection_GetScalarType(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_GetResourceResultType(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int TypeReflection_GetResourceShape(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int TypeReflection_GetResourceAccess(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string TypeReflection_GetName(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint TypeReflection_GetUserAttributeCount(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_GetUserAttributeByIndex(
        nint typeReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_FindAttributeByName(
        nint typeReflection, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_ApplySpecializations(
        nint typeReflection, 
        nint genRef, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_GetGenericContainer(nint typeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeReflection_GetNative(nint typeReflection, out string error);

    #endregion

    #region TypeLayoutReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void TypeLayoutReflection_Release(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeLayoutReflection_GetType(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int TypeLayoutReflection_GetKind(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint TypeLayoutReflection_GetSize(
        nint typeLayoutReflection, 
        int category, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint TypeLayoutReflection_GetStride(
        nint typeLayoutReflection, 
        int category, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int TypeLayoutReflection_GetAlignment(
        nint typeLayoutReflection, 
        int category, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint TypeLayoutReflection_GetFieldCount(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeLayoutReflection_GetFieldByIndex(
        nint typeLayoutReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int TypeLayoutReflection_FindFieldIndexByName(
        nint typeLayoutReflection, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeLayoutReflection_GetExplicitCounter(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool TypeLayoutReflection_IsArray(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeLayoutReflection_UnwrapArray(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint TypeLayoutReflection_GetElementCount(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint TypeLayoutReflection_GetTotalArrayElementCount(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint TypeLayoutReflection_GetElementStride(
        nint typeLayoutReflection, 
        int category, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeLayoutReflection_GetElementTypeLayout(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeLayoutReflection_GetElementVarLayout(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeLayoutReflection_GetContainerVarLayout(nint typeLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeLayoutReflection_GetNative(nint typeLayoutReflection, out string error);

    #endregion

    #region VariableReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void VariableReflection_Release(nint variableReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string VariableReflection_GetName(nint variableReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableReflection_GetType(nint variableReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableReflection_FindModifier(
        nint variableReflection, 
        int modifierId, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint VariableReflection_GetUserAttributeCount(nint variableReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableReflection_GetUserAttributeByIndex(
        nint variableReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableReflection_FindAttributeByName(
        nint variableReflection, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableReflection_FindUserAttributeByName(
        nint variableReflection, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool VariableReflection_HasDefaultValue(nint variableReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int VariableReflection_GetDefaultValueInt(
        nint variableReflection, 
        long* value, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableReflection_GetGenericContainer(nint variableReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableReflection_ApplySpecializations(
        nint variableReflection, 
        void** specializations, 
        int count, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableReflection_GetNative(nint variableReflection, out string error);

    #endregion

    #region VariableLayoutReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void VariableLayoutReflection_Release(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableLayoutReflection_GetVariable(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string VariableLayoutReflection_GetName(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableLayoutReflection_FindModifier(
        nint variableLayoutReflection, 
        int modifierId, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableLayoutReflection_GetTypeLayout(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int VariableLayoutReflection_GetCategory(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint VariableLayoutReflection_GetCategoryCount(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int VariableLayoutReflection_GetCategoryByIndex(
        nint variableLayoutReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint VariableLayoutReflection_GetOffset(
        nint variableLayoutReflection, 
        int category, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableLayoutReflection_GetType(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint VariableLayoutReflection_GetBindingIndex(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint VariableLayoutReflection_GetBindingSpace(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableLayoutReflection_GetSpace(
        nint variableLayoutReflection, 
        int category, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int VariableLayoutReflection_GetImageFormat(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string VariableLayoutReflection_GetSemanticName(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nuint VariableLayoutReflection_GetSemanticIndex(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint VariableLayoutReflection_GetStage(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableLayoutReflection_GetPendingDataLayout(nint variableLayoutReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint VariableLayoutReflection_GetNative(nint variableLayoutReflection, out string error);

    #endregion

    #region FunctionReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void FunctionReflection_Release(nint functionReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string FunctionReflection_GetName(nint functionReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_GetReturnType(nint functionReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint FunctionReflection_GetParameterCount(nint functionReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_GetParameterByIndex(
        nint functionReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_FindModifier(
        nint functionReflection, 
        int modifierId, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint FunctionReflection_GetUserAttributeCount(nint functionReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_GetUserAttributeByIndex(
        nint functionReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_FindAttributeByName(
        nint functionReflection, 
        string name, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_GetGenericContainer(nint functionReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_ApplySpecializations(
        nint functionReflection, 
        nint genRef, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_SpecializeWithArgTypes(
        nint functionReflection, 
        uint typeCount, 
        void** types, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool FunctionReflection_IsOverloaded(nint functionReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint FunctionReflection_GetOverloadCount(nint functionReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_GetOverload(
        nint functionReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint FunctionReflection_GetNative(nint functionReflection, out string error);

    #endregion

    #region EntryPointReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void EntryPointReflection_Release(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPointReflection_GetParent(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPointReflection_AsFunction(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string EntryPointReflection_GetName(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string? EntryPointReflection_GetNameOverride(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint EntryPointReflection_GetParameterCount(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPointReflection_GetParameterByIndex(
        nint entryPointReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPointReflection_GetFunction(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int EntryPointReflection_GetStage(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void EntryPointReflection_GetComputeThreadGroupSize(
        nint entryPointReflection, 
        uint axisCount, 
        ulong* outSizeAlongAxis, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int EntryPointReflection_GetComputeWaveSize(
        nint entryPointReflection, 
        ulong* outWaveSize, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool EntryPointReflection_UsesAnySampleRateInput(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPointReflection_GetVarLayout(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPointReflection_GetTypeLayout(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPointReflection_GetResultVarLayout(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    internal static partial bool EntryPointReflection_HasDefaultConstantBuffer(nint entryPointReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint EntryPointReflection_GetNative(nint entryPointReflection, out string error);

    #endregion

    #region GenericReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void GenericReflection_Release(nint genRefReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string GenericReflection_GetName(nint genRefReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint GenericReflection_GetTypeParameterCount(nint genRefReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint GenericReflection_GetTypeParameter(
        nint genRefReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint GenericReflection_GetValueParameterCount(nint genRefReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint GenericReflection_GetValueParameter(
        nint genRefReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint GenericReflection_GetTypeParameterConstraintCount(
        nint genRefReflection, 
        nint typeParam, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint GenericReflection_GetTypeParameterConstraintType(
        nint genRefReflection, 
        nint typeParam, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int GenericReflection_GetInnerKind(nint genRefReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint GenericReflection_GetOuterGenericContainer(nint genRefReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint GenericReflection_GetConcreteType(
        nint genRefReflection, 
        nint typeParam, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int GenericReflection_GetConcreteIntVal(
        nint genRefReflection, 
        nint valueParam, 
        long* value, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint GenericReflection_ApplySpecializations(
        nint genRefReflection, 
        nint genRef, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint GenericReflection_GetNative(nint genRefReflection, out string error);

    #endregion

    #region TypeParameterReflection API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void TypeParameterReflection_Release(nint typeParameterReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string TypeParameterReflection_GetName(nint typeParameterReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint TypeParameterReflection_GetIndex(nint typeParameterReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint TypeParameterReflection_GetConstraintCount(nint typeParameterReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeParameterReflection_GetConstraintByIndex(
        nint typeParameterReflection, 
        int index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint TypeParameterReflection_GetNative(nint typeParameterReflection, out string error);

    #endregion

    #region Attribute API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void Attribute_Release(nint attributeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string? Attribute_GetName(nint attributeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial uint Attribute_GetArgumentCount(nint attributeReflection, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Attribute_GetArgumentType(
        nint attributeReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int Attribute_GetArgumentValueInt(
        nint attributeReflection, 
        uint index, 
        int* value, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int Attribute_GetArgumentValueFloat(
        nint attributeReflection, 
        uint index, 
        float* value, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string? Attribute_GetArgumentValueString(
        nint attributeReflection, 
        uint index, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Attribute_GetNative(nint attributeReflection, out string error);

    #endregion

    #region Modifier API

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial void Modifier_Release(nint modifier, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int Modifier_GetID(nint modifier, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial string Modifier_GetName(nint modifier, out string error);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint Modifier_GetNative(nint modifier, out string error);

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
        string[] searchPaths, int searchPathsLength,
        out string error)
    {
        var handle = SlangNativeInterop.Session_Create(options, optionsLength, macros, macrosLength, models, modelsLength, searchPaths, searchPathsLength, out error);
        return new SessionHandle(handle);
    }

    /// <summary>
    /// Finds a previously loaded module by its index
    /// </summary>
    /// <param name="module"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    internal static ModuleHandle Session_GetModuleByIndex(nint module, uint index, out string error)
    {
        var handle = SlangNativeInterop.Session_GetModuleByIndex(module, index, out error);
        return new ModuleHandle(handle);
    }

    #endregion

    #region Module API

    /// <summary>
    /// Creates a module with strongly-typed handle.
    /// </summary>
    internal static ModuleHandle Module_Create(
        nint parentSession,
        string moduleName,
        string modulePath,
        string shaderSource,
        out string error)
    {
        var handle = SlangNativeInterop.Module_Create(parentSession, moduleName, modulePath, shaderSource, out error);
        return new ModuleHandle(handle);
    }

    /// <summary>
    /// Creates a module with strongly-typed handle.
    /// </summary>
    internal static ModuleHandle Module_Import(
        nint parentSession,
        string moduleName,
        out string error)
    {
        var handle = SlangNativeInterop.Module_Import(parentSession, moduleName, out error);
        return new ModuleHandle(handle);
    }

    /// <summary>
    /// Gets entry point by index with strongly-typed handle.
    /// </summary>
    internal static EntryPointHandle Module_GetEntryPointByIndex(
        nint parentModule,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.Module_GetEntryPointByIndex(parentModule, index, out error);
        return new EntryPointHandle(handle);
    }

    /// <summary>
    /// Finds entry point by name with strongly-typed handle.
    /// </summary>
    internal static EntryPointHandle Module_FindEntryPointByName(
        nint parentModule,
        string entryPointName,
        out string error)
    {
        var handle = SlangNativeInterop.Module_FindEntryPointByName(parentModule, entryPointName, out error);
        return new EntryPointHandle(handle);
    }

    #endregion

    #region EntryPoint API

    /// <summary>
    /// Creates an entry point with strongly-typed handle.
    /// </summary>
    internal static EntryPointHandle EntryPoint_Create(
        nint parentModule,
        uint entryPointIndex,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPoint_Create(parentModule, entryPointIndex, out error);
        return new EntryPointHandle(handle);
    }

    /// <summary>
    /// Creates an entry point by name with strongly-typed handle.
    /// </summary>
    internal static EntryPointHandle EntryPoint_CreateByName(
        nint parentModule,
        string entryPointName,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPoint_CreateByName(parentModule, entryPointName, out error);
        return new EntryPointHandle(handle);
    }

    #endregion

    #region Program API

    /// <summary>
    /// Creates a program with strongly-typed handle.
    /// </summary>
    internal static ProgramHandle Program_Create(nint parentModule, out string error)
    {
        var handle = SlangNativeInterop.Program_Create(parentModule, out error);
        return new ProgramHandle(handle);
    }

    /// <summary>
    /// Gets program reflection with strongly-typed handle.
    /// </summary>
    internal static ShaderReflectionHandle Program_GetProgramReflection(
        nint program,
        uint targetIndex, 
        out string error)
    {
        var handle = SlangNativeInterop.Program_GetProgramReflection(program, targetIndex, out error);
        return new ShaderReflectionHandle(handle);
    }

    #endregion

    #region ShaderReflection API

    /// <summary>
    /// Gets shader reflection parent with strongly-typed handle.
    /// </summary>
    internal static ShaderReflectionHandle ShaderReflection_GetParent(
        nint shaderReflection,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetParent(shaderReflection, out error);
        return new ShaderReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type parameter by index with strongly-typed handle.
    /// </summary>
    internal static TypeParameterReflectionHandle ShaderReflection_GetTypeParameterByIndex(
        nint shaderReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetTypeParameterByIndex(shaderReflection, index, out error);
        return new TypeParameterReflectionHandle(handle);
    }

    /// <summary>
    /// Finds type parameter with strongly-typed handle.
    /// </summary>
    internal static TypeParameterReflectionHandle ShaderReflection_FindTypeParameter(
        nint shaderReflection, 
        string name, 
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindTypeParameter(shaderReflection, name, out error);
        return new TypeParameterReflectionHandle(handle);
    }

    /// <summary>
    /// Gets parameter by index with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle ShaderReflection_GetParameterByIndex(
        nint shaderReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetParameterByIndex(shaderReflection, index, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point by index with strongly-typed handle.
    /// </summary>
    internal static EntryPointReflectionHandle ShaderReflection_GetEntryPointByIndex(
        nint shaderReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetEntryPointByIndex(shaderReflection, index, out error);
        return new EntryPointReflectionHandle(handle);
    }

    /// <summary>
    /// Finds entry point by name with strongly-typed handle.
    /// </summary>
    internal static EntryPointReflectionHandle ShaderReflection_FindEntryPointByName(
        nint shaderReflection,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindEntryPointByName(shaderReflection, name, out error);
        return new EntryPointReflectionHandle(handle);
    }

    /// <summary>
    /// Finds type by name with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle ShaderReflection_FindTypeByName(
        nint shaderReflection,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindTypeByName(shaderReflection, name, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds function by name with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle ShaderReflection_FindFunctionByName(
        nint shaderReflection,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindFunctionByName(shaderReflection, name, out error);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Finds function by name in type with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle ShaderReflection_FindFunctionByNameInType(
        nint shaderReflection,
        nint type,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindFunctionByNameInType(shaderReflection, type, name, out error);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Finds variable by name in type with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle ShaderReflection_FindVarByNameInType(
        nint shaderReflection,
        nint type,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_FindVarByNameInType(shaderReflection, type, name, out error);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle ShaderReflection_GetTypeLayout(
        nint shaderReflection,
        nint type,
        int layoutRules,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetTypeLayout(shaderReflection, type, layoutRules, out error);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Specializes type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle ShaderReflection_SpecializeType(
        nint shaderReflection,
        nint type,
        int argCount,
        void** args,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_SpecializeType(shaderReflection, type, argCount, args, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets global params type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle ShaderReflection_GetGlobalParamsTypeLayout(
        nint shaderReflection,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetGlobalParamsTypeLayout(shaderReflection, out error);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets global params var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle ShaderReflection_GetGlobalParamsVarLayout(
        nint shaderReflection,
        out string error)
    {
        var handle = SlangNativeInterop.ShaderReflection_GetGlobalParamsVarLayout(shaderReflection, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    #endregion

    #region TypeReflection API

    /// <summary>
    /// Gets type reflection field by index with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle TypeReflection_GetFieldByIndex(
        nint typeReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.TypeReflection_GetFieldByIndex(typeReflection, index, out error);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Unwraps array type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeReflection_UnwrapArray(
        nint typeReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeReflection_UnwrapArray(typeReflection, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets element type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeReflection_GetElementType(
        nint typeReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeReflection_GetElementType(typeReflection, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets resource result type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeReflection_GetResourceResultType(
        nint typeReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeReflection_GetResourceResultType(typeReflection, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets user attribute by index with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle TypeReflection_GetUserAttributeByIndex(
        nint typeReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.TypeReflection_GetUserAttributeByIndex(typeReflection, index, out error);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds attribute by name with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle TypeReflection_FindAttributeByName(
        nint typeReflection,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.TypeReflection_FindAttributeByName(typeReflection, name, out error);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Applies specializations with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeReflection_ApplySpecializations(
        nint typeReflection,
        nint genRef,
        out string error)
    {
        var handle = SlangNativeInterop.TypeReflection_ApplySpecializations(typeReflection, genRef, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets generic container with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle TypeReflection_GetGenericContainer(
        nint typeReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeReflection_GetGenericContainer(typeReflection, out error);
        return new GenericReflectionHandle(handle);
    }

    #endregion

    #region TypeLayoutReflection API

    /// <summary>
    /// Gets type layout type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeLayoutReflection_GetType(
        nint typeLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetType(typeLayoutReflection, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type layout field by index with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle TypeLayoutReflection_GetFieldByIndex(
        nint typeLayoutReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetFieldByIndex(typeLayoutReflection, index, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets explicit counter with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle TypeLayoutReflection_GetExplicitCounter(
        nint typeLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetExplicitCounter(typeLayoutReflection, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Unwraps array type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle TypeLayoutReflection_UnwrapArray(
        nint typeLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_UnwrapArray(typeLayoutReflection, out error);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets element type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle TypeLayoutReflection_GetElementTypeLayout(
        nint typeLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetElementTypeLayout(typeLayoutReflection, out error);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets element var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle TypeLayoutReflection_GetElementVarLayout(
        nint typeLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetElementVarLayout(typeLayoutReflection, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets container var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle TypeLayoutReflection_GetContainerVarLayout(
        nint typeLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.TypeLayoutReflection_GetContainerVarLayout(typeLayoutReflection, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    #endregion

    #region VariableReflection API

    /// <summary>
    /// Gets variable type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle VariableReflection_GetType(
        nint variableReflection,
        out string error)
    {
        var handle = SlangNativeInterop.VariableReflection_GetType(variableReflection, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds modifier with strongly-typed handle.
    /// </summary>
    internal static ModifierReflectionHandle VariableReflection_FindModifier(
        nint variableReflection,
        int modifierId,
        out string error)
    {
        var handle = SlangNativeInterop.VariableReflection_FindModifier(variableReflection, modifierId, out error);
        return new ModifierReflectionHandle(handle);
    }

    /// <summary>
    /// Gets user attribute by index with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle VariableReflection_GetUserAttributeByIndex(
        nint variableReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.VariableReflection_GetUserAttributeByIndex(variableReflection, index, out error);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds attribute by name with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle VariableReflection_FindAttributeByName(
        nint variableReflection,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.VariableReflection_FindAttributeByName(variableReflection, name, out error);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds user attribute by name with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle VariableReflection_FindUserAttributeByName(
        nint variableReflection,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.VariableReflection_FindUserAttributeByName(variableReflection, name, out error);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets generic container with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle VariableReflection_GetGenericContainer(
        nint variableReflection,
        out string error)
    {
        var handle = SlangNativeInterop.VariableReflection_GetGenericContainer(variableReflection, out error);
        return new GenericReflectionHandle(handle);
    }

    /// <summary>
    /// Applies specializations with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle VariableReflection_ApplySpecializations(
        nint variableReflection,
        void** specializations,
        int count,
        out string error)
    {
        var handle = SlangNativeInterop.VariableReflection_ApplySpecializations(variableReflection, specializations, count, out error);
        return new VariableReflectionHandle(handle);
    }

    #endregion

    #region VariableLayoutReflection API

    /// <summary>
    /// Gets variable layout variable with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle VariableLayoutReflection_GetVariable(
        nint variableLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetVariable(variableLayoutReflection, out error);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Finds modifier with strongly-typed handle.
    /// </summary>
    internal static ModifierReflectionHandle VariableLayoutReflection_FindModifier(
        nint variableLayoutReflection,
        int modifierId,
        out string error)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_FindModifier(variableLayoutReflection, modifierId, out error);
        return new ModifierReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle VariableLayoutReflection_GetTypeLayout(
        nint variableLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetTypeLayout(variableLayoutReflection, out error);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle VariableLayoutReflection_GetType(
        nint variableLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetType(variableLayoutReflection, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets space with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle VariableLayoutReflection_GetSpace(
        nint variableLayoutReflection,
        int category,
        out string error)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetSpace(variableLayoutReflection, category, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets pending data layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle VariableLayoutReflection_GetPendingDataLayout(
        nint variableLayoutReflection,
        out string error)
    {
        var handle = SlangNativeInterop.VariableLayoutReflection_GetPendingDataLayout(variableLayoutReflection, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    #endregion

    #region FunctionReflection API

    /// <summary>
    /// Gets function return type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle FunctionReflection_GetReturnType(
        nint functionReflection,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetReturnType(functionReflection, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets function parameter by index with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle FunctionReflection_GetParameterByIndex(
        nint functionReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetParameterByIndex(functionReflection, index, out error);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Finds modifier with strongly-typed handle.
    /// </summary>
    internal static ModifierReflectionHandle FunctionReflection_FindModifier(
        nint functionReflection,
        int modifierId,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_FindModifier(functionReflection, modifierId, out error);
        return new ModifierReflectionHandle(handle);
    }

    /// <summary>
    /// Gets user attribute by index with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle FunctionReflection_GetUserAttributeByIndex(
        nint functionReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetUserAttributeByIndex(functionReflection, index, out error);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Finds attribute by name with strongly-typed handle.
    /// </summary>
    internal static AttributeReflectionHandle FunctionReflection_FindAttributeByName(
        nint functionReflection,
        string name,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_FindAttributeByName(functionReflection, name, out error);
        return new AttributeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets generic container with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle FunctionReflection_GetGenericContainer(
        nint functionReflection,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetGenericContainer(functionReflection, out error);
        return new GenericReflectionHandle(handle);
    }

    /// <summary>
    /// Applies specializations with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle FunctionReflection_ApplySpecializations(
        nint functionReflection,
        nint genRef,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_ApplySpecializations(functionReflection, genRef, out error);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Specializes with arg types with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle FunctionReflection_SpecializeWithArgTypes(
        nint functionReflection,
        uint typeCount,
        void** types,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_SpecializeWithArgTypes(functionReflection, typeCount, types, out error);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Gets function overload with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle FunctionReflection_GetOverload(
        nint functionReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.FunctionReflection_GetOverload(functionReflection, index, out error);
        return new FunctionReflectionHandle(handle);
    }

    #endregion

    #region EntryPointReflection API

    /// <summary>
    /// Gets entry point parent with strongly-typed handle.
    /// </summary>
    internal static ShaderReflectionHandle EntryPointReflection_GetParent(
        nint entryPointReflection,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetParent(entryPointReflection, out error);
        return new ShaderReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point as function with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle EntryPointReflection_AsFunction(
        nint entryPointReflection,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPointReflection_AsFunction(entryPointReflection, out error);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point parameter by index with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle EntryPointReflection_GetParameterByIndex(
        nint entryPointReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetParameterByIndex(entryPointReflection, index, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point function with strongly-typed handle.
    /// </summary>
    internal static FunctionReflectionHandle EntryPointReflection_GetFunction(
        nint entryPointReflection,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetFunction(entryPointReflection, out error);
        return new FunctionReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle EntryPointReflection_GetVarLayout(
        nint entryPointReflection,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetVarLayout(entryPointReflection, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point type layout with strongly-typed handle.
    /// </summary>
    internal static TypeLayoutReflectionHandle EntryPointReflection_GetTypeLayout(
        nint entryPointReflection,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetTypeLayout(entryPointReflection, out error);
        return new TypeLayoutReflectionHandle(handle);
    }

    /// <summary>
    /// Gets entry point result var layout with strongly-typed handle.
    /// </summary>
    internal static VariableLayoutReflectionHandle EntryPointReflection_GetResultVarLayout(
        nint entryPointReflection,
        out string error)
    {
        var handle = SlangNativeInterop.EntryPointReflection_GetResultVarLayout(entryPointReflection, out error);
        return new VariableLayoutReflectionHandle(handle);
    }

    #endregion

    #region GenericReflection API

    /// <summary>
    /// Gets generic type parameter with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle GenericReflection_GetTypeParameter(
        nint genRefReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.GenericReflection_GetTypeParameter(genRefReflection, index, out error);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Gets generic value parameter with strongly-typed handle.
    /// </summary>
    internal static VariableReflectionHandle GenericReflection_GetValueParameter(
        nint genRefReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.GenericReflection_GetValueParameter(genRefReflection, index, out error);
        return new VariableReflectionHandle(handle);
    }

    /// <summary>
    /// Gets type parameter constraint type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle GenericReflection_GetTypeParameterConstraintType(
        nint genRefReflection,
        nint typeParam,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.GenericReflection_GetTypeParameterConstraintType(genRefReflection, typeParam, index, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Gets outer generic container with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle GenericReflection_GetOuterGenericContainer(
        nint genRefReflection,
        out string error)
    {
        var handle = SlangNativeInterop.GenericReflection_GetOuterGenericContainer(genRefReflection, out error);
        return new GenericReflectionHandle(handle);
    }

    /// <summary>
    /// Gets concrete type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle GenericReflection_GetConcreteType(
        nint genRefReflection,
        nint typeParam,
        out string error)
    {
        var handle = SlangNativeInterop.GenericReflection_GetConcreteType(genRefReflection, typeParam, out error);
        return new TypeReflectionHandle(handle);
    }

    /// <summary>
    /// Applies specializations with strongly-typed handle.
    /// </summary>
    internal static GenericReflectionHandle GenericReflection_ApplySpecializations(
        nint genRefReflection,
        nint genRef,
        out string error)
    {
        var handle = SlangNativeInterop.GenericReflection_ApplySpecializations(genRefReflection, genRef, out error);
        return new GenericReflectionHandle(handle);
    }

    #endregion

    #region TypeParameterReflection API

    /// <summary>
    /// Gets constraint by index with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle TypeParameterReflection_GetConstraintByIndex(
        nint typeParameterReflection,
        int index,
        out string error)
    {
        var handle = SlangNativeInterop.TypeParameterReflection_GetConstraintByIndex(typeParameterReflection, index, out error);
        return new TypeReflectionHandle(handle);
    }

    #endregion

    #region Attribute API

    /// <summary>
    /// Gets argument type with strongly-typed handle.
    /// </summary>
    internal static TypeReflectionHandle Attribute_GetArgumentType(
        nint attributeReflection,
        uint index,
        out string error)
    {
        var handle = SlangNativeInterop.Attribute_GetArgumentType(attributeReflection, index, out error);
        return new TypeReflectionHandle(handle);
    }

    #endregion
}