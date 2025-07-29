using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class TypeLayoutReflection
        {
            /// <summary>
            /// Releases a type layout reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.TypeLayoutReflection_Release(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the type.
            /// </summary>
            internal static TypeReflectionHandle GetType(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.TypeLayoutReflection_GetType(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the type kind.
            /// </summary>
            internal static int GetKind(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                int result = SlangNativeInterop.TypeLayoutReflection_GetKind(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the size for a specific category.
            /// </summary>
            internal static nuint GetSize(
                TypeLayoutReflectionHandle typeLayoutReflection,
                int category,
                out string? error)
            {
                char* pError = null;
                nuint result = SlangNativeInterop.TypeLayoutReflection_GetSize(typeLayoutReflection, category, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the stride for a specific category.
            /// </summary>
            internal static nuint GetStride(
                TypeLayoutReflectionHandle typeLayoutReflection,
                int category,
                out string? error)
            {
                char* pError = null;
                nuint result = SlangNativeInterop.TypeLayoutReflection_GetStride(typeLayoutReflection, category, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the alignment for a specific category.
            /// </summary>
            internal static int GetAlignment(
                TypeLayoutReflectionHandle typeLayoutReflection,
                int category,
                out string? error)
            {
                char* pError = null;
                int result = SlangNativeInterop.TypeLayoutReflection_GetAlignment(typeLayoutReflection, category, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the field count.
            /// </summary>
            internal static uint GetFieldCount(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.TypeLayoutReflection_GetFieldCount(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a field by index.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetFieldByIndex(
                TypeLayoutReflectionHandle typeLayoutReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.TypeLayoutReflection_GetFieldByIndex(typeLayoutReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Finds a field index by name.
            /// </summary>
            internal static int FindFieldIndexByName(
                TypeLayoutReflectionHandle typeLayoutReflection,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                int result = SlangNativeInterop.TypeLayoutReflection_FindFieldIndexByName(typeLayoutReflection, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the explicit counter.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetExplicitCounter(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.TypeLayoutReflection_GetExplicitCounter(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Checks if the type layout is an array.
            /// </summary>
            internal static bool IsArray(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                bool result = SlangNativeInterop.TypeLayoutReflection_IsArray(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Unwraps an array type layout.
            /// </summary>
            internal static TypeLayoutReflectionHandle UnwrapArray(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.TypeLayoutReflection_UnwrapArray(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the element count.
            /// </summary>
            internal static nuint GetElementCount(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                nuint result = SlangNativeInterop.TypeLayoutReflection_GetElementCount(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the total array element count.
            /// </summary>
            internal static nuint GetTotalArrayElementCount(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                nuint result = SlangNativeInterop.TypeLayoutReflection_GetTotalArrayElementCount(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the element stride for a specific category.
            /// </summary>
            internal static nuint GetElementStride(
                TypeLayoutReflectionHandle typeLayoutReflection,
                int category,
                out string? error)
            {
                char* pError = null;
                nuint result = SlangNativeInterop.TypeLayoutReflection_GetElementStride(typeLayoutReflection, category, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the element type layout.
            /// </summary>
            internal static TypeLayoutReflectionHandle GetElementTypeLayout(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.TypeLayoutReflection_GetElementTypeLayout(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the element variable layout.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetElementVarLayout(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.TypeLayoutReflection_GetElementVarLayout(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the container variable layout.
            /// </summary>
            internal static VariableLayoutReflectionHandle GetContainerVarLayout(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.TypeLayoutReflection_GetContainerVarLayout(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableLayoutReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the native type layout reflection handle.
            /// </summary>
            internal static nint GetNative(TypeLayoutReflectionHandle typeLayoutReflection, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.TypeLayoutReflection_GetNative(typeLayoutReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}