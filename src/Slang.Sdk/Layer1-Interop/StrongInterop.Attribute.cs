using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class Attribute
        {
            /// <summary>
            /// Releases an attribute reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(AttributeReflectionHandle attributeReflection, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.Attribute_Release(attributeReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the attribute name.
            /// </summary>
            internal static string? GetName(AttributeReflectionHandle attributeReflection, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.Attribute_GetName(attributeReflection, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string? result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the argument count.
            /// </summary>
            internal static uint GetArgumentCount(AttributeReflectionHandle attributeReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.Attribute_GetArgumentCount(attributeReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the argument type by index.
            /// </summary>
            internal static TypeReflectionHandle GetArgumentType(
                AttributeReflectionHandle attributeReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.Attribute_GetArgumentType(attributeReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the argument value as integer.
            /// </summary>
            internal static SlangResult GetArgumentValueInt(
                AttributeReflectionHandle attributeReflection,
                uint index,
                out int output,
                out string? error)
            {
                int value;
                char* pError = null;
                var result = SlangNativeInterop.Attribute_GetArgumentValueInt(attributeReflection, index, &value, &pError);

                output = value;
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                SlangNativeInterop.FreeChar(&pError);
                return (SlangResult)result;
            }

            /// <summary>
            /// Gets the argument value as float.
            /// </summary>
            internal static SlangResult GetArgumentValueFloat(
                AttributeReflectionHandle attributeReflection,
                uint index,
                out float output,
                out string? error)
            {
                char* pError = null;
                float value;
                var result = SlangNativeInterop.Attribute_GetArgumentValueFloat(attributeReflection, index, &value, &pError);

                output = value;
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                SlangNativeInterop.FreeChar(&pError);
                return (SlangResult)result;
            }

            /// <summary>
            /// Gets the argument value as string.
            /// </summary>
            internal static string? GetArgumentValueString(
                AttributeReflectionHandle attributeReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                char* pValue = SlangNativeInterop.Attribute_GetArgumentValueString(attributeReflection, index, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string? result = Utf8StringMarshaller.ConvertToManaged((byte*)pValue);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the native attribute handle.
            /// </summary>
            internal static nint GetNative(AttributeReflectionHandle attributeReflection, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.Attribute_GetNative(attributeReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}