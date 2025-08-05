using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class EntryPointReflection
        {
            /// <summary>
            /// Releases an entry point reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                //char* pError = null;
                //SlangNativeInterop.EntryPointReflection_Release(entryPointReflection, &pError);
                error = null;// Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                //SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the parent shader reflection.
            /// </summary>
            internal static ShaderReflectionHandle GetParent(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.EntryPointReflection_GetParent(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new ShaderReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the entry point as a function.
            /// </summary>
            internal static FunctionReflectionHandle AsFunction(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.EntryPointReflection_AsFunction(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new FunctionReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the entry point name.
            /// </summary>
            internal static string GetName(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.EntryPointReflection_GetName(entryPointReflection, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the entry point name override.
            /// </summary>
            internal static string? GetNameOverride(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.EntryPointReflection_GetNameOverride(entryPointReflection, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string? result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the parameter count.
            /// </summary>
            internal static uint GetParameterCount(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.EntryPointReflection_GetParameterCount(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a parameter by index.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetParameterByIndex(
                EntryPointReflectionHandle entryPointReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.EntryPointReflection_GetParameterByIndex(entryPointReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the function.
            /// </summary>
            internal static FunctionReflectionHandle GetFunction(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.EntryPointReflection_GetFunction(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new FunctionReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the stage.
            /// </summary>
            internal static int GetStage(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                int result = SlangNativeInterop.EntryPointReflection_GetStage(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the compute thread group size.
            /// </summary>
            internal static void GetComputeThreadGroupSize(
                EntryPointReflectionHandle entryPointReflection,
                uint axisCount,
                ulong* outSizeAlongAxis,
                out string? error)
            {
                char* pError = null;
                SlangNativeInterop.EntryPointReflection_GetComputeThreadGroupSize(entryPointReflection, axisCount, outSizeAlongAxis, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the compute wave size.
            /// </summary>
            internal static SlangResult GetComputeWaveSize(EntryPointReflectionHandle entryPointReflection, out ulong output, out string? error)
            {
                char* pError = null;
                ulong waveSize;
                var result = SlangNativeInterop.EntryPointReflection_GetComputeWaveSize(entryPointReflection, &waveSize, &pError);

                output = waveSize;
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                SlangNativeInterop.FreeChar(&pError);
                return (SlangResult)result;
            }

            /// <summary>
            /// Checks if uses any sample rate input.
            /// </summary>
            internal static bool UsesAnySampleRateInput(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                bool result = SlangNativeInterop.EntryPointReflection_UsesAnySampleRateInput(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the variable layout.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetVarLayout(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.EntryPointReflection_GetVarLayout(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the type layout.
            /// </summary>
            internal static TypeLayoutReflectionHandle GetTypeLayout(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.EntryPointReflection_GetTypeLayout(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the result variable layout.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetResultVarLayout(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.EntryPointReflection_GetResultVarLayout(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Checks if has default constant buffer.
            /// </summary>
            internal static bool HasDefaultConstantBuffer(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                bool result = SlangNativeInterop.EntryPointReflection_HasDefaultConstantBuffer(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the native entry point reflection handle.
            /// </summary>
            internal static nint GetNative(EntryPointReflectionHandle entryPointReflection, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.EntryPointReflection_GetNative(entryPointReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}