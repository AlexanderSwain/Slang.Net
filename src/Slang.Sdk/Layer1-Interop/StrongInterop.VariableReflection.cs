using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class VariableReflection
        {
            /// <summary>
            /// Releases a variable reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(VariableReflectionHandle variableReflection, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.VariableReflection_Release(variableReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the variable name.
            /// </summary>
            internal static string GetName(VariableReflectionHandle variableReflection, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.VariableReflection_GetName(variableReflection, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the variable type.
            /// </summary>
            internal static TypeReflectionHandle GetType(VariableReflectionHandle variableReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.VariableReflection_GetType(variableReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Finds a modifier.
            /// </summary>
            internal static ModifierReflectionHandle FindModifier(
                VariableReflectionHandle variableReflection,
                int modifierId,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.VariableReflection_FindModifier(variableReflection, modifierId, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new ModifierReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the user attribute count.
            /// </summary>
            internal static uint GetUserAttributeCount(VariableReflectionHandle variableReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.VariableReflection_GetUserAttributeCount(variableReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a user attribute by index.
            /// </summary>
            internal static AttributeReflectionHandle GetUserAttributeByIndex(
                VariableReflectionHandle variableReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.VariableReflection_GetUserAttributeByIndex(variableReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new AttributeReflectionHandle(handle);
            }

            /// <summary>
            /// Finds an attribute by name.
            /// </summary>
            internal static AttributeReflectionHandle FindAttributeByName(
                VariableReflectionHandle variableReflection,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.VariableReflection_FindAttributeByName(variableReflection, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new AttributeReflectionHandle(handle);
            }

            /// <summary>
            /// Finds a user attribute by name.
            /// </summary>
            internal static AttributeReflectionHandle FindUserAttributeByName(
                VariableReflectionHandle variableReflection,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.VariableReflection_FindUserAttributeByName(variableReflection, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new AttributeReflectionHandle(handle);
            }

            /// <summary>
            /// Checks if the variable has a default value.
            /// </summary>
            internal static bool HasDefaultValue(VariableReflectionHandle variableReflection, out string? error)
            {
                char* pError = null;
                bool result = SlangNativeInterop.VariableReflection_HasDefaultValue(variableReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the default value as an integer.
            /// </summary>
            internal static SlangResult GetDefaultValueInt(VariableReflectionHandle variableReflection, out long output, out string? error)
            {
                long value = 0;
                char* pError = null;
                var result = SlangNativeInterop.VariableReflection_GetDefaultValueInt(variableReflection, &value, &pError);

                output = value;
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                SlangNativeInterop.FreeChar(&pError);
                return (SlangResult)result;
            }

            /// <summary>
            /// Gets the generic container.
            /// </summary>
            internal static GenericReflectionHandle GetGenericContainer(VariableReflectionHandle variableReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.VariableReflection_GetGenericContainer(variableReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new GenericReflectionHandle(handle);
            }

            /// <summary>
            /// Applies specializations.
            /// </summary>
            internal static VariableReflectionHandle ApplySpecializations(
                VariableReflectionHandle variableReflection,
                void** specializations,
                int count,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.VariableReflection_ApplySpecializations(variableReflection, specializations, count, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the native variable reflection handle.
            /// </summary>
            internal static nint GetNative(VariableReflectionHandle variableReflection, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.VariableReflection_GetNative(variableReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}