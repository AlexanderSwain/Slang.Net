using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class ShaderReflection
        {
            /// <summary>
            /// Releases a shader reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.ShaderReflection_Release(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the parent shader reflection.
            /// </summary>
            internal static ShaderReflectionHandle GetParent(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.ShaderReflection_GetParent(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new ShaderReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the parameter count.
            /// </summary>
            internal static uint GetParameterCount(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.ShaderReflection_GetParameterCount(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the type parameter count.
            /// </summary>
            internal static uint GetTypeParameterCount(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.ShaderReflection_GetTypeParameterCount(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a type parameter by index.
            /// </summary>
            internal static TypeParameterReflectionHandle GetTypeParameterByIndex(
                ShaderReflectionHandle shaderReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.ShaderReflection_GetTypeParameterByIndex(shaderReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeParameterReflectionHandle(handle);
            }

            /// <summary>
            /// Finds a type parameter by name.
            /// </summary>
            internal static TypeParameterReflectionHandle FindTypeParameter(
                ShaderReflectionHandle shaderReflection,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.ShaderReflection_FindTypeParameter(shaderReflection, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new TypeParameterReflectionHandle(handle);
            }

            /// <summary>
            /// Gets a parameter by index.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetParameterByIndex(
                ShaderReflectionHandle shaderReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.ShaderReflection_GetParameterByIndex(shaderReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the entry point count.
            /// </summary>
            internal static uint GetEntryPointCount(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.ShaderReflection_GetEntryPointCount(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets an entry point by index.
            /// </summary>
            internal static EntryPointReflectionHandle GetEntryPointByIndex(
                ShaderReflectionHandle shaderReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.ShaderReflection_GetEntryPointByIndex(shaderReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new EntryPointReflectionHandle(handle);
            }

            /// <summary>
            /// Finds an entry point by name.
            /// </summary>
            internal static EntryPointReflectionHandle FindEntryPointByName(
                ShaderReflectionHandle shaderReflection,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.ShaderReflection_FindEntryPointByName(shaderReflection, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new EntryPointReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the global constant buffer binding.
            /// </summary>
            internal static uint GetGlobalConstantBufferBinding(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.ShaderReflection_GetGlobalConstantBufferBinding(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the global constant buffer size.
            /// </summary>
            internal static nuint GetGlobalConstantBufferSize(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                nuint result = SlangNativeInterop.ShaderReflection_GetGlobalConstantBufferSize(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Finds a type by name.
            /// </summary>
            internal static TypeReflectionHandle FindTypeByName(
                ShaderReflectionHandle shaderReflection,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.ShaderReflection_FindTypeByName(shaderReflection, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Finds a function by name.
            /// </summary>
            internal static FunctionReflectionHandle FindFunctionByName(
                ShaderReflectionHandle shaderReflection,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.ShaderReflection_FindFunctionByName(shaderReflection, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new FunctionReflectionHandle(handle);
            }

            /// <summary>
            /// Finds a function by name in a specific type.
            /// </summary>
            internal static FunctionReflectionHandle FindFunctionByNameInType(
                ShaderReflectionHandle shaderReflection,
                TypeReflectionHandle type,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.ShaderReflection_FindFunctionByNameInType(shaderReflection, type, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new FunctionReflectionHandle(handle);
            }

            /// <summary>
            /// Finds a variable by name in a specific type.
            /// </summary>
            internal static VariableReflectionHandle FindVarByNameInType(
                ShaderReflectionHandle shaderReflection,
                TypeReflectionHandle type,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.ShaderReflection_FindVarByNameInType(shaderReflection, type, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new VariableReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the type layout for a specific type.
            /// </summary>
            internal static TypeLayoutReflectionHandle GetTypeLayout(
                ShaderReflectionHandle shaderReflection,
                TypeReflectionHandle type,
                int layoutRules,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.ShaderReflection_GetTypeLayout(shaderReflection, type, layoutRules, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Specializes a type.
            /// </summary>
            internal static TypeReflectionHandle SpecializeType(
                ShaderReflectionHandle shaderReflection,
                TypeReflectionHandle type,
                int argCount,
                void** args,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.ShaderReflection_SpecializeType(shaderReflection, type, argCount, args, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Checks if a type is a subtype of another.
            /// </summary>
            internal static bool IsSubType(
                ShaderReflectionHandle shaderReflection,
                TypeReflectionHandle subType,
                TypeReflectionHandle superType,
                out string? error)
            {
                char* pError = null;
                bool result = SlangNativeInterop.ShaderReflection_IsSubType(shaderReflection, subType, superType, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the hashed string count.
            /// </summary>
            internal static uint GetHashedStringCount(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.ShaderReflection_GetHashedStringCount(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a hashed string by index.
            /// </summary>
            internal static string? GetHashedString(
                ShaderReflectionHandle shaderReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                char* result = SlangNativeInterop.ShaderReflection_GetHashedString(shaderReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string? managedResult = Utf8StringMarshaller.ConvertToManaged((byte*)result);
                SlangNativeInterop.FreeChar(&pError);
                return managedResult;
            }

            /// <summary>
            /// Gets the global parameters type layout.
            /// </summary>
            internal static TypeLayoutReflectionHandle GetGlobalParamsTypeLayout(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.ShaderReflection_GetGlobalParamsTypeLayout(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the global parameters variable layout.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetGlobalParamsVarLayout(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.ShaderReflection_GetGlobalParamsVarLayout(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Converts the shader reflection to JSON.
            /// </summary>
            internal static SlangResult ToJson(ShaderReflectionHandle shaderReflection, out string output, out string? error)
            {
                char* pError = null;
                char* pOutput = null;
                var result = SlangNativeInterop.ShaderReflection_ToJson(shaderReflection, &pOutput, &pError);

                output = Utf8StringMarshaller.ConvertToManaged((byte*)pOutput);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                SlangNativeInterop.FreeChar(&pError);
                SlangNativeInterop.FreeChar(&pOutput);

                return (SlangResult)result;
            }

            /// <summary>
            /// Gets the native shader reflection handle.
            /// </summary>
            internal static nint GetNative(ShaderReflectionHandle shaderReflection, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.ShaderReflection_GetNative(shaderReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}