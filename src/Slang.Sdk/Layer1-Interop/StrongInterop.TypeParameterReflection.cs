using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class TypeParameterReflection
        {
            /// <summary>
            /// Releases a type parameter reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(TypeParameterReflectionHandle typeParameterReflection, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.TypeParameterReflection_Release(typeParameterReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the type parameter name.
            /// </summary>
            internal static string GetName(TypeParameterReflectionHandle typeParameterReflection, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.TypeParameterReflection_GetName(typeParameterReflection, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the type parameter index.
            /// </summary>
            internal static uint GetIndex(TypeParameterReflectionHandle typeParameterReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.TypeParameterReflection_GetIndex(typeParameterReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the constraint count.
            /// </summary>
            internal static uint GetConstraintCount(TypeParameterReflectionHandle typeParameterReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.TypeParameterReflection_GetConstraintCount(typeParameterReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a constraint by index.
            /// </summary>
            internal static TypeReflectionHandle GetConstraintByIndex(
                TypeParameterReflectionHandle typeParameterReflection,
                int index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.TypeParameterReflection_GetConstraintByIndex(typeParameterReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the native type parameter reflection handle.
            /// </summary>
            internal static nint GetNative(TypeParameterReflectionHandle typeParameterReflection, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.TypeParameterReflection_GetNative(typeParameterReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}