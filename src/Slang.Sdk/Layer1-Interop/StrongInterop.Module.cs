using Slang.Sdk.Interop;
using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class Module
        {
            /// <summary>
            /// Creates a module with strongly-typed handle.
            /// </summary>
            internal static ModuleHandle Create(
                SessionHandle parentSession,
                CompileRequestHandle compileRequest,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.Module_CreateFromCompileRequest(parentSession, compileRequest, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                SlangNativeInterop.FreeChar(&pError);

                return new ModuleHandle(handle);
            }

            /// <summary>
            /// Creates a module with strongly-typed handle.
            /// </summary>
            internal static ModuleHandle Create(
                SessionHandle parentSession,
                string moduleName,
                string modulePath,
                string shaderSource,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(moduleName);
                var unmanagedPath = Utf8StringMarshaller.ConvertToUnmanaged(modulePath);
                var unmanagedSource = Utf8StringMarshaller.ConvertToUnmanaged(shaderSource);
                var handle = SlangNativeInterop.Module_Create(parentSession, (char*)unmanagedName, (char*)unmanagedPath, (char*)unmanagedSource, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                // Free temporary resources
                Utf8StringMarshaller.Free(unmanagedName);
                Utf8StringMarshaller.Free(unmanagedPath);
                Utf8StringMarshaller.Free(unmanagedSource);
                SlangNativeInterop.FreeChar(&pError);

                return new ModuleHandle(handle);
            }

            /// <summary>
            /// Imports a module with strongly-typed handle.
            /// </summary>
            internal static ModuleHandle Import(
                SessionHandle parentSession,
                string moduleName,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(moduleName);
                var handle = SlangNativeInterop.Module_Import(parentSession, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new ModuleHandle(handle);
            }

            /// <summary>
            /// Releases a module with strongly-typed handle.
            /// </summary>
            internal static void Release(ModuleHandle module, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.Module_Release(module, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets module name.
            /// </summary>
            internal static string? GetName(ModuleHandle module, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.Module_GetName(module, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string? result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets module entry point count.
            /// </summary>
            internal static uint GetEntryPointCount(ModuleHandle module, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.Module_GetEntryPointCount(module, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets entry point by index with strongly-typed handle.
            /// </summary>
            internal static EntryPointHandle GetEntryPointByIndex(
                ModuleHandle parentModule,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.Module_GetEntryPointByIndex(parentModule, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new EntryPointHandle(handle);
            }

            /// <summary>
            /// Finds entry point by name with strongly-typed handle.
            /// </summary>
            internal static EntryPointHandle FindEntryPointByName(
                ModuleHandle parentModule,
                string entryPointName,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(entryPointName);
                var handle = SlangNativeInterop.Module_FindEntryPointByName(parentModule, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new EntryPointHandle(handle);
            }

            /// <summary>
            /// Gets native module handle.
            /// </summary>
            internal static nint GetNative(ModuleHandle module, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.Module_GetNative(module, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}
