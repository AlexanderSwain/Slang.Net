using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;
using static Slang.Sdk.Interop.SlangNativeInterop;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding
{
    internal unsafe class TypeReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override TypeReflectionHandle Handle { get; }

        internal TypeReflection(Reflection parent, TypeReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal TypeKind GetKind()
        {
            string? error = null;
            return (TypeKind)Call(() => SlangNativeInterop.TypeReflection_GetKind(Handle, out error), () => error);
        }

        internal uint GetFieldCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_GetFieldCount(Handle, out error), () => error);
        }

        internal VariableReflection GetFieldByIndex(uint index)
        {
            string? error = null;
            return new VariableReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetFieldByIndex(Handle, index, out error), () => error));
        }

        internal bool IsArray()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_IsArray(Handle, out error), () => error);
        }

        internal TypeReflection UnwrapArray()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeReflection_UnwrapArray(Handle, out error), () => error));
        }

        internal nuint GetElementCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_GetElementCount(Handle, out error), () => error);
        }

        internal TypeReflection GetElementType()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetElementType(Handle, out error), () => error));
        }

        internal uint GetRowCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_GetRowCount(Handle, out error), () => error);
        }

        internal uint GetColumnCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_GetColumnCount(Handle, out error), () => error);
        }

        internal ScalarType GetScalarType()
        {
            string? error = null;
            return (ScalarType)Call(() => SlangNativeInterop.TypeReflection_GetScalarType(Handle, out error), () => error);
        }

        internal TypeReflection GetResourceResultType()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetResourceResultType(Handle, out error), () => error));
        }

        internal int GetResourceShape()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_GetResourceShape(Handle, out error), () => error);
        }

        internal int GetResourceAccess()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_GetResourceAccess(Handle, out error), () => error);
        }

        internal string GetName()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_GetName(Handle, out error), () => error);
        }

        internal uint GetUserAttributeCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeReflection_GetUserAttributeCount(Handle, out error), () => error);
        }

        internal AttributeReflection GetUserAttributeByIndex(uint index)
        {
            string? error = null;
            return new AttributeReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetUserAttributeByIndex(Handle, index, out error), () => error));
        }

        internal AttributeReflection FindAttributeByName(string name)
        {
            string? error = null;
            return new AttributeReflection(
                this,
                Call(() => StrongTypeInterop.TypeReflection_FindAttributeByName(Handle, name, out error), () => error));
        }

        internal TypeReflection ApplySpecializations(GenericReflection genRef)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeReflection_ApplySpecializations(Handle, genRef.Handle, out error), () => error));
        }

        internal GenericReflection GetGenericContainer()
        {
            string? error = null;
            return new GenericReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetGenericContainer(Handle, out error), () => error));
        }
    }
}
