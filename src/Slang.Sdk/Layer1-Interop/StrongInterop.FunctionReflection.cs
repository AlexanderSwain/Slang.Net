using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop
{
    internal unsafe partial class StrongInterop
    {
        internal class FunctionReflection
        {
            /// <summary>
            /// Releases a function reflection with strongly-typed handle.
            /// </summary>
            internal static void Release(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                SlangNativeInterop.FunctionReflection_Release(functionReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
            }

            /// <summary>
            /// Gets the function name.
            /// </summary>
            internal static string GetName(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                char* pName = SlangNativeInterop.FunctionReflection_GetName(functionReflection, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                string result = Utf8StringMarshaller.ConvertToManaged((byte*)pName);

                SlangNativeInterop.FreeChar(&pError);

                return result;
            }

            /// <summary>
            /// Gets the return type.
            /// </summary>
            internal static TypeReflectionHandle GetReturnType(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.FunctionReflection_GetReturnType(functionReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new TypeReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the parameter count.
            /// </summary>
            internal static uint GetParameterCount(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.FunctionReflection_GetParameterCount(functionReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a parameter by index.
            /// </summary>
            internal static VariableReflectionHandle GetParameterByIndex(
                FunctionReflectionHandle functionReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.FunctionReflection_GetParameterByIndex(functionReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new VariableReflectionHandle(handle);
            }

            /// <summary>
            /// Finds a modifier.
            /// </summary>
            internal static ModifierReflectionHandle FindModifier(
                FunctionReflectionHandle functionReflection,
                int modifierId,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.FunctionReflection_FindModifier(functionReflection, modifierId, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new ModifierReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the user attribute count.
            /// </summary>
            internal static uint GetUserAttributeCount(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.FunctionReflection_GetUserAttributeCount(functionReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets a user attribute by index.
            /// </summary>
            internal static AttributeReflectionHandle GetUserAttributeByIndex(
                FunctionReflectionHandle functionReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.FunctionReflection_GetUserAttributeByIndex(functionReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new AttributeReflectionHandle(handle);
            }

            /// <summary>
            /// Finds an attribute by name.
            /// </summary>
            internal static AttributeReflectionHandle FindAttributeByName(
                FunctionReflectionHandle functionReflection,
                string name,
                out string? error)
            {
                char* pError = null;
                var unmanagedName = Utf8StringMarshaller.ConvertToUnmanaged(name);
                var handle = SlangNativeInterop.FunctionReflection_FindAttributeByName(functionReflection, (char*)unmanagedName, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                Utf8StringMarshaller.Free(unmanagedName);
                SlangNativeInterop.FreeChar(&pError);

                return new AttributeReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the generic container.
            /// </summary>
            internal static GenericReflectionHandle GetGenericContainer(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.FunctionReflection_GetGenericContainer(functionReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new GenericReflectionHandle(handle);
            }

            /// <summary>
            /// Applies specializations.
            /// </summary>
            internal static FunctionReflectionHandle ApplySpecializations(
                FunctionReflectionHandle functionReflection,
                GenericReflectionHandle genRef,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.FunctionReflection_ApplySpecializations(functionReflection, genRef, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new FunctionReflectionHandle(handle);
            }

            /// <summary>
            /// Specializes with argument types.
            /// </summary>
            internal static FunctionReflectionHandle SpecializeWithArgTypes(
                FunctionReflectionHandle functionReflection,
                uint typeCount,
                void** types,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.FunctionReflection_SpecializeWithArgTypes(functionReflection, typeCount, types, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new FunctionReflectionHandle(handle);
            }

            /// <summary>
            /// Checks if the function is overloaded.
            /// </summary>
            internal static bool IsOverloaded(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                bool result = SlangNativeInterop.FunctionReflection_IsOverloaded(functionReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets the overload count.
            /// </summary>
            internal static uint GetOverloadCount(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                uint result = SlangNativeInterop.FunctionReflection_GetOverloadCount(functionReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }

            /// <summary>
            /// Gets an overload by index.
            /// </summary>
            internal static FunctionReflectionHandle GetOverload(
                FunctionReflectionHandle functionReflection,
                uint index,
                out string? error)
            {
                char* pError = null;
                var handle = SlangNativeInterop.FunctionReflection_GetOverload(functionReflection, index, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return new FunctionReflectionHandle(handle);
            }

            /// <summary>
            /// Gets the native function reflection handle.
            /// </summary>
            internal static nint GetNative(FunctionReflectionHandle functionReflection, out string? error)
            {
                char* pError = null;
                nint result = SlangNativeInterop.FunctionReflection_GetNative(functionReflection, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                SlangNativeInterop.FreeChar(&pError);
                return result;
            }
        }
    }
}