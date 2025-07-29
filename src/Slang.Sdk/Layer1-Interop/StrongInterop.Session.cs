using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop;

/// <summary>
/// Strongly-typed wrapper methods for Session and Module management APIs.
/// </summary>
internal unsafe partial class StrongInterop
{
    internal class Session
    {
        /// <summary>
        /// Creates a Slang session with strongly-typed handle.
        /// </summary>
        internal static SessionHandle Create(
            void* options, int optionsLength,
            void* macros, int macrosLength,
            void* models, int modelsLength,
            string[] searchPaths, int searchPathsLength,
            out string? error)
        {
            char* pError = null;
            char*[] managedSearchPaths = new char*[searchPathsLength];

            try
            {
                // Convert string array to char** for native interop
                if (searchPaths != null && searchPaths.Length > 0)
                {
                    for (int i = 0; i < searchPaths.Length; i++)
                    {
                        managedSearchPaths[i] = (char*)Utf8StringMarshaller.ConvertToUnmanaged(searchPaths[i]);
                    }
                }

                var handle = SlangNativeInterop.Session_Create(options, optionsLength, macros, macrosLength, models, modelsLength, managedSearchPaths, searchPathsLength, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                return new SessionHandle(handle);
            }
            finally
            {
                // Free temporary resources
                if (managedSearchPaths != null)
                {
                    for (int i = 0; i < searchPathsLength && i < searchPaths.Length; i++)
                    {
                        if (managedSearchPaths[i] != null)
                            Utf8StringMarshaller.Free((byte*)managedSearchPaths[i]);
                    }
                    //Marshal.FreeHGlobal((nint)managedSearchPaths);
                }
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Releases a session with strongly-typed handle.
        /// </summary>
        internal static void Release(SessionHandle session, out string? error)
        {
            char* pError = null;
            SlangNativeInterop.Session_Release(session, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
        }

        /// <summary>
        /// Gets the number of modules in the session.
        /// </summary>
        internal static uint GetModuleCount(SessionHandle session, out string? error)
        {
            char* pError = null;
            uint result = SlangNativeInterop.Session_GetModuleCount(session, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
            return result;
        }

        /// <summary>
        /// Gets a module by index with strongly-typed handle.
        /// </summary>
        internal static ModuleHandle GetModuleByIndex(SessionHandle session, uint index, out string? error)
        {
            char* pError = null;
            var handle = SlangNativeInterop.Session_GetModuleByIndex(session, index, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
            return new ModuleHandle(handle);
        }

        /// <summary>
        /// Gets the native session handle.
        /// </summary>
        internal static nint GetNative(SessionHandle session, out string? error)
        {
            char* pError = null;
            nint result = SlangNativeInterop.Session_GetNative(session, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
            return result;
        }
    }
}