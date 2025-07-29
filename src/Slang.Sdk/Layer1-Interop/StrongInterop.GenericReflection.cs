using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class GenericReflection
        {
            /// <summary>
            /// Releases a generic reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(GenericReflectionHandle genRefReflection, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.GenericReflection_Release(genRefReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the generic name.
            /// </summary>
            internal static string GetName(GenericReflectionHandle genRefReflection, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.GenericReflection_GetName(genRefReflection, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the type parameter count.
            /// </summary>
            internal static uint GetTypeParameterCount(GenericReflectionHandle genRefReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.GenericReflection_GetTypeParameterCount(genRefReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a type parameter by index.
            /// </summary>
            internal static VariableReflectionHandle GetTypeParameter(
                GenericReflectionHandle genRefReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.GenericReflection_GetTypeParameter(genRefReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the value parameter count.
            /// </summary>
            internal static uint GetValueParameterCount(GenericReflectionHandle genRefReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.GenericReflection_GetValueParameterCount(genRefReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a value parameter by index.
            /// </summary>
            internal static VariableReflectionHandle GetValueParameter(
                GenericReflectionHandle genRefReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.GenericReflection_GetValueParameter(genRefReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the type parameter constraint count.
            /// </summary>
            internal static uint GetTypeParameterConstraintCount(
                GenericReflectionHandle genRefReflection,
                TypeParameterReflectionHandle typeParam,
                out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.GenericReflection_GetTypeParameterConstraintCount(genRefReflection, typeParam, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a type parameter constraint type by index.
            /// </summary>
            internal static TypeReflectionHandle GetTypeParameterConstraintType(
                GenericReflectionHandle genRefReflection,
                TypeParameterReflectionHandle typeParam,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.GenericReflection_GetTypeParameterConstraintType(genRefReflection, typeParam, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the inner kind.
            /// </summary>
            internal static int GetInnerKind(GenericReflectionHandle genRefReflection, out string? error)
            {
                char* pError = null;
                int result = SlangNativeInterop.GenericReflection_GetInnerKind(genRefReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the outer generic container.
            /// </summary>
            internal static GenericReflectionHandle GetOuterGenericContainer(GenericReflectionHandle genRefReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.GenericReflection_GetOuterGenericContainer(genRefReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new GenericReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the concrete type.
            /// </summary>
            internal static TypeReflectionHandle GetConcreteType(
                GenericReflectionHandle genRefReflection,
                TypeParameterReflectionHandle typeParam,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.GenericReflection_GetConcreteType(genRefReflection, typeParam, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the concrete integer value.
            /// </summary>
            internal static SlangResult GetConcreteIntVal(
                GenericReflectionHandle genRefReflection,
                VariableReflectionHandle valueParam,
                out long output,
                out string? error)
            {
                char* pError = null;
                long value;
                var result = SlangNativeInterop.GenericReflection_GetConcreteIntVal(genRefReflection, valueParam, &value, &pError);

                output = value;
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                
                SlangNativeInterop.FreeChar(&pError);
                return (SlangResult)result;
            }

            /// <summary>
            /// Applies specializations.
            /// </summary>
            internal static GenericReflectionHandle ApplySpecializations(
                GenericReflectionHandle genRefReflection,
                GenericReflectionHandle genRef,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.GenericReflection_ApplySpecializations(genRefReflection, genRef, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new GenericReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the native generic reflection handle.
            /// </summary>
            internal static nint GetNative(GenericReflectionHandle genRefReflection, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.GenericReflection_GetNative(genRefReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}