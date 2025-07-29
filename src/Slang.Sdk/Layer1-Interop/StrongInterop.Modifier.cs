using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class Modifier
        {
            /// <summary>
            /// Releases a modifier reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(ModifierReflectionHandle modifier, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.Modifier_Release(modifier, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the modifier ID.
            /// </summary>
            internal static int GetID(ModifierReflectionHandle modifier, out string? error)
            {
                char* pError = null;
                int result = SlangNativeInterop.Modifier_GetID(modifier, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the modifier name.
            /// </summary>
            internal static string GetName(ModifierReflectionHandle modifier, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.Modifier_GetName(modifier, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the native modifier handle.
            /// </summary>
            internal static nint GetNative(ModifierReflectionHandle modifier, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.Modifier_GetNative(modifier, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}